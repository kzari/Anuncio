using Lopes.Botmaker.Application.DadosServices;
using Lopes.Botmaker.Application.Models;
using Lopes.Domain.Commons;
using System.Collections.Concurrent;

namespace Lopes.Botmaker.Application.Services
{
    public class IntegracaoAppService : IIntegracaoAppService
    {
        private readonly IIntegracaoBotmakerDadosService _dados;
        private readonly IBotmakerApiService _botmakerApi;
        private readonly ILogger _logger;
        private int _usuariosNovos = 0;

        public IntegracaoAppService(IBotmakerApiService botmakerApi,
                                    ILogger logger,
                                    IIntegracaoBotmakerDadosService dados)
        {
            _botmakerApi = botmakerApi;
            _logger = logger;
            _dados = dados;
        }


        public IEnumerable<UsuarioIntegracao> ObterUsuarios()
        {
            IResultado<IEnumerable<UsuarioBotmakerApi>> resultUsuariosBotmaker = _botmakerApi.ObterUsuariosNaBotmaker();
            if (resultUsuariosBotmaker.Falha)
                throw new Exception($"Ocorreu um erro ao obter os usuários na API da Botmaker: {resultUsuariosBotmaker.ErrosConcatenados()}");

            List<UsuarioBotmakerApi> usuariosBotmaker = resultUsuariosBotmaker.Dado.Where(_ => _.extraValues != null && !string.IsNullOrEmpty(_.extraValues.CpfCorretor))
                                                                                   .ToList();

            IEnumerable<DadosUsuarioDTO> usuariosParaIntegrar = _dados.ObterUsuariosIntegracao().ToList();

            List<UsuarioIntegracao> usuarios = new List<UsuarioIntegracao>();
            List<string> emails = new List<string>();
            foreach (DadosUsuarioDTO usuario in usuariosParaIntegrar)
            {
                UsuarioBotmakerApi usuarioBotmaker = usuariosBotmaker.FirstOrDefault(_ => _.extraValues?.CpfCorretor != null && _.email == usuario.Email);
                if (usuarioBotmaker == null)
                {
                    usuarios.Add(new UsuarioIntegracao(usuario));
                }
                else
                {
                    usuarios.Add(new UsuarioIntegracao(usuario, usuarioBotmaker));
                }
            }

            //Adicionando usuários para excluir
            foreach (UsuarioBotmakerApi usuarioBotmaker in usuariosBotmaker)
            {
                if (!usuarios.Any(_ => _.UsuarioBotmaker != null && _.UsuarioBotmaker.email == usuarioBotmaker.email))
                    usuarios.Add(new UsuarioIntegracao(usuarioBotmaker));
            }

            return usuarios;
        }


        public void IntegrarTudo()
        {
            IEnumerable<UsuarioBotmakerApi> usuariosBotmaker = ObterUsuariosNaBotmaker();

            RemoverUsuariosDuplicados(ref usuariosBotmaker);

            IEnumerable<DadosUsuarioDTO> usuariosParaIntegrar = _dados.ObterUsuariosIntegracao().ToList();

            IncluirAtualizarUsuarios(usuariosBotmaker, usuariosParaIntegrar, removerAntesDeIncluir: false);

            //RemoverUsuarios(usuariosBotmaker, usuariosParaIntegrar);
        }


        /// <summary>
        /// Resgata as informações dos usuários que estão na Botmaker
        /// </summary>
        /// <param name="emails">Filtrar por e-mails</param>
        /// <param name="cpfs">Filtrar por CPFs</param>
        /// <param name="nomes">Filtrar por nomes</param>
        /// <returns></returns>
        private IEnumerable<UsuarioBotmakerApi> ObterUsuariosNaBotmaker(string[] emails = null, string[] cpfs = null, string[] nomes = null)
        {
            IResultado<IEnumerable<UsuarioBotmakerApi>> resultUsuariosBotmaker = _botmakerApi.ObterUsuariosNaBotmaker();
            if (resultUsuariosBotmaker.Falha)
                throw new Exception($"Ocorreu um erro ao obter os usuários na API da Botmaker: {resultUsuariosBotmaker.ErrosConcatenados()}");

            List<UsuarioBotmakerApi> usuariosBotmaker = resultUsuariosBotmaker.Dado.Where(_ => _.extraValues != null && !string.IsNullOrEmpty(_.extraValues.CpfCorretor))
                                                                                   .ToList();

            Info($"{usuariosBotmaker.Count()} usuários na Botmaker.");

            int qtdeAntes = usuariosBotmaker.Count;
            BotmakerApiService.FiltrarUsuarios(ref usuariosBotmaker, emails: emails, cpfs: cpfs, nomes: nomes);

            if (qtdeAntes > usuariosBotmaker.Count)
                Info($"Usuários no Botmaker filtrados para {usuariosBotmaker.Count}.");

            return usuariosBotmaker;
        }


        private IResultadoItens IncluirAtualizarUsuarios(IEnumerable<UsuarioBotmakerApi> usuariosBotmaker, IEnumerable<DadosUsuarioDTO> usuariosParaIntegrar, bool removerAntesDeIncluir)
        {
            Info("Inserindo/Atualizando usuários");

            Info($"{usuariosParaIntegrar.Count()} usuários encontrados para serem intregrados.");

            List<DadosUsuarioDTO> usuariosInserirAtualizar = ObterUsuariosInserirAtualizar(usuariosParaIntegrar, usuariosBotmaker).ToList();

            VerificarUsuariosParaIntegracaoDuplicados(usuariosInserirAtualizar);

            Info($"{_usuariosNovos} usuários para inserir. {usuariosInserirAtualizar.Count() - _usuariosNovos} usuários para atualizar. Total: {usuariosInserirAtualizar.Count()}");

            IResultadoItens execItemsResult = InserirAtualizarUsuarios(usuariosInserirAtualizar, removerAntesDeIncluir: removerAntesDeIncluir);

            Info($"{execItemsResult.ItensSucesso} usuários inseridos/atualizados com sucesso. {execItemsResult.ItensErro} usuários com erro ({string.Join(",", execItemsResult.Erros)})");

            return execItemsResult;
        }


        /// <summary>
        /// Retorna os usuários que devem ser incluídos ou atualizados
        /// </summary>
        /// <param name="usuarios"></param>
        /// <param name="usuariosBotmaker"></param>
        /// <returns></returns>
        private IEnumerable<DadosUsuarioDTO> ObterUsuariosInserirAtualizar(IEnumerable<DadosUsuarioDTO> usuarios, IEnumerable<UsuarioBotmakerApi> usuariosBotmaker)
        {
            foreach (DadosUsuarioDTO usuario in usuarios)
            {
                UsuarioBotmakerApi usuarioBotmaker = usuariosBotmaker.FirstOrDefault(_ => _.extraValues?.CpfCorretor != null && _.email == usuario.Email);
                if (usuarioBotmaker == null)
                {
                    _usuariosNovos++;
                    _logger.Info($"Usuários novo: {usuario.Apelido} - {usuario.Email}.");
                    yield return usuario;
                }
                else
                {
                    IEnumerable<string> alteracoes = UsuarioAlterado(usuario, usuarioBotmaker);
                    if (alteracoes.Any())
                    {
                        _logger.Info($"Usuário alterado: {usuario.Apelido} - {usuario.Email} - Alteração: {string.Join(" | ", alteracoes)}.");
                        yield return usuario;
                    }
                }
            }
        }

        /// <summary>
        /// Retorna as diferenças entre o usuário da Botmaker e do usuário para integrar
        /// </summary>
        /// <param name="usuarioInserirAtualizar">Usuário para integrar</param>
        /// <param name="usuarioBotmaker">Usuário na botmaker</param>
        /// <returns></returns>
        private static IEnumerable<string> UsuarioAlterado(DadosUsuarioDTO usuarioInserirAtualizar, UsuarioBotmakerApi usuarioBotmaker)
        {
            IList<string> alteracoes = new List<string>();

            if (usuarioInserirAtualizar.NomeSupervisor?.ToLower() != usuarioBotmaker.extraValues?.top_name?.ToLower())
                alteracoes.Add($"Nome do Supervisor alterado de '{usuarioBotmaker.extraValues?.top_name?.ToLower() ?? ""}' para '{usuarioInserirAtualizar.NomeSupervisor ?? ""}'");

            if (usuarioInserirAtualizar.Email.ToLower() != usuarioBotmaker.email.ToLower())
                alteracoes.Add($"E-mail alterado de '{usuarioBotmaker.email.ToLower()}' para '{usuarioInserirAtualizar.Email}'");

            if (usuarioInserirAtualizar.Nome?.ToUpper() != usuarioBotmaker.name?.ToUpper())
                alteracoes.Add($"Nome alterado de '{usuarioBotmaker.name ?? ""}' para '{usuarioInserirAtualizar.Nome ?? ""}'");

            if (usuarioInserirAtualizar.Apelido?.ToUpper() != usuarioBotmaker.extraValues?.Apelido?.ToUpper())
                alteracoes.Add($"Nome alterado de '{usuarioBotmaker.extraValues?.Apelido ?? ""}' para '{usuarioInserirAtualizar.Apelido ?? ""}'");

            if (usuarioInserirAtualizar.EmailDiretor?.ToUpper() != usuarioBotmaker.extraValues?.EmailDiretor?.ToUpper())
                alteracoes.Add($"E-mail do diretor alterado de '{usuarioBotmaker.extraValues?.EmailDiretor ?? ""}' para '{usuarioInserirAtualizar.EmailDiretor ?? ""}'");

            if (usuarioInserirAtualizar.EmailSupervisor?.ToUpper() != usuarioBotmaker.extraValues?.EmailSupervisor?.ToUpper())
                alteracoes.Add($"E-mail do Supervisor alterado de '{usuarioBotmaker.extraValues?.EmailSupervisor ?? ""}' para '{usuarioInserirAtualizar.EmailSupervisor ?? ""}'");

            return alteracoes;
        }

        private string[] VerificarUsuariosParaIntegracaoDuplicados(List<DadosUsuarioDTO> usuariosInserirAtualizar)
        {
            List<string> agrupado = usuariosInserirAtualizar.GroupBy(_ => _.Email)
                                                            .Where(_ => _.Count() > 1)
                                                            .Select(_ => _.Key)
                                                            .ToList();
            if (agrupado.Any())
            {
                _logger.Warn($"--- {agrupado.Count} usuários duplicados (verificar procedure): {string.Join(" | ", agrupado)}");
            }

            return agrupado.ToArray();
        }


        private void RemoverUsuariosDuplicados(ref IEnumerable<UsuarioBotmakerApi> usuariosBotmaker)
        {
            Info("Removendo usuários duplicados...");
            var usuariosDuplicados = usuariosBotmaker.Where(_ => _.extraValues?.CpfCorretor != null)
                                                     .GroupBy(_ => _.email)
                                                     .Where(_ => _.Count() > 1)
                                                     .Select(_ => new { Email = _.Key, D = _.ToArray() });
            string[] emailUsuarios = usuariosDuplicados.Select(_ => _.Email).ToArray();
            if (!emailUsuarios.Any())
            {
                Info($"Nenhum usuário duplicado");
            }
            else
            {
                RemoverUsuarios(emailUsuarios);
            }
            usuariosBotmaker = usuariosBotmaker.Where(_ => !emailUsuarios.Contains(_.email)).ToList();
        }

        /// <summary>
        /// Atualiza/Insere as informações dos usuários na Botmaker
        /// </summary>
        public IResultadoItens InserirAtualizarUsuarios(IEnumerable<DadosUsuarioDTO> usuarios, bool removerAntesDeIncluir)
        {
            IList<IEnumerator<DadosUsuarioDTO>> partitions = GetListPartitions(usuarios);

            int partitionIds = 0;
            int i = 0;
            IResultadoItens execItemsResult = new ResultadoItens();

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                using (partition)
                    while (partition.MoveNext())
                    {
                        DadosUsuarioDTO current = partition.Current;
                        var model = new BotmakerInserirAtualizarUsuario(cpf: current.CPF,
                                                                        email: current.Email,
                                                                        nome: current.Nome,
                                                                        cdEmpresa: current.CdEmpresa,
                                                                        emailDiretor: current.EmailDiretor ?? string.Empty,
                                                                        emailSupervisor: current.EmailSupervisor ?? string.Empty,
                                                                        nomeCargo: current.NomeCargo ?? string.Empty,
                                                                        apelido: current.Apelido,
                                                                        nomeSupervisor: current.NomeSupervisor ?? string.Empty);

                        if (removerAntesDeIncluir)
                            _botmakerApi.RemoverUsuarioChatbox(model.Email);

                        IResultado resposta = _botmakerApi.AtualizarUsuarioChatbox(model);
                        if (resposta.Sucesso)
                        {
                            string msg = $"{++i} P{partitionId,2} - Usuário integrado com sucesso ({model.Alias} - {model.Email})";
                            _logger.Info(msg);
                        }
                        else
                        {
                            string msgErro = $"{++i} P{partitionId,2} - Erro ao integrar Usuário ({model.Alias} - {model.Email}). Descrição do Erro: ({resposta.ErrosConcatenados()})";
                            _logger.Error(msgErro);
                        }
                        execItemsResult.Add(resposta.Sucesso);
                    }
            })).ToArray();
            Task.WaitAll(tasks);

            return execItemsResult;
        }

        /// <summary>
        /// Remove da Botmaker os usuários inativos
        /// </summary>
        /// <param name="dataHoraLimiteIntegracao"></param>
        public void RemoverUsuarios(string[] emailUsuarios)
        {
            if (emailUsuarios == null || emailUsuarios.All(_ => string.IsNullOrWhiteSpace(_)))
                return;

            Info($"Removendo da Botmaker {emailUsuarios.Length} usuários.");

            IList<IEnumerator<string>> partitions = GetListPartitions(emailUsuarios);

            int partitionIds = 0;
            int i = 0;
            int sucesso = 0;

            // Create a task for each partition.
            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                using (partition)   // Remember, the IEnumerator<T> implementation might implement IDisposable.
                    while (partition.MoveNext()) // While there are items in p.
                    {
                        // Get the current item.
                        string email = partition.Current;
                        string partitionStr = $" P{partitionId,2}";

                        IResultado exec = _botmakerApi.RemoverUsuarioChatbox(email);
                        if (exec.Sucesso)
                        {
                            string mensagem = $"{++i} P{partitionId,2} - Usuário removido da Botmaker com sucesso ({email})";
                            _logger.Info(mensagem);
                        }
                        else
                        {
                            string mensagem = $"{++i} P{partitionId,2} - Erro ao remover o usuário da Botmaker ({email}): {exec.ErrosConcatenados()}";
                            _logger.Error(mensagem);
                        }
                        if (exec.Sucesso)
                            sucesso++;
                    }
            })).ToArray(); // ToArray is needed (or something to materialize the list) to avoid deferred execution.
            Task.WaitAll(tasks);

            Info($"{sucesso} usuários removidos de {emailUsuarios.Count()}.");
        }

        private static void Info(string mensagem)
        {
            Console.WriteLine(mensagem);
        }

        private static IList<IEnumerator<T>> GetListPartitions<T>(IEnumerable<T> items)
        {
            // Get the partitioner.
            OrderablePartitioner<T> partitioner = Partitioner.Create(items);

            int qtdeProcessadores = 1;// Environment.ProcessorCount;

            // Get the partitions.
            // You'll have to set the parameter for the number of partitions here.
            // See the link for creating custom partitions for more creation strategies.
            IList<IEnumerator<T>> partitions = partitioner.GetPartitions(qtdeProcessadores);

            return partitions;
        }
    }
}
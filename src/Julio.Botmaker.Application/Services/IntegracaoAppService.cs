using Julio.Botmaker.Application.DadosServices;
using Julio.Botmaker.Application.Models;
using Julio.Domain.Commons;
using Julio.Domain.Commons.Cache;
using System.Collections.Concurrent;

namespace Julio.Botmaker.Application.Services
{
    public class IntegracaoAppService : IIntegracaoAppService
    {
        private readonly IIntegracaoBotmakerDadosService _dados;
        private readonly IBotmakerApiService _botmakerApi;
        private readonly ICacheService _cache;
        private ILogger _logger;

        public IntegracaoAppService(IBotmakerApiService botmakerApi,
                                    ILogger logger,
                                    IIntegracaoBotmakerDadosService dados,
                                    ICacheService cache)
        {
            _botmakerApi = botmakerApi;
            _logger = logger;
            _dados = dados;
            _cache = cache;
        }


        public IEnumerable<UsuarioIntegracao> ObterUsuarios()
        {
            List<UsuarioBotmakerApi> usuariosBotmaker = ObterUsuariosBotmaker(null);
            IEnumerable<DadosUsuarioDTO> usuariosParaIntegrar = ObterUsuarioParaIntegracao(null);
            return MontarUsuariosIntegracao(usuariosBotmaker, usuariosParaIntegrar);
        }
        public IEnumerable<UsuarioIntegracao> ObterUsuarios(TimeSpan duracaoCacheBotmaker, TimeSpan duracaoCacheBd)
        {
            List<UsuarioBotmakerApi> usuariosBotmaker = ObterUsuariosBotmaker(duracaoCacheBotmaker);
            IEnumerable<DadosUsuarioDTO> usuariosParaIntegrar = ObterUsuarioParaIntegracao(duracaoCacheBd);
            return MontarUsuariosIntegracao(usuariosBotmaker, usuariosParaIntegrar);
        }

        public IResultadoItens IntegrarUsuarios(ILogger? log = null)
        {
            if (log != null)
                _logger = log;

            List<UsuarioIntegracao> usuarios = ObterUsuarios().ToList();

            RemoverUsuariosDuplicados(ref usuarios);

            IResultadoItens resultado = InserirAtualizar(usuarios);
            IResultadoItens resultadoExclusoes = RemoverUsuarios(usuarios);

            return resultado.Add(resultadoExclusoes);
        }

        public IResultadoItens EnviarUsuarios(string[] emails)
        {
            IEnumerable<DadosUsuarioDTO> usuariosParaIntegrar = _dados.ObterUsuariosIntegracao(emails).ToList();

            return InserirAtualizarUsuarios(usuariosParaIntegrar, false);
        }





        private IResultadoItens RemoverUsuarios(List<UsuarioIntegracao> usuarios)
        {
            string[] emailUsuariosRemover = usuarios.Where(_ => _.Remover).Select(_ => _.Email).ToArray();
            
            _logger.Info($"Removendo da Botmaker {emailUsuariosRemover.Length} usuários.");
            IResultadoItens resultadoExclusoes = RemoverUsuarios(emailUsuariosRemover);

            _logger.Info($"{resultadoExclusoes.ItensSucesso} usuários removidos de {emailUsuariosRemover.Length}.");

            return resultadoExclusoes;
        }

        private IResultadoItens InserirAtualizar(List<UsuarioIntegracao> usuarios)
        {
            List<UsuarioIntegracao> usuariosInserirAtualizar = usuarios.Where(_ => _.Novo || _.Atualizar).ToList();

            _logger.Info($"{usuariosInserirAtualizar.Count(_ => _.Novo)} usuários para inserir. " +
                         $"{usuariosInserirAtualizar.Count(_ => _.Atualizar)} usuários para atualizar. " +
                         $"Total: {usuariosInserirAtualizar.Count}");

            List<DadosUsuarioDTO> usuariosSistema = usuariosInserirAtualizar.Select(_ => _.UsuarioSistema).ToList();
            VerificarUsuariosParaIntegracaoDuplicados(usuariosSistema);
            IResultadoItens execItemsResult = InserirAtualizarUsuarios(usuariosSistema, removerAntesDeIncluir: false);

            _logger.Info($"{execItemsResult.ItensSucesso} usuários inseridos/atualizados com sucesso. {execItemsResult.ItensErro} usuários com erro ({string.Join(",", execItemsResult.Erros)})");

            return execItemsResult;
        }

        /// <summary>
        /// Remove os usuários duplicados na Botmaker
        /// </summary>
        /// <param name="usuarios"></param>
        private void RemoverUsuariosDuplicados(ref List<UsuarioIntegracao> usuarios)
        {
            string[] emailsUsuariosDuplicados = usuarios.Select(_ => _.UsuarioBotmaker)
                                                        .Where(_ => _?.extraValues?.CpfCorretor != null)
                                                        .GroupBy(_ => _.email)
                                                        .Where(_ => _.Count() > 1)
                                                        .Select(_ => _.Key)
                                                        .ToArray();
            if (!emailsUsuariosDuplicados.Any())
            {
                _logger.Info($"Nenhum usuário duplicado");
            }
            else
            {
                _logger.Info("Removendo usuários duplicados...");
                RemoverUsuarios(emailsUsuariosDuplicados);
            }

            usuarios = usuarios.Where(_ => !emailsUsuariosDuplicados.Contains(_.UsuarioBotmaker?.email)).ToList();
        }



        private static List<UsuarioIntegracao> MontarUsuariosIntegracao(List<UsuarioBotmakerApi> usuariosBotmaker, IEnumerable<DadosUsuarioDTO> usuariosParaIntegrar)
        {
            List<UsuarioIntegracao> usuarios = new List<UsuarioIntegracao>();

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

        private IEnumerable<DadosUsuarioDTO> ObterUsuarioParaIntegracao(TimeSpan? duracaoCacheBd = null)
        {
            List<DadosUsuarioDTO> usuarios;
            if (duracaoCacheBd.HasValue)
            {
                usuarios = _cache.Obter<List<DadosUsuarioDTO>>("UsuariosIntegracao");
                if (usuarios != null)
                    return usuarios;
            }

            usuarios = _dados.ObterUsuariosIntegracao().ToList();

            if (duracaoCacheBd.HasValue)
                _cache.Gravar("UsuariosIntegracao", usuarios, duracaoCacheBd.Value);

            return usuarios;
        }

        private List<UsuarioBotmakerApi> ObterUsuariosBotmaker(TimeSpan? duracaoCacheBotmaker = null)
        {
            List<UsuarioBotmakerApi>? usuarios = null;
            if (duracaoCacheBotmaker.HasValue)
            {
                usuarios = _cache.Obter<List<UsuarioBotmakerApi>>("UsuariosBotmaker");
                if (usuarios != null)
                    return usuarios;
            }

            IResultado<IEnumerable<UsuarioBotmakerApi>> resultUsuariosBotmaker = _botmakerApi.ObterUsuariosNaBotmaker();
            if (resultUsuariosBotmaker.Falha)
                throw new Exception($"Ocorreu um erro ao obter os usuários na API da Botmaker: {resultUsuariosBotmaker.ErrosConcatenados()}");

            usuarios = resultUsuariosBotmaker.Dado.Where(_ => _.extraValues != null && !string.IsNullOrEmpty(_.extraValues.CpfCorretor))
                                                  .ToList();

            if (duracaoCacheBotmaker.HasValue)
                _cache.Gravar("UsuariosBotmaker", usuarios, duracaoCacheBotmaker.Value);

            return usuarios;
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
        public IResultadoItens RemoverUsuarios(string[] emailUsuarios)
        {
            if (emailUsuarios == null || emailUsuarios.All(_ => string.IsNullOrWhiteSpace(_)))
                return new ResultadoItens();

            var resultado = new ResultadoItens();

            foreach (string email in emailUsuarios)
            {
                IResultado exec = RemoverUsuario(email);
                resultado.Add(exec);
            }

            return resultado;
        }

        public IResultado RemoverUsuario(string email)
        {
            IResultado exec = _botmakerApi.RemoverUsuarioChatbox(email);
            if (exec.Sucesso)
            {
                string mensagem = $"Usuário removido da Botmaker com sucesso ({email})";
                _logger.Info(mensagem);
            }
            else
            {
                string mensagem = $"Erro ao remover o usuário da Botmaker ({email}): {exec.ErrosConcatenados()}";
                _logger.Error(mensagem);
            }

            return exec;
        }

        private static IList<IEnumerator<T>> GetListPartitions<T>(IEnumerable<T> items)
        {
            // Get the partitioner.
            OrderablePartitioner<T> partitioner = Partitioner.Create(items);

            int qtdeProcessadores = Environment.ProcessorCount;

            // Get the partitions.
            // You'll have to set the parameter for the number of partitions here.
            // See the link for creating custom partitions for more creation strategies.
            IList<IEnumerator<T>> partitions = partitioner.GetPartitions(qtdeProcessadores);

            return partitions;
        }
    }
}
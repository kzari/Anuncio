using Julio.Botmaker.Application.Models;
using Julio.Domain.Commons;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Julio.Botmaker.Application.Services
{
    /// <summary>
    /// Consume os métodos da API da Botmaker
    /// Swagger: https://go.botmaker.com/apidocs
    /// </summary>
    public class BotmakerApiService : IBotmakerApiService
    {
        private readonly string _urlUpdate;
        private readonly string _urlRemove;
        private readonly string _urlList;
        private readonly string _urlLogin;
        private readonly HttpClient _httpClient;

        public BotmakerApiService(string token, string urlApi)
        {
            _urlUpdate = $"{urlApi}/operator/update";
            _urlRemove = $"{urlApi}/operator/remove";
            _urlList = $"{urlApi}/operator/list";
            _urlLogin = $"{urlApi}/operator/autoLoginToken";
            _httpClient = GetHttpClient(token);
            Token = token;
        }


        public string Token { get; }


        public IResultado<IEnumerable<UsuarioBotmakerApi>> ObterUsuariosNaBotmaker()
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
            HttpResponseMessage response = _httpClient.PostAsync(_urlList, content).Result;

            if (!response.IsSuccessStatusCode)
            {
                string mensagem = TratarResponse(response);
                return Resultado<IEnumerable<UsuarioBotmakerApi>>.ComErro(mensagem);
            }

            string json = response.Content.ReadAsStringAsync().Result;

            ListaUsuariosBotmaker root = JsonParaListaUsuarioBotmaker(json);

            return new Resultado<IEnumerable<UsuarioBotmakerApi>>(root.active);
        }

        public IResultado RemoverUsuarioChatbox(string email)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { });
            content.Headers.Add("email", email);

            HttpResponseMessage response = _httpClient.PostAsync(_urlRemove, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return new Resultado();
            }
            else
            {
                string mensagem = TratarResponse(response);
                return Resultado.ComErro(mensagem);
            }
        }

        public IResultado AtualizarUsuarioChatbox(BotmakerInserirAtualizarUsuario item)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
            content.Headers.Add("email", item.Email);
            content.Headers.Add("name", item.Name);
            content.Headers.Add("lang", item.Lang);
            content.Headers.Add("groups", item.Groups);
            content.Headers.Add("queues", item.Queues.ToString());
            content.Headers.Add("priority", item.Priority);
            content.Headers.Add("extravalues", item.ExtraValues);
            content.Headers.Add("showMyChats", item.ShowMyCharts);
            content.Headers.Add("alias", item.Alias);
            content.Headers.Add("slots", item.Slots.ToString());
            content.Headers.Add("password", "lopes@123");

            if (!string.IsNullOrEmpty(item.Role))
                content.Headers.Add("role", item.Role);

            else if (!string.IsNullOrEmpty(item.CustomRole))
                content.Headers.Add("customRole", item.CustomRole);

            HttpResponseMessage response = _httpClient.PostAsync(_urlUpdate, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return Resultado.ComSucesso;
            }
            else
            {
                string mensagem = TratarResponse(response);
                return Resultado.ComErro(mensagem);
            }
        }

        public IResultado<string> ObterTokenDeAcesso(string email)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
            content.Headers.Add("email", email);

            HttpResponseMessage response = _httpClient.PostAsync(_urlLogin, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                Token token = JsonConvert.DeserializeObject<Token>(json);
                return new Sucesso<string>(token.token);
            }
            else
            {
                string mensagem = TratarResponse(response);
                return new Falha<string>(mensagem);
            }
        }


        private HttpClient GetHttpClient(string token)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            httpclient.DefaultRequestHeaders.Add("access-token", $"{token}");
            return httpclient;
        }

        private static string TratarResponse(HttpResponseMessage response)
        {
            return $"{response.StatusCode} - {response.Content.ReadAsStringAsync()?.Result}";
        }

        private static ListaUsuariosBotmaker JsonParaListaUsuarioBotmaker(string resposta)
        {
            resposta = resposta.Replace("extraValues\": \"{", "extraValues\": {");
            resposta = resposta.Replace("\"}\",", "\"},");
            resposta = resposta.Replace("\\\"", "\"");
            resposta = resposta.Replace("EmailDiretor\":null}\"", "EmailDiretor\":null}");

            ListaUsuariosBotmaker root = JsonConvert.DeserializeObject<ListaUsuariosBotmaker>(resposta);
            return root;
        }

        public static void FiltrarUsuarios(ref List<UsuarioBotmakerApi> usuariosBotmaker, string[] emails = null, string[] cpfs = null, string[] nomes = null)
        {
            FiltrarPorEmail(ref usuariosBotmaker, emails);
            FiltrarPorCPF(ref usuariosBotmaker, cpfs);
            FiltrarPorNome(ref usuariosBotmaker, nomes);
        }
        private static void FiltrarPorEmail(ref List<UsuarioBotmakerApi> usuariosBotmaker, params string[] emails)
        {
            if (emails == null || emails.All(_ => string.IsNullOrEmpty(_)))
                return;

            emails = emails.Select(_ => _.ToLower()).ToArray();

            usuariosBotmaker = usuariosBotmaker.Where(_ => emails.Contains(_.email.ToLower())).ToList();
        }
        private static void FiltrarPorCPF(ref List<UsuarioBotmakerApi> usuariosBotmaker, params string[] cpfs)
        {
            if (cpfs == null || cpfs.All(_ => string.IsNullOrEmpty(_)))
                return;

            usuariosBotmaker = usuariosBotmaker.Where(_ => cpfs.Contains(_.extraValues?.CpfCorretor)).ToList();
        }
        private static void FiltrarPorNome(ref List<UsuarioBotmakerApi> usuariosBotmaker, params string[] nomes)
        {
            if (nomes == null || nomes.All(_ => string.IsNullOrEmpty(_)))
                return;

            nomes = nomes.Select(_ => _.ToLower()).ToArray();

            usuariosBotmaker = usuariosBotmaker.Where(_ => nomes.Contains(_.name.ToLower())).ToList();
        }
    }
}

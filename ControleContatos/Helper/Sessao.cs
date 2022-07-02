using ControleContatos.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ControleContatos.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext; 
        }
        public UsuarioModel BuscarSessaoDoUsuario()
        {
            string SessaoUsuario = _httpContext.HttpContext.Session.GetString("SessaoUsuarioLogado");

            if (string.IsNullOrEmpty(SessaoUsuario)) return null;

            return JsonConvert.DeserializeObject<UsuarioModel>(SessaoUsuario);

        }

        public void CriarSessaoDoUsuario(UsuarioModel usuario)
        {
            string valor = JsonConvert.SerializeObject(usuario);
            _httpContext.HttpContext.Session.SetString("SessaoUsuarioLogado",valor);
        }

        public void RemoverSessaoDoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("SessaoUsuarioLogado");
        }
    }
}

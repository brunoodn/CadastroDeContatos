using ControleContatos.Helper;
using ControleContatos.Models;
using ControleContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, 
                                ISessao sessao,
                                IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;

        }
        public IActionResult Index()
        {
            // se o usuario estiver logado, redirecionar para a Home
            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");
            return View();
        }

        public IActionResult RedefinirSenha()
        {
            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if(usuario != null)
                    {

                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            //Criando a sessão do usuario
                            _sessao.CriarSessaoDoUsuario(usuario);

                            return RedirectToAction("Index", "Home");
                        }

                        TempData["MensagemErro"] = "Senha do usuario é inválida. Tente novamente.";
                    }

                    TempData["MensagemErro"] = "Usuario e/ou senha inválido(s). Por favor tente novamente.";
                }
                return View("Index");
            }
            catch (System.Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, Não conseguimos realizar o seu login.  Erro :{ erro.Message }";
                return RedirectToAction("Index");
            }

        }

        [HttpPost] 
        public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailELogin(redefinirSenhaModel.Email, redefinirSenhaModel.Login);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        
                        string mensagem = $"Sua nova senha é : {novaSenha}";

                        bool emailEnviado = _email.Enviar(usuario.Email, "Sistema de contatos - Nova senha", mensagem);

                        if (emailEnviado)
                        {
                            _usuarioRepositorio.Atualizar(usuario);
                            TempData["MensagemSucesso"] = "Enviamos para o e-mail cadastro uma nova senha.";
                        }
                        else
                        {
                            TempData["MensagemErro"] = "Não conseguimos Enviar o E-mail. Tente novamente.";
                        }
                        
                        return RedirectToAction("Index","Login");
                    }

                    TempData["MensagemErro"] = "Não conseguimos redefinir sua senha. Verifique os dados informados.";
                }
                return View("Index");
            }
            catch (System.Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, Não conseguimos redefinir sua senha.  Erro :{ erro.Message }";
                return RedirectToAction("Index");
            }
        }
    }
}

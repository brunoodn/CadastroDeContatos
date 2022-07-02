using ControleContatos.Models;

namespace ControleContatos.Helper
{
    public interface ISessao
    {
        void CriarSessaoDoUsuario(UsuarioModel usuario);
        void RemoverSessaoDoUsuario();

        UsuarioModel BuscarSessaoDoUsuario();
    }
}
 
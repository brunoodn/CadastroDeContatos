using System.ComponentModel.DataAnnotations;

namespace ControleContatos.Models
{
    public class RedefinirSenhaModel
    {
        [Required(ErrorMessage = "Digite o login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite o E-mail")]
        public string Email { get; set; }
    }
}

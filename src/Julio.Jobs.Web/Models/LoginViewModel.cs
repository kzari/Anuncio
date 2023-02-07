using System.ComponentModel.DataAnnotations;

namespace Julio.Jobs.Web.Models
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Necessário informar o login.")]
        public string Usuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Necessário informar a senha.")]
        public string Senha { get; set; }
    }
}
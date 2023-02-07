using System.ComponentModel.DataAnnotations;

namespace Julio.Jobs.Models;

public class LoginModel 
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Necessário informar o login.")]
    public string Usuario { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Necessário informar a senha.")]
    public string Senha { get; set; }
}


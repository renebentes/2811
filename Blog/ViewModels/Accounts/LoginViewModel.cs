using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O E-mail é inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "A Senha é obrigatória")]
    public string Password { get; set; } = null!;
}

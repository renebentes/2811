using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class RegisterUserViewModel
{
    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O E-mail é inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = null!;
}

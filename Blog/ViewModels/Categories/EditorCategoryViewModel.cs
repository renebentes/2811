using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "O slug é obrigatório")]
    public string Slug { get; set; } = null!;

    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(40, MinimumLength = 3, ErrorMessage = "O título deve conter entre 3 e 40 caracteres")]
    public string Title { get; set; } = null!;
}

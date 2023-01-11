using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    [HttpPost("signin")]
    public IActionResult SignIn([FromServices] TokenService tokenService)
        => Ok(tokenService.GenerateToken(null));

    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync([FromServices] BlogDataContext blogDataContext, [FromBody] RegisterUserViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-")
            };

            await blogDataContext.Users.AddAsync(user);
            await blogDataContext.SaveChangesAsync();

            return Created($"api/v1/accounts/{user.Id}", user);
        }
        catch (DbUpdateException)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<Category>("Houve um erro de banco de dados ao registrar um usuário."));
        }
        catch (Exception)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<Category>("Erro interno do servidor ao registrar um usuário."));
        }
    }
}

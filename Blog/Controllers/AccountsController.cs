using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Mvc;
using SecureIdentity.Password;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] LoginViewModel model,
                                                 [FromServices] BlogDataContext context,
                                                 [FromServices] TokenService tokenService)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = await context
                .Users
                .AsNoTracking()
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Email == model.Email);

            if (user is null)
            {
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos!"));
            }

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            {
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos!"));
            }

            var token = tokenService.GenerateToken(user);

            return Ok(new ResultViewModel<dynamic>(token, null!));
        }
        catch (Exception)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<Category>("Erro interno do servidor ao registrar um usuário."));
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync([FromServices] BlogDataContext blogDataContext,
                                                 [FromBody] RegisterUserViewModel model,
                                                 [FromServices] SmtpEmailService emailService)
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

            var password = PasswordGenerator.Generate();
            user.PasswordHash = PasswordHasher.Hash(password);

            _ = await blogDataContext.Users.AddAsync(user);
            _ = await blogDataContext.SaveChangesAsync();
            emailService.Send(user.Name, user.Email, "Bem-vindo ao blog", $"Sua senha é <strong>{password}</strong>");

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

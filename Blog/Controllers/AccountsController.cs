using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

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
            return StatusCode(500, new ResultViewModel<User>("Erro interno do servidor ao registrar um usuário."));
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
            return StatusCode(500, new ResultViewModel<User>("Houve um erro de banco de dados ao registrar um usuário."));
        }
        catch (Exception)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<User>("Erro interno do servidor ao registrar um usuário."));
        }
    }

    [Authorize]
    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromBody] UploadImageViewModel model, [FromServices] BlogDataContext context)
    {
        try
        {
            var fileName = $"{Guid.NewGuid()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
            var bytes = Convert.FromBase64String(data);

            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);

            var user = await context
                .Users
                .FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            if (user is null)
            {
                return NotFound(new ResultViewModel<string>("Usuário não encontrado!"));
            }

            user.Image = $"http://localhost:0000/images/{fileName}";

            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<User>("Houve um erro de banco de dados ao salvar imagem do usuário."));
        }
        catch (Exception)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<User>("Erro interno do servidor ao salvar a imagem do usuário."));
        }

        return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!", null));
    }
}

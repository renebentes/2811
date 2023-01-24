using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

builder
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();
builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>();

var app = builder.Build();

Configuration.ApiKey = app.Configuration.GetValue<string>(nameof(Configuration.ApiKey));
Configuration.ApiKeyName = app.Configuration.GetValue<string>(nameof(Configuration.ApiKeyName));
Configuration.JwtKey = app.Configuration.GetValue<string>(nameof(Configuration.JwtKey));

var smtpConfiguration = new SmtpConfiguration();
app.Configuration.GetSection(nameof(Configuration.SmtpConfiguration)).Bind(smtpConfiguration);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

var builder = WebApplication.CreateBuilder(args);

builder.AddJwtAuthentication();
builder.Services.AddControllers();
builder.AddServices();

var app = builder.Build();
app.Configuration.LoadConfiguration();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

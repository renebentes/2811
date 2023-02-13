using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddJwtAuthentication();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    });

builder.Services.AddMemoryCache();
builder.AddServices();

var app = builder.Build();
app.Configuration.LoadConfiguration();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();

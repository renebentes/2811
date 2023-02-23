using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadConfiguration();
builder.AddJwtAuthentication();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    });

builder.Services.AddMemoryCache();
builder.AddCompression();
builder.AddServices();
builder.AddSwagger();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.Run();

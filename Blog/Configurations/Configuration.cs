namespace Blog.Configurations;

public static class Configuration
{
    public static string ApiKey { get; set; } = null!;

    public static string ApiKeyName { get; set; } = null!;

    public static string JwtKey { get; set; } = null!;

    public static SmtpConfiguration SmtpConfiguration { get; set; } = null!;
}

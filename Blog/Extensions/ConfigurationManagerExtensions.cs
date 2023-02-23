namespace Blog.Extensions;

public static class ConfigurationManagerExtensions
{
    public static void LoadConfiguration(this ConfigurationManager configuration)
    {
        Configuration.ApiKey = configuration.GetValue<string>(nameof(Configuration.ApiKey));
        Configuration.ApiKeyName = configuration.GetValue<string>(nameof(Configuration.ApiKeyName));
        Configuration.JwtKey = configuration.GetValue<string>(nameof(Configuration.JwtKey));

        var smtpConfiguration = new SmtpConfiguration();
        configuration.GetSection(nameof(Configuration.SmtpConfiguration)).Bind(smtpConfiguration);
    }
}

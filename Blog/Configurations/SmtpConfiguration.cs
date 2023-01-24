namespace Blog.Configurations;

public class SmtpConfiguration
{
    public string Host { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int Port { get; set; } = 25;

    public string Username { get; set; } = string.Empty;
}

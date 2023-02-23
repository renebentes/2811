namespace Blog.Extensions;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder builder)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI();

        return builder;
    }
}

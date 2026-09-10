using Microsoft.Extensions.DependencyInjection;
 
public static class DependencyInjection
{
    public static IServiceCollection Add{{ServiceName}} (this IServiceCollection services)
    {
        {{DependencyRegistration}}
 
        return services;
    }
}
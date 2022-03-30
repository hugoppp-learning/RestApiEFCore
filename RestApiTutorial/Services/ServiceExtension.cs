namespace RestApiTutorial.Services;

public static class ServiceExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<ProjectService>()
            .AddScoped<TicketService>()
            ;
    }
}

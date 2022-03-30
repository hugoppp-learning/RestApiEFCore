namespace RestApiTutorial.Services;

public static class ServiceExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IProjectService, ProjectService>()
            .AddScoped<ITicketService, TicketService>()
            ;
    }
}

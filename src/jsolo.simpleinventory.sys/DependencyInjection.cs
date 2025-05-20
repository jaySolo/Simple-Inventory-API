using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using FluentValidation;
using MediatR;
using MediatR.Pipeline;

using jsolo.simpleinventory.sys.common.behaviours;


namespace jsolo.simpleinventory.sys;



public static class DependencyInjection
{
    public static IServiceCollection AddSystem(this IServiceCollection services)
    {

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));

        return services;
    }
}
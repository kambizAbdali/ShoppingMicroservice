using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Behaviors;
using System.Reflection;

namespace Ordering.Application.Dependencies
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // 1. Register MediatR along with all handlers in the current assembly
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(assembly));

            // 2. Automatically register all FluentValidation validators
            services.AddValidatorsFromAssembly(assembly);

            // 3. Register the validation and exception-handling pipeline behaviors for MediatR
            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(UnhandledExceptionBehavior<,>));

            // 4. Auto-scan and register all IRegister implementations (e.g. OrderMapper) in Mapster
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(assembly);

            // 5. Register Mapster services in the IoC container
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}

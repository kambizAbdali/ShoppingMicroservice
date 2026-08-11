using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddDbContext<OrderContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"))
            );
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}

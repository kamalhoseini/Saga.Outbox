using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure;

public static class DependencyContainer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString,
            optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(OrderContext).Assembly.FullName));
        });
        services.AddScoped<IOrderContext>(provider => provider.GetService<OrderContext>()!);
    }
}


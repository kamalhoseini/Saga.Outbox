using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Application;
using OrderService.Application.Orders.EventHandlers;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Saga;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplication(hostContext.Configuration);
        services.AddInfrastructure(hostContext.Configuration);

        services.AddDbContext<OrderContext>(x =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

            x.UseNpgsql(connectionString, options =>
            {
                options.MinBatchSize(1);
            });
        });
        var massTransitConfig = hostContext.Configuration.GetSection("MassTransit");

        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<OrderContext>(o =>
            {
                o.UsePostgres();

                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });

            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<OrderAcceptedEventHandler>();
            x.AddConsumer<OrderRejectedEventHandler>();
            x.AddConsumer<OrderSubmittedEventHandler>();

            x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<OrderContext>();
                    r.UsePostgres();
                });

            x.UsingRabbitMq((context, cfg) =>
            {
                //cfg.Host(massTransitConfig["Host"],
                //    h =>
                //    {
                //        h.Username(massTransitConfig["Username"]);
                //        h.Password(massTransitConfig["Password"]);
                //    }
                //);
                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

await host.RunAsync();
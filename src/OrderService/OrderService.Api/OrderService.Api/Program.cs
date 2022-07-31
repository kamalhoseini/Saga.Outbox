using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Application;
using OrderService.Application.Orders.EventHandlers;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var massTransitConfig = builder.Configuration.GetSection("MassTransit");
// MassTraansit configuration
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<OrderContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(1);

        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.AddConsumer<OrderAcceptedEventHandler>();
    x.AddConsumer<OrderRejectedEventHandler>();
    x.AddConsumer<OrderSubmittedEventHandler>();

    x.UsingRabbitMq((context, cfg) =>
    {
        //cfg.Host(massTransitConfig["Host"],
        //    h =>
        //    {
        //        h.Username(massTransitConfig["Username"]);
        //        h.Password(massTransitConfig["Password"]);
        //    }
        //);
        cfg.AutoStart = true;
        //cfg.ConfigureEndpoints(context);
    });
});

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//Auto update database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();

app.MapControllers();

app.Run();
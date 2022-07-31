using MassTransit;
using PaymentService.Api.EventHandlers;

var builder = WebApplication.CreateBuilder(args);

var massTransitConfig = builder.Configuration.GetSection("MassTransit");

// MassTraansit configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderSubmittedEventHandler>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(massTransitConfig["Host"],
            h =>
            {
                h.Username(massTransitConfig["Username"]);
                h.Password(massTransitConfig["Password"]);
            }
        );
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseRouting();

app.Run();

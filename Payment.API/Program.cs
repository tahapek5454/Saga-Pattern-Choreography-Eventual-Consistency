using MassTransit;
using Payment.API.Consumers;
using Shared.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<StockReservedEventConsumer>();

    configure.UsingRabbitMq((contex, configurator) =>
    {
        configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        configurator.ReceiveEndpoint(RabbitMQSettings.Payment_StockReservedEventQueue, e =>
        {
            e.ConfigureConsumer<StockReservedEventConsumer>(contex);
            e.DiscardSkippedMessages();
        });
    });
});

var app = builder.Build();



app.Run();

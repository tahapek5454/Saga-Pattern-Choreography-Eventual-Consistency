using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.RabbitMQ;
using Stock.API.Consumers;
using Stock.API.Models.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<OrderCreatedEventConsumer>();

    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
            e.DiscardSkippedMessages();
        });

    });
});


builder.Services.AddDbContext<StockAPIDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
});

var app = builder.Build();


app.Run();

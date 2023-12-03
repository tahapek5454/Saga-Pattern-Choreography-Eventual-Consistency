using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.RabbitMQ;
using Stock.API.Consumers;
using Stock.API.Models.DbContexts;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<OrderCreatedEventConsumer>();
    configure.AddConsumer<PaymentFailedEventConsumer>();

    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
            e.DiscardSkippedMessages();
        });

        configurator.ReceiveEndpoint(RabbitMQSettings.Stock_PaymentFailedEventQueue, e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
            e.DiscardSkippedMessages();
        });

    });
});


builder.Services.AddDbContext<StockAPIDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
});

var app = builder.Build();


// First Time Info
using IServiceScope scope = app.Services.CreateScope();

StockAPIDbContext? dbContext = scope.ServiceProvider.GetRequiredService<StockAPIDbContext>();

if (dbContext != null)
{
    var stocks = dbContext.Stocks.ToList();


    StringBuilder stringBuilder = new StringBuilder();

    stringBuilder.AppendLine("");
    stringBuilder.AppendLine("");
    stringBuilder.AppendLine($"Stokta Bulunan Ürün Numaralarý Listesi");

    int counter = 1;
    foreach (var stock in stocks)
    {
        stringBuilder.AppendLine($"{counter}-) {stock.ProductId}");

        counter++;
    }


    Console.WriteLine(stringBuilder.ToString());
}
else
{
    Console.WriteLine("Seed datalar þu an için gösterilemiyor");
}


// show product Id in stock each request
app.Use(async (context, next) =>
{
    
    using IServiceScope scope = app.Services.CreateScope();

    StockAPIDbContext? dbContext = scope.ServiceProvider.GetRequiredService<StockAPIDbContext>();

    if(dbContext != null)
    {
        var stocks = dbContext.Stocks.ToList();


        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"Stokta Bulunan Ürün Numaralarý Listesi");

        int counter = 1;
        foreach (var stock in stocks)
        {
            stringBuilder.AppendLine($"{counter}-) {stock.ProductId}");

            counter++;
        }


         System.Console.WriteLine( stringBuilder.ToString() );  
    }
    else
    {
         System.Console.WriteLine("Seed datalar þu an için gösterilemiyor");
    }
    
    await next(context);
});

app.Run();



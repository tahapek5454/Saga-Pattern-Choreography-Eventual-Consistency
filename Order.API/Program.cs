using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models.Contexts;
using Order.API.ViewModels;
using Shared.Events;
using Shared.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<PaymentCompletedEventConsumer>();
    configure.AddConsumer<PaymentFailedEventConsumer>();   
    configure.AddConsumer<StockNotReservedEventConsumer>();



    configure.UsingRabbitMq((contex, configurator) =>
    {
        configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentCompletedEventQueue, e =>
        {
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(contex);
            e.DiscardSkippedMessages();
        });

        configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue, e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(contex);
            e.DiscardSkippedMessages();
        });

        configurator.ReceiveEndpoint(RabbitMQSettings.Order_StockNotReservedEventQueue, e =>
        {
            e.ConfigureConsumer<StockNotReservedEventConsumer>(contex);
            e.DiscardSkippedMessages();
        });
    });
});


builder.Services.AddDbContext<OrderAPIDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/create-order", async ([FromBody] CreateOrderVM vm, OrderAPIDbContext context, IPublishEndpoint publishEndpoint) =>
{
    Order.API.Models.Order order = new()
    {
        BuyerId = Guid.Parse(vm.BuyerId),
        OrderItems = vm.OrderItems.Select(x => new Order.API.Models.OrderItem()
        {
            ProductId = Guid.Parse(x.ProductId),
            Count = x.Count,
            Price = x.Price,
        }).ToList(),
        CreatedDate = DateTime.UtcNow,
        OrderStatus = Order.API.Enums.OrderStatus.Suspend,
        TotalPrice = vm.OrderItems.Sum(x => x.Price * x.Count),
    };

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();


    OrderCreatedEvent orderCreatedEvent = new()
    {
        BuyerId = order.BuyerId,
        OrderId = order.Id,
        TotalPrice = order.TotalPrice,
        OrderItems = order.OrderItems.Select(x => new Shared.Messages.OrderItemMessage()
        {
            Count = x.Count,
            Price = x.Price,
            ProductId = x.ProductId
        }).ToList()
    };

    await publishEndpoint.Publish(orderCreatedEvent);

});

app.Run();

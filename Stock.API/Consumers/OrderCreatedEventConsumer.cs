using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Shared.RabbitMQ;
using Stock.API.Models.DbContexts;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly StockAPIDbContext stockAPIDbContext;
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly IPublishEndpoint publishEndpoint;
        public OrderCreatedEventConsumer(StockAPIDbContext stockAPIDbContext, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            this.stockAPIDbContext = stockAPIDbContext;
            this.sendEndpointProvider = sendEndpointProvider;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new List<bool>();


            var stocks = stockAPIDbContext.Stocks.AsQueryable();
            List<Stock.API.Models.Stock> updateStock = new();

            foreach (var orderItem in context.Message.OrderItems)
            {
                var isExisit = stocks.Where(x => x.ProductId == orderItem.ProductId && x.Count >=  orderItem.Count).Any();
                stockResult.Add(isExisit);
            }

            if(stockResult.TrueForAll(x => x.Equals(true)))
            {
                foreach (var orderItem in context.Message.OrderItems)
                {
                    var stock = await stocks.FirstAsync(x => x.ProductId == orderItem.ProductId );
                    stock.Count -= orderItem.Count;

                    updateStock.Add(stock);
                }

                stockAPIDbContext.Stocks.UpdateRange(updateStock);
                stockAPIDbContext.SaveChanges();


                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice,
                    OrderItems = context.Message.OrderItems
                };


                var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));

                await sendEndpoint.Send(stockReservedEvent);
            }
            else
            {

                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Stok Yetersiz"
                };


                await publishEndpoint.Publish(stockNotReservedEvent);
            }
        }
    }
}

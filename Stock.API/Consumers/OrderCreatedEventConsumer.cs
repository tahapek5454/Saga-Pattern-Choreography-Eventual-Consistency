using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.API.Models.DbContexts;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly StockAPIDbContext stockAPIDbContext;

        public OrderCreatedEventConsumer(StockAPIDbContext stockAPIDbContext)
        {
            this.stockAPIDbContext = stockAPIDbContext;
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

            }
            else
            {
                // throw 
            }
        }
    }
}

using MassTransit;
using Shared.Events;
using Stock.API.Models.DbContexts;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly StockAPIDbContext _dbContext;

        public PaymentFailedEventConsumer(StockAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var stocks = _dbContext.Stocks;

            List<Stock.API.Models.Stock> updateStock = new();

            foreach (var orderItem in context.Message.OrderItems)
            {
                var targetStock = stocks.First(x => x.ProductId == orderItem.ProductId);
                targetStock.Count += orderItem.Count;
                updateStock.Add(targetStock);
            }


            _dbContext.Stocks.UpdateRange(updateStock);
            await _dbContext.SaveChangesAsync();
        }
    }
}

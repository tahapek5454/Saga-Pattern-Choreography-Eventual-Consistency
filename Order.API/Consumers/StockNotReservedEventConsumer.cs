using MassTransit;
using Order.API.Models.Contexts;
using Shared.Events;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly OrderAPIDbContext _dbContext;

        public StockNotReservedEventConsumer(OrderAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var targetOrder = _dbContext.Orders.First(x => x.Id == context.Message.OrderId);


            targetOrder.OrderStatus = Enums.OrderStatus.Fail;

            _dbContext.Orders.Update(targetOrder);

            await _dbContext.SaveChangesAsync();
        }
    }
}

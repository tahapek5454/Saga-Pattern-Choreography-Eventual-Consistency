using MassTransit;
using Order.API.Models.Contexts;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly OrderAPIDbContext _dbContext;

        public PaymentCompletedEventConsumer(OrderAPIDbContext context)
        {
            _dbContext = context;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var targetOrder = _dbContext.Orders.First(x => x.Id == context.Message.OrderId);
            targetOrder.OrderStatus = Enums.OrderStatus.Completed;

            _dbContext.Orders.Update(targetOrder);  

            await _dbContext.SaveChangesAsync();
        }
    }
}

using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _endpoint;

        public StockReservedEventConsumer(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {

            var array = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Random rnd = new Random();
            int index = rnd.Next(array.Length);
                      
            if(index < 9)
            {
                // everything okey
                Console.WriteLine($"{context.Message.OrderId} numaralı {context.Message.TotalPrice} TL tutarlı ödeme başarılı bir " +
                    $"şekilde gerçekleşti");

                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };

                await _endpoint.Publish(paymentCompletedEvent);
            }
            else
            {
                Console.WriteLine($"{context.Message.OrderId} numaralı {context.Message.TotalPrice} TL tutarlı ödeme başarısız. ");

                PaymentFaildEvent paymentFaildEvent = new()
                {
                    Message = $"{context.Message.OrderId} numaralı {context.Message.TotalPrice} TL tutarlı ödeme başarısız. ",
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems
                };

                await _endpoint.Publish(paymentFaildEvent);

            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMQ
{
    public static class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock_order_created_event_queue";
        public const string Payment_StockReservedEventQueue = "payment_stock_reserved_event_queue";
        public const string Order_PaymentCompletedEventQueue = "order_payment_completed_event_queue";
    }
}

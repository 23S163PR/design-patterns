using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patterns_playground
{
    public class OrderService
    {
        public void PlaceOrder(Order order)
        {
            order.OrderId = new OrderIdGenerator().GetNextId();

            var inventory = new Inventory();
            inventory.CheckInventory(order.OrderId);

            var payment = new Payment();
            payment.DeductPayment(order.OrderId);
        }
    }

    public class Order
    {
        public string OrderId { get; set; }
        public List<string> Products { get; set; }
    }

    public class OrderIdGenerator
    {
        public string GetNextId()
        {
            var id = Convert.ToBase64String(Encoding.Default.GetBytes(Guid.NewGuid().ToString()));
            Console.WriteLine("Generated new order id: {0}", id);
            return id;
        }
    }

    public class Inventory
    {
        public void CheckInventory(string orderId)
        {
            Console.WriteLine("Inventory checked for order {0}.", orderId);
        }
    }

    public class Payment
    {
        public void DeductPayment(string orderId)
        {
            Console.WriteLine("Payment for order {0} completed.", orderId);
        }
    }
}

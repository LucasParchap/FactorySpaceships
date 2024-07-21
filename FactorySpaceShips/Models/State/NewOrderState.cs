namespace FactorySpaceships.Models.State;

public class NewOrderState : IOrderState
{
    public void HandleOrder(OrderContext context, string args)
    {
        var orderId = Guid.NewGuid().ToString();
        var items = args.Split(',')
            .Select(x => x.Trim())
            .Select(x => x.Split(' '))
            .ToDictionary(x => x[1], x => int.Parse(x[0]));

        var orderDetails = new OrderDetails(args, items);
        orderDetails.CurrentState = new FulfillOrderState();
        context.Orders.Add(orderId, orderDetails);
        Console.WriteLine($"Order {orderId} created with details: {args}");
    }

    public void SendOrder(OrderContext context, string orderId, Dictionary<string, int> items)
    {
        Console.WriteLine("Cannot send order in NewOrderState. Create an order first.");
    }

    public void ListOrders(OrderContext context)
    {
        /*foreach (var order in context.Orders)
        {
            if (order.Value.CurrentState is NewOrderState)
            {
                var remainingItems = string.Join(", ", order.Value.RemainingItems.Select(kv => $"{kv.Value} {kv.Key}"));
                Console.WriteLine($"b Order {order.Key}: {remainingItems}");
            }
        }*/
    }
}
namespace FactorySpaceships.Models.State;

public class FulfillOrderState : IOrderState
{
    public void HandleOrder(OrderContext context, string args) 
    {
        Console.WriteLine("Order is already being fulfilled.");
    }

    public void SendOrder(OrderContext context, string orderId, Dictionary<string, int> items)
    {
        var order = context.Orders[orderId];
        
        foreach (var item in items)
        {
            
            if (order.RemainingItems.ContainsKey(item.Key))
            {
                int availableQuantity = context.Inventory.Spaceships.Count(s => s.Type == item.Key);

                if (availableQuantity >= item.Value)
                {
                    order.RemainingItems[item.Key] -= item.Value;
                    context.Inventory.RemoveSpaceships(item.Key, item.Value);
                    context.Inventory.Notify($"SEND {item.Value} {item.Key}");

                    if (order.RemainingItems[item.Key] <= 0)
                    {
                        order.RemainingItems.Remove(item.Key);
                    }
                }
                else
                {
                    Console.WriteLine($"Not enough {item.Key} in stock to fulfill the order.");
                }
            }
            else
            {
                Console.WriteLine($"Item {item.Key} not found in the order or already fulfilled.");
            }
        }

        if (order.RemainingItems.Count > 0)
        {
            var remainingArgs = string.Join(", ", order.RemainingItems.Select(kvp => $"{kvp.Value} {kvp.Key}"));
            Console.WriteLine($"Remaining for {orderId} : {remainingArgs}");
        }
        else
        {
            context.Orders.Remove(orderId);
            order.CurrentState = new CompletedOrderState();
            Console.WriteLine($"COMPLETED {orderId}");
        }
    }

    public void ListOrders(OrderContext context)
    {
        /*foreach (var order in context.Orders)
        {
            if (order.Value.CurrentState is FulfillOrderState)
            {
                var remainingItems = string.Join(", ", order.Value.RemainingItems.Select(kv => $"{kv.Value} {kv.Key}"));
                Console.WriteLine($"a Order {order.Key}: {remainingItems}");
            }
        }*/
    }
}

namespace FactorySpaceships.Models.State;

public class OrderContext
{
    public Dictionary<string, OrderDetails> Orders { get; set; } = new Dictionary<string, OrderDetails>();
    public Inventory Inventory { get; set; }

    public OrderContext(Inventory inventory)
    {
        Inventory = inventory;
    }

    public void ProcessOrder(string args)
    {
        var newState = new NewOrderState();
        newState.HandleOrder(this, args);
    }

    public void SendOrder(string orderId, Dictionary<string, int> items)
    {
        if (!Orders.ContainsKey(orderId))
        {
            Console.WriteLine($"Order ID {orderId} does not exist.");
            return;
        }

        var order = Orders[orderId];
        var remainingItems = order.RemainingItems;

        foreach (var item in items)
        {
            if (remainingItems.ContainsKey(item.Key))
            {
                int availableQuantity = Inventory.Spaceships.Count(s => s.Type == item.Key);

                if (availableQuantity >= item.Value)
                {
                    remainingItems[item.Key] -= item.Value;
                    Inventory.RemoveSpaceships(item.Key, item.Value);

                    Inventory.Notify($"SEND {item.Value} {item.Key}");

                    if (remainingItems[item.Key] <= 0)
                    {
                        remainingItems.Remove(item.Key);
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

        if (remainingItems.Count > 0)
        {
            var remainingArgs = string.Join(", ", remainingItems.Select(kvp => $"{kvp.Value} {kvp.Key}"));
            Console.WriteLine($"Remaining for {orderId} : {remainingArgs}");
        }
        else
        {
            Orders.Remove(orderId);
            Console.WriteLine($"COMPLETED {orderId}");
        }
    }
    /*
    public void ListOrders()
    {
        foreach (var order in Orders)
        {
            order.Value.CurrentState.ListOrders(this);
        }
    }*/
    public void ListOrders()
    {
        foreach (var order in Orders)
        {
            var remainingItems = string.Join(", ", order.Value.RemainingItems.Select(kv => $"{kv.Value} {kv.Key}"));
            Console.WriteLine($"Order {order.Key}: {remainingItems}");
        }
    }
}
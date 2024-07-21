namespace FactorySpaceships.Models.State;

public class OrderContext
{
    public Dictionary<string, OrderDetails> Orders { get; set; } = new Dictionary<string, OrderDetails>();
    public Inventory Inventory { get; set; }
    private List<string> validSpaceships;

    public OrderContext(Inventory inventory)
    {
        Inventory = inventory;
        validSpaceships = inventory.GetValidSpaceships();
    }

    public void ProcessOrder(string args)
    {
        var newState = new NewOrderState();
        var invalidSpaceships = GetInvalidSpaceships(args);
        if (invalidSpaceships.Any())
        {
            Console.WriteLine("\u001b[31mERROR\u001b[0m : Invalid spaceship types in the order: " + string.Join(", ", invalidSpaceships));
            return;
        }
        newState.HandleOrder(this, args);
    }

    public void SendOrder(string orderId, Dictionary<string, int> items)
    {
        if (!Orders.ContainsKey(orderId))
        {
            Console.WriteLine($"\u001b[31mERROR\u001b[0m : Order ID {orderId} does not exist.");
            return;
        }

        var order = Orders[orderId];
        var remainingItems = order.RemainingItems;
        foreach (var item in items)
        {
            var itemKeyUpper = item.Key.ToUpper();
            var remainingItemKeyUpper = remainingItems.Keys.FirstOrDefault(k => k.ToUpper() == itemKeyUpper);

            if (remainingItemKeyUpper != null)
            {
                int availableQuantity = Inventory.Spaceships.Count(s => s.Type.ToUpper() == itemKeyUpper);

                if (availableQuantity >= item.Value)
                {
                    remainingItems[remainingItemKeyUpper] -= item.Value;
                    Inventory.RemoveSpaceships(remainingItemKeyUpper, item.Value);

                    Inventory.Notify($"SEND {item.Value} {item.Key}");

                    if (remainingItems[remainingItemKeyUpper] <= 0)
                    {
                        remainingItems.Remove(remainingItemKeyUpper);
                    }
                }
                else
                {
                    Console.WriteLine($"\u001b[31mERROR\u001b[0m : Not enough {item.Key} in stock to fulfill the order.");
                }
            }
            else
            {
                Console.WriteLine($"\u001b[31mERROR\u001b[0m : Item {item.Key} not found in the order or already fulfilled.");
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
            Console.WriteLine($"\u001b[38;5;214mCOMPLETED\u001b[0m {orderId}");
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
    private List<string> GetInvalidSpaceships(string args)
    {
        var items = args.Split(',')
            .Select(x => x.Trim())
            .Select(x => x.Split(' ')[1])
            .ToList();

        var invalidSpaceships = items.Where(item => !validSpaceships.Contains(item, StringComparer.OrdinalIgnoreCase)).ToList();
        return invalidSpaceships;
    }
}
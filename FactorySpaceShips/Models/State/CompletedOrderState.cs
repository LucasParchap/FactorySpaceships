namespace FactorySpaceships.Models.State;

public class CompletedOrderState : IOrderState
{
    public void HandleOrder(OrderContext context, string args)
    {
        Console.WriteLine("Order is already completed.");
    }

    public void SendOrder(OrderContext context, string orderId, Dictionary<string, int> items)
    {
        Console.WriteLine("Order is already completed. No items can be sent.");
    }

    public void ListOrders(OrderContext context)
    {
        // :)
    }
}
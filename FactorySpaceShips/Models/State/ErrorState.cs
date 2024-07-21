namespace FactorySpaceships.Models.State;

public class ErrorState : IOrderState
{
    private string errorMessage;

    public ErrorState(string message)
    {
        errorMessage = message;
    }

    public void HandleOrder(OrderContext context, string args)
    {
        Console.WriteLine($"Error processing the order: {errorMessage}");
    }

    public void SendOrder(OrderContext context, string orderId, Dictionary<string, int> items)
    {
        Console.WriteLine($"Error sending the order: {errorMessage}");
    }

    public void ListOrders(OrderContext context)
    {
        Console.WriteLine($"Error listing the orders: {errorMessage}");
    }
}
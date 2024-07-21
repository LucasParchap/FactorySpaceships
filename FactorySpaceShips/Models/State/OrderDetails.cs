namespace FactorySpaceships.Models.State;

public class OrderDetails
{
    public string Details { get; set; }
    public Dictionary<string, int> RemainingItems { get; set; }
    public IOrderState CurrentState { get; set; }

    public OrderDetails(string details, Dictionary<string, int> remainingItems)
    {
        Details = details;
        RemainingItems = remainingItems;
        CurrentState = new NewOrderState();
    }
}
namespace FactorySpaceships.Models.State;

public interface IOrderState
{
    void HandleOrder(OrderContext context, string args);
    void SendOrder(OrderContext context, string orderId, Dictionary<string, int> items);
    void ListOrders(OrderContext context);
}
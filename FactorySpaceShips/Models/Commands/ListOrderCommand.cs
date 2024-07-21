using FactorySpaceships.Models.State;

namespace FactorySpaceships.Models.Commands;

public class ListOrderCommand : ICommand
{
    private OrderContext _orderContext;

    public ListOrderCommand(OrderContext orderContext)
    {
        _orderContext = orderContext;
    }

    public void Execute()
    {
        _orderContext.ListOrders();
    }
}
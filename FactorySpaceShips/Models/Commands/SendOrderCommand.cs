using FactorySpaceships.Models;
using FactorySpaceships.Models.Commands;
using FactorySpaceships.Models.State;
namespace FactorySpaceships.Models.Commands;

public class SendOrderCommand : ICommand
{
    private readonly OrderContext _orderContext;
    private readonly string _orderId;
    private readonly Dictionary<string, int> _itemsToSend;

    public SendOrderCommand(OrderContext orderContext, string orderId, Dictionary<string, int> itemsToSend)
    {
        _orderContext = orderContext;
        _orderId = orderId;
        _itemsToSend = itemsToSend;
    }

    public void Execute()
    {
        _orderContext.SendOrder(_orderId, _itemsToSend);
    }
}
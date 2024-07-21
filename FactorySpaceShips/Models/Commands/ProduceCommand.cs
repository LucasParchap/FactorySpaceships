namespace FactorySpaceships.Models.Commands;

public class ProduceCommand : ICommand
{
    private Inventory _inventory;
    private Dictionary<string, int> _productionOrders;

    public ProduceCommand(Inventory inventory, Dictionary<string, int> productionOrders)
    {
        _inventory = inventory;
        _productionOrders = productionOrders;
    }

    public void Execute()
    {
        _inventory.ProduceCommand(_productionOrders);
    }
}

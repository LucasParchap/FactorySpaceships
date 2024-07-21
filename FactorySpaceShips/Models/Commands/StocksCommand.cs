namespace FactorySpaceships.Models.Commands;

public class StocksCommand : ICommand
{
    private Inventory _inventory;

    public StocksCommand(Inventory inventory)
    {
        _inventory = inventory;
    }

    public void Execute()
    {
        _inventory.SummarizeInventory();
    }
}

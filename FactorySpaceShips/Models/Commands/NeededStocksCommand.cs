namespace FactorySpaceships.Models.Commands;

public class NeededStocksCommand : ICommand
{
    private Inventory _inventory;
    private Dictionary<string, int> _items;

    public NeededStocksCommand(Inventory inventory, Dictionary<string, int> items)
    {
        _inventory = inventory;
        _items = items;
    }

    public void Execute()
    {
        _inventory.DisplayNeededStocks(_items);
    }
}
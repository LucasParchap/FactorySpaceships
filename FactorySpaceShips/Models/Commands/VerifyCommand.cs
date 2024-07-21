namespace FactorySpaceships.Models.Commands;

public class VerifyCommand : ICommand
{
    private Inventory _inventory;
    private Dictionary<string, int> _items;

    public VerifyCommand(Inventory inventory, Dictionary<string, int> items)
    {
        _inventory = inventory;
        _items = items;
    }

    public void Execute()
    {
        _inventory.VerifyCommand(_items);
    }
}
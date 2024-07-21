namespace FactorySpaceships.Models.Commands;

public class ReceiveCommand : ICommand
{
    private Inventory _inventory;
    private Dictionary<string, int> _itemsToReceive;

    public ReceiveCommand(Inventory inventory, Dictionary<string, int> itemsToReceive)
    {
        _inventory = inventory;
        _itemsToReceive = itemsToReceive;
    }

    public void Execute()
    {
        _inventory.ReceiveCommand(_itemsToReceive);
    }
}
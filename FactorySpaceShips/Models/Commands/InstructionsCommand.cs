namespace FactorySpaceships.Models.Commands;

public class InstructionsCommand : ICommand
{
    private Inventory _inventory;
    private Dictionary<string, int> _items;

    public InstructionsCommand(Inventory inventory, Dictionary<string, int> items)
    {
        _inventory = inventory;
        _items = items;
    }

    public void Execute()
    {
        _inventory.DisplayAssemblyInstructions(_items);
    }
}
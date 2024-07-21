namespace FactorySpaceships.Models.Commands;

public class GetMovementsCommand : ICommand
{
    private Inventory _inventory;
    private string[] _args;

    public GetMovementsCommand(Inventory inventory, string[] args)
    {
        _inventory = inventory;
        _args = args;
    }

    public void Execute()
    {
        if (_args == null || _args.Length == 0)
        {
            _inventory.DisplayAllMovements(); 
        }
        else
        {
            _inventory.DisplaySpecificMovements(_args); 
        }
    }
}
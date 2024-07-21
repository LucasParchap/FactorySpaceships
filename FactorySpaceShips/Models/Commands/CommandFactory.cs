namespace FactorySpaceships.Models.Commands;

public static class CommandFactory
{
    public static ICommand CreateCommand(string commandType, Inventory inventory, Dictionary<string, int> argumentsDict = null)
    {
        switch (commandType.ToUpper())
        {
            case "STOCKS":
                return new StocksCommand(inventory);
            case "NEEDED_STOCKS":
                return new NeededStocksCommand(inventory, argumentsDict);
            case "INSTRUCTIONS":
                return new InstructionsCommand(inventory, argumentsDict);
            case "VERIFY":
                return new VerifyCommand(inventory, argumentsDict);
            case "RECEIVE":
                return new ReceiveCommand(inventory, argumentsDict);
            case "PRODUCE":
                return new ProduceCommand(inventory, argumentsDict);
            default:
                throw new ArgumentException("Invalid command type");
        }
    }
}
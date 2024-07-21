using System.Security.AccessControl;
using FactorySpaceships.Models.State;

namespace FactorySpaceships.Models.Commands;

public static class CommandFactory
{
    public static ICommand CreateCommand(string commandType, Inventory inventory, OrderContext orderContext, string[] arguments = null, Dictionary<string, int> argumentsDict = null)
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
            case "ORDER":
                return new OrderCommand(orderContext, arguments);
            case "SEND":
                if (arguments.Length > 0)
                {
                    var orderId = arguments[0];
                    var itemsToSend = ConvertValidArgumentsToDictionary(arguments.Skip(1).ToArray());
                    return new SendOrderCommand(orderContext, orderId, itemsToSend);
                }
                else
                {
                    throw new ArgumentException("Invalid arguments for SEND command");
                }
            case "LIST_ORDER":
                return new ListOrderCommand(orderContext);
            default:
                throw new ArgumentException("Invalid command type");
        }
    }
    private static Dictionary<string, int> ConvertValidArgumentsToDictionary(string[] arguments)
    {
        var argumentsDictionary = new Dictionary<string, int>();
        if (arguments != null)
        {
            foreach (var argument in arguments)
            {
                var parts = argument.Split(new char[] { ' ' }, 2);
                if (parts.Length == 2 && int.TryParse(parts[0], out int quantity))
                {
                    var name = parts[1];
                    if (argumentsDictionary.ContainsKey(name))
                    {
                        argumentsDictionary[name] += quantity;
                    }
                    else
                    {
                        argumentsDictionary[name] = quantity;
                    }
                }
            }
        }
        return argumentsDictionary;
    }
}
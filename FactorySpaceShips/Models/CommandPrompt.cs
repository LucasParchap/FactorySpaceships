using FactorySpaceships.Error;
using FactorySpaceships.Models.Commands;
using FactorySpaceships.Models.Factories;
using FactorySpaceships.Models.Observer;

namespace FactorySpaceships.Models;

public class CommandPrompt
{
    public static Inventory Inventory;
    public static CommandLineErrorHandler CommandLineErrorHandler = new CommandLineErrorHandler();
    
    static CommandPrompt()
    {
        Inventory.ConfigureFactories(new HullFactory(), new EngineFactory(), new WingsFactory(), new ThrusterFactory());
        Inventory = Inventory.Instance;
        StockLogger logger = new StockLogger();
        Inventory.Attach(logger);
    }
    /*
     * Method to process commands
     */
    public static void Launch()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Please enter a command : ");
            string input = Console.ReadLine();
            string command = ExtractValidCommandFromInput(input);
            string[] arguments = ExtractValidArgumentsFromInput(input);

            if (CommandLineErrorHandler.ValidateArgumentsStructure(input))
            {
                ICommand cmd = null;

                if (command.ToUpper() == "GET_MOVEMENTS")
                {
                    cmd = new GetMovementsCommand(Inventory, arguments);
                }
                else if (command.ToUpper() != "EXIT")
                {
                    Dictionary<string, int> argumentsDict = ConvertValidArgumentsToDictionary(arguments);
                    cmd = CommandFactory.CreateCommand(command, Inventory, argumentsDict);
                }
                else if (command.ToUpper() == "EXIT")
                {
                    running = false;
                    continue;
                }

                cmd?.Execute();
            }
            else
            {
                CommandLineErrorHandler.PrintError();
            }
        }
    }

    public static string ExtractValidCommandFromInput(string input)
    {
        string[] parts = input.Split(' ');
        return parts[0];  
    }
    public static string[] ExtractValidArgumentsFromInput(string input)
    {
 
        string[] inputParts = input.Split(new char[] { ' ' }, 2);
        
        string[] arguments = null;
        if (inputParts.Length > 1)
        {
            string argumentStr = inputParts[1];
            arguments = argumentStr.Split(',')
                .Select(arg => arg.Trim()) 
                .Where(arg => !string.IsNullOrEmpty(arg))
                .ToArray();
        }
        return arguments; 
    }
    public static Dictionary<string, int> ConvertValidArgumentsToDictionary(string[] arguments)
    {
        Dictionary<string, int> argumentsDictionary = new Dictionary<string, int>();
        if (arguments != null)
        { 
            foreach (string argument in arguments)
            {
                string[] parts = argument.Split(new char[] { ' ' }, 2);
                if (parts.Length == 2 && int.TryParse(parts[0], out int quantity))
                {
                    string name = parts[1];
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
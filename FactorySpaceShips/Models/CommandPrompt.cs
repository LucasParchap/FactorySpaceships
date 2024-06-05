using FactorySpaceships.Error;

namespace FactorySpaceships.Models;

public class CommandPrompt
{
    public static Inventory Inventory = new Inventory();
    public static CommandLineErrorHandler CommandLineErrorHandler = new CommandLineErrorHandler();
    /*
     * Method to process commands
     */
    public static void Launch()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Please enter a command : ");
            String? input = Convert.ToString(Console.ReadLine());
            
            string command = ExtractValidCommandFromInput(input);
            string[] arguments;
            if (CommandLineErrorHandler.ValidateArgumentsStructure(input))
            {
                arguments = ExtractValidArgumentsFromInput(input);
                
                if (!CommandLineErrorHandler.ValidateArguments(arguments))
                {
                    CommandLineErrorHandler.PrintError();
                }
                else
                {
                    Dictionary<string, int> argumentsDict = ConvertValidArgumentsToDictionary(arguments);
                    switch (command.ToUpper())
                    {
                        case "STOCKS":
                            Inventory.SummarizeInventory();
                            break;
                        case "EXIT":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Unknown command. Please try again.");
                            break;
                    }
                }
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
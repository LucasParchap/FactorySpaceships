using System.Text.RegularExpressions;
using FactorySpaceships.Config;
using FactorySpaceships.Error;
using FactorySpaceships.Models.Commands;
using FactorySpaceships.Models.Factories;
using FactorySpaceships.Models.Observer;
using FactorySpaceships.Models.State;

namespace FactorySpaceships.Models;

public class CommandPrompt
{
    public static Inventory Inventory;
    public static CommandLineErrorHandler CommandLineErrorHandler = new CommandLineErrorHandler();
    private static OrderContext orderContext;
    
    static CommandPrompt()
    {
        Inventory.ConfigureFactories(new HullFactory(), new EngineFactory(), new WingsFactory(), new ThrusterFactory());
        Inventory = Inventory.Instance;
        orderContext = new OrderContext(Inventory);
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
            DisplayPrompt();
            string input = ReadLineWithColor(); 
            var (command, arguments, orderId) = ExtractCommandAndArguments(input);

            if (CommandLineErrorHandler.ValidateArgumentsStructure(input) && CommandLineErrorHandler.ValidateArguments(arguments))
            {
                
                ICommand cmd = null;
                if (command.ToUpper() == "GET_MOVEMENTS")
                {
                    cmd = new GetMovementsCommand(Inventory, arguments);
                }
                else if (command.ToUpper() == "SEND")
                {
                    cmd = new SendOrderCommand(orderContext,orderId, ConvertValidArgumentsToDictionary(arguments));
                }
                else if (command.ToUpper() != "EXIT")
                {
                    Dictionary<string, int> argumentsDict = ConvertValidArgumentsToDictionary(arguments);
                    cmd = CommandFactory.CreateCommand(command, Inventory, orderContext,arguments,argumentsDict);
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
    public static (string, string[], string) ExtractCommandAndArguments(string input)
    {
        string[] parts = input.Split(' ');
        string command = parts[0];
        string orderId = null;
        string[] arguments = null;

        if (command.ToUpper() == "SEND" && parts.Length > 1)
        {
            string[] orderAndArgs = input.Substring(5).Split(new char[] { ',' }, 2);
            if (orderAndArgs.Length == 2)
            {
                orderId = orderAndArgs[0].Trim();
                arguments = orderAndArgs[1].Split(',')
                    .Select(arg => arg.Trim())
                    .Where(arg => !string.IsNullOrEmpty(arg))
                    .ToArray();
            }
        }
        else
        {
            string[] inputParts = input.Split(new char[] { ' ' }, 2);
            if (inputParts.Length > 1)
            {
                arguments = inputParts[1].Split(',')
                    .Select(arg => arg.Trim())
                    .Where(arg => !string.IsNullOrEmpty(arg))
                    .ToArray();
            }
        }

        return (command, arguments, orderId);
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
   public static string ColorizeCommand(string input)
    {
        string[] commands = { "STOCKS", "NEEDED_STOCKS", "INSTRUCTIONS", "VERIFY", "PRODUCE", "ORDER", "SEND", "LIST_ORDER", "GET_MOVEMENTS", "RECEIVE" };
        foreach (var command in commands)
        {
            string pattern = $"\\b{command}\\b";
            string replacement = $"\u001b[32m{command}\u001b[0m";
            input = Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
        }
        return input;
    }

    private static string ReadLineWithColor()
    {
        var input = new System.Text.StringBuilder();
        ConsoleKeyInfo keyInfo;
        int cursorPosition = 0;

        while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length - 1, 1);
                cursorPosition--;
                ReprintInput(input.ToString(), cursorPosition);
            }
            else if (keyInfo.Key != ConsoleKey.Backspace)
            {
                input.Insert(cursorPosition, keyInfo.KeyChar);
                cursorPosition++;
                ReprintInput(input.ToString(), cursorPosition);
            }
        }

        Console.WriteLine();
        return input.ToString();
    }

    private static void ReprintInput(string input, int cursorPosition)
    {
        int currentLineCursor = Console.CursorTop;

        Console.SetCursorPosition(0, Console.CursorTop);
        ClearCurrentConsoleLine();
        Console.SetCursorPosition(0, currentLineCursor);
        Console.Write(ColorizeCommand(input));
        Console.SetCursorPosition(cursorPosition, currentLineCursor);
    }

    private static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    private static void DisplayPrompt()
    {
        string promptText = "Please enter a command:";
        int promptWidth = promptText.Length + 8; // 4 spaces padding on each side

        string topBottomBorder = new string('═', promptWidth);

        Console.WriteLine($"\u001b[36m╔{topBottomBorder}╗");
        Console.WriteLine($"║\u001b[33m    {promptText}    \u001b[36m║");
        Console.WriteLine($"╚{topBottomBorder}╝\u001b[0m");
    }












}
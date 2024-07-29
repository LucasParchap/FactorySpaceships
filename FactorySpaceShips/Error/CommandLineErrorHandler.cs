namespace FactorySpaceships.Error;

public class CommandLineErrorHandler : ErrorHandler
{
    private string message; 
    public override void PrintError()
    {
        Console.WriteLine(message);
    }

    public string GetErrorMessage()
    {
        return message;
    }
    
    public bool ValidateArgumentsStructure(string input)
    {
        string[] argumentsParts = input.Split(new char[] { ' ' }, 2);
        
        if (argumentsParts[0].ToUpper() == "GET_MOVEMENTS" || argumentsParts[0].ToUpper() == "LIST_ORDER" || argumentsParts[0].ToUpper() == "STOCKS" )
        {
            return true;
        }

        if (argumentsParts[0].ToUpper() == "SEND" && argumentsParts.Length > 1)
        {
            string[] orderAndArgs = argumentsParts[1].Split(new char[] { ',' }, 2);
            if (orderAndArgs.Length != 2 || string.IsNullOrWhiteSpace(orderAndArgs[0]) || string.IsNullOrWhiteSpace(orderAndArgs[1]))
            {
                message = "\u001b[31mERROR\u001b[0m : SEND command must include an order ID followed by a comma and the arguments.";
                return false;
            }

            string[] arguments = orderAndArgs[1].Split(',');
            foreach (string argument in arguments)
            {
                string trimmedArgument = argument.Trim();
                if (string.IsNullOrEmpty(trimmedArgument))
                {
                    message = "\u001b[31mERROR\u001b[0m : Empty argument detected. Ensure arguments are separated by a single comma without extra spaces.";
                    return false;
                }

                string[] parts = trimmedArgument.Split(' ');
                if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                {
                    message = $"\u001b[31mERROR\u001b[0m : Each argument must consist of exactly two parts: a quantity and a name, separated by one space. Problem found with '{trimmedArgument}'.";
                    return false;
                }
            }
        }
        else if (argumentsParts.Length < 2 || string.IsNullOrWhiteSpace(argumentsParts[1]))
        {
            message = "\u001b[31mERROR\u001b[0m : Invalid command structure.";
            return false;
        }
        else
        {
            string[] arguments = argumentsParts[1].Split(',');
            foreach (string argument in arguments)
            {
                string trimmedArgument = argument.Trim();
                if (string.IsNullOrEmpty(trimmedArgument))
                {
                    message = "\u001b[31mERROR\u001b[0m : Empty argument detected. Ensure arguments are separated by a single comma without extra spaces.";
                    return false;
                }

                string[] parts = trimmedArgument.Split(' ');
                if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                {
                    message = $"\u001b[31mERROR\u001b[0m : Each argument must consist of exactly two parts: a quantity and a name, separated by one space. Problem found with '{trimmedArgument}'.";
                    return false;
                }
            }
        }

        message = "";
        return true;
    }


    public bool ValidateArguments(string[] arguments)
    {

        if (arguments != null)
        {
            foreach (string arg in arguments)
            {
                string[] parts = arg.Split(' ');
                if (!int.TryParse(arg.Split(' ')[0], out _))
                {
                    message = "\u001b[31mERROR\u001b[0m : A numeric value is expected as the first argument.";
                    return false;
                }
                string namePart = parts[1];
                if (namePart.All(char.IsDigit))
                {
                    message = "\u001b[31mERROR\u001b[0m : A string value is expected as the second argument, not just numbers.";
                    return false;
                }
            } 
        }
        
        message = "";
        return true;
    }
    
}
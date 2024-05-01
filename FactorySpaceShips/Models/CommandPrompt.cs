namespace FactorySpaceships.Models;

public class CommandPrompt
{
    public static Inventory Inventory = new Inventory();
    /*
     * Method to process commands
     */
    public static void Launch()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Please enter a command : ");
            String? command = Convert.ToString(Console.ReadLine());
            switch (command?.ToUpper())
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
}
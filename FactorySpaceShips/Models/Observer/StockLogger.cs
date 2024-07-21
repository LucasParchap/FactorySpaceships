namespace FactorySpaceships.Models.Observer;

public class StockLogger : IObserver
{
    private List<string> movements = new List<string>();

    public void Update(string message)
    {
        movements.Add(message);
    }

    public void DisplayMovements()
    {
        foreach (var movement in movements)
        {
            Console.WriteLine(movement);
        }
    }

    public void DisplaySpecificMovements(string[] args)
    {
        foreach (var movement in movements)
        {
            if (args.Any(arg => movement.Contains(arg)))
            {
                Console.WriteLine(movement);
            }
        }
    }
}
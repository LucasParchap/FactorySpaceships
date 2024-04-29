namespace FactorySpaceships.Models;

public class Engine : Part
{
    private static int _nextSerialNumber;
    
    public Engine(string name): base(name) 
    {
    }
    protected override string GenerateSerialNumber()
    {
        return $"E{(++_nextSerialNumber).ToString("D5")}";
    }

}
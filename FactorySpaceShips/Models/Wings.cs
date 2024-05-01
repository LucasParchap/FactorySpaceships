namespace FactorySpaceships.Models;

public class Wings : Part
{
    private static int _nextSerialNumber;
    
    public Wings(string name): base(name) 
    {
    }
    protected override string GenerateSerialNumber()
    {
        return $"W{(++_nextSerialNumber).ToString("D5")}";
    }
    
}
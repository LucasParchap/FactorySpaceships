namespace FactorySpaceships.Models;

public class Thruster : Part
{
    private static int _nextSerialNumber;
    
    public Thruster(string name): base(name) 
    {
    }
    protected override string GenerateSerialNumber()
    {
        return $"T{(++_nextSerialNumber).ToString("D5")}";
    }
}
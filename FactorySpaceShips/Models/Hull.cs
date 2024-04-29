namespace FactorySpaceships.Models;

public class Hull : Part
{
    private static int _nextSerialNumber; 
    
    public Hull(string name): base(name)
    {
    }
    
    protected override string GenerateSerialNumber()
    {
        return $"H{(++_nextSerialNumber).ToString("D5")}";
    }
}
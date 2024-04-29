namespace FactorySpaceships.Models;

public abstract class Part
{
    public string? SerialNumber { get; private set; }
    public string Name { get; private set; }
    protected Part(string name)
    {
        Name = name;
        InitializeSerialNumber();
    }
    private void InitializeSerialNumber()
    {
        SerialNumber = GenerateSerialNumber();
    }
    
    protected abstract string GenerateSerialNumber();
    
    public override string ToString()
    {
        return $"{GetType().Name} - Serial Number: {SerialNumber}, Name: {Name}";
    }
}
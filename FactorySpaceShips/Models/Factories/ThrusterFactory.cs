namespace FactorySpaceships.Models.Factories;

public class ThrusterFactory : IPartFactory<Thruster>
{
    public Thruster CreatePart(string modelName)
    {
        return new Thruster(modelName);
    } 
}
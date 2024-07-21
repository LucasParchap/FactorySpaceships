namespace FactorySpaceships.Models.Factories;

public class WingsFactory : IPartFactory<Wings>
{
    public Wings CreatePart(string modelName)
    {
        return new Wings(modelName);
    }
}
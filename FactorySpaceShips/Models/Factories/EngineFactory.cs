namespace FactorySpaceships.Models.Factories;

public class EngineFactory : IPartFactory<Engine>
{
    public Engine CreatePart(string modelName)
    {
        return new Engine(modelName);
    }
}
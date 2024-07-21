namespace FactorySpaceships.Models.Factories;

public class HullFactory : IPartFactory<Hull>
{
    public Hull CreatePart(string modelName)
    {
        return new Hull(modelName);
    }
}
namespace FactorySpaceships.Models.Factories;

public interface IPartFactory<T> where T : Part
{
    T CreatePart(string specification);
}
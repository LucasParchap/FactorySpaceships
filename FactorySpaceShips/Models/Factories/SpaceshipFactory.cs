using FactorySpaceships.Config;

namespace FactorySpaceships.Models.Factories;

public class SpaceshipFactory
{
    private IPartFactory<Hull> hullFactory;
    private IPartFactory<Engine> engineFactory;
    private IPartFactory<Wings> wingsFactory;
    private IPartFactory<Thruster> thrusterFactory;

    public SpaceshipFactory(IPartFactory<Hull> hullF, IPartFactory<Engine> engineF, IPartFactory<Wings> wingsF, IPartFactory<Thruster> thrusterF)
    {
        hullFactory = hullF;
        engineFactory = engineF;
        wingsFactory = wingsF;
        thrusterFactory = thrusterF;
    }
    public Spaceship CreateSpaceshipFromConfig(SpaceshipConfig.SpaceshipData config)
    {
        Hull hull = null;
        Engine engine = null;
        Wings wings = null;
        List<Thruster> thrusters = new List<Thruster>();

        foreach (var part in config.Parts)
        {
            switch (part.Key.Split('_')[0])
            {
                case "Hull":
                    hull = hullFactory.CreatePart(part.Key);
                    break;
                case "Engine":
                    engine = engineFactory.CreatePart(part.Key);
                    break;
                case "Wings":
                    wings = wingsFactory.CreatePart(part.Key);
                    break;
                case "Thruster":
                    for (int i = 0; i < part.Value; i++)
                    {
                        thrusters.Add(thrusterFactory.CreatePart(part.Key));
                    }
                    break;
            }
        }
        return new Spaceship(config.Type, hull, engine, wings, thrusters);
    }
}

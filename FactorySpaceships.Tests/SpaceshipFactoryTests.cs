using FactorySpaceships.Config;
using FactorySpaceships.Models;
using FactorySpaceships.Models.Factories;
using Moq;


namespace FactorySpaceships.Tests;
public class SpaceshipFactoryTests
{
    private Mock<IPartFactory<Hull>> mockHullFactory;
    private Mock<IPartFactory<Engine>> mockEngineFactory;
    private Mock<IPartFactory<Wings>> mockWingsFactory;
    private Mock<IPartFactory<Thruster>> mockThrusterFactory;
    private SpaceshipFactory spaceshipFactory;

    public SpaceshipFactoryTests()
    {
        mockHullFactory = new Mock<IPartFactory<Hull>>();
        mockEngineFactory = new Mock<IPartFactory<Engine>>();
        mockWingsFactory = new Mock<IPartFactory<Wings>>();
        mockThrusterFactory = new Mock<IPartFactory<Thruster>>();

        spaceshipFactory = new SpaceshipFactory(mockHullFactory.Object, mockEngineFactory.Object, mockWingsFactory.Object, mockThrusterFactory.Object);
    }

    [Fact]
    public void CreateSpaceshipFromConfig_ShouldCreateCompleteSpaceship()
    {
        var config = new SpaceshipConfig.SpaceshipData
        {
            Type = "Explorer",
            Parts = new Dictionary<string, int>
            {
                { "Hull_HE1", 1 },
                { "Engine_EE1", 1 },
                { "Wings_WE1", 1 },
                { "Thruster_TE1", 1 }
            }
        };

        mockHullFactory.Setup(f => f.CreatePart("Hull_HE1")).Returns(new Hull("Hull_HE1"));
        mockEngineFactory.Setup(f => f.CreatePart("Engine_EE1")).Returns(new Engine("Engine_EE1"));
        mockWingsFactory.Setup(f => f.CreatePart("Wings_WE1")).Returns(new Wings("Wings_WE1"));
        mockThrusterFactory.Setup(f => f.CreatePart(It.IsAny<string>())).Returns(new Thruster("Thruster_TE1"));

        var spaceship = spaceshipFactory.CreateSpaceshipFromConfig(config);

        Assert.NotNull(spaceship);
        Assert.Equal("Explorer", spaceship.Type);
        Assert.NotNull(spaceship.Hull);
        Assert.NotNull(spaceship.Engine);
        Assert.NotNull(spaceship.Wings);
        Assert.Single(spaceship.Thrusters);
    }
}
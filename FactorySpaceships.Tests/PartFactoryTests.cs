using FactorySpaceships.Models.Factories;

namespace FactorySpaceships.Tests;

public class PartFactoryTests
{
    [Fact]
    public void HullFactory_ShouldCreateHull()
    {
        var hullFactory = new HullFactory();
        var hull = hullFactory.CreatePart("Hull_HE1");

        Assert.NotNull(hull);
        Assert.Equal("Hull_HE1", hull.Name);
    }

    [Fact]
    public void EngineFactory_ShouldCreateEngine()
    {
        var engineFactory = new EngineFactory();
        var engine = engineFactory.CreatePart("Engine_EE1");

        Assert.NotNull(engine);
        Assert.Equal("Engine_EE1", engine.Name);
    }

    [Fact]
    public void WingsFactory_ShouldCreateWings()
    {
        var wingsFactory = new WingsFactory();
        var wings = wingsFactory.CreatePart("Wings_WE1");

        Assert.NotNull(wings);
        Assert.Equal("Wings_WE1", wings.Name);
    }

    [Fact]
    public void ThrusterFactory_ShouldCreateThruster()
    {
        var thrusterFactory = new ThrusterFactory();
        var thruster = thrusterFactory.CreatePart("Thruster_TE1");

        Assert.NotNull(thruster);
        Assert.Equal("Thruster_TE1", thruster.Name);
    }
}
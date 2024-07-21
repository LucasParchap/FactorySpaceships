using FactorySpaceships.Models;
using FactorySpaceships.Models.Factories;
namespace FactorySpaceships.Tests;


public class FactoryTests
{
    private Inventory inventory;

    public FactoryTests()
    {
        SetUp();
    }
    public void SetUp()
    {
        Inventory.ConfigureFactories(new HullFactory(), new EngineFactory(), new WingsFactory(), new ThrusterFactory());
        inventory = Inventory.Instance;
        
        inventory.Spaceships.Clear();
        inventory.Parts.Clear();
        inventory.Assemblies.Clear();
        
        inventory.Parts.Add(new Hull("Hull_HE1"));
        inventory.Parts.Add(new Hull("Hull_HE1"));
        inventory.Parts.Add(new Hull("Hull_HE1"));

        inventory.Parts.Add(new Engine("Engine_EE1"));
        inventory.Parts.Add(new Engine("Engine_EE1"));
        inventory.Parts.Add(new Engine("Engine_EE1"));

        inventory.Parts.Add(new Wings("Wings_WE1"));
        inventory.Parts.Add(new Wings("Wings_WE1"));
        inventory.Parts.Add(new Wings("Wings_WE1"));

        inventory.Parts.Add(new Thruster("Thruster_TE1"));
        inventory.Parts.Add(new Thruster("Thruster_TE1"));
        inventory.Parts.Add(new Thruster("Thruster_TE1"));
    }
    [Fact]
    public void ProduceCommand_ShouldDecreasePartStockCorrectly()
    {
        var command = new Dictionary<string, int> { { "Explorer", 1 } };

        inventory.ProduceCommand(command);
        
        int hullCount = inventory.Parts.Count(p => p.Name == "Hull_HE1");
        int engineCount = inventory.Parts.Count(p => p.Name == "Engine_EE1");
        int wingsCount = inventory.Parts.Count(p => p.Name == "Wings_WE1");
        int thrusterCount = inventory.Parts.Count(p => p.Name == "Thruster_TE1");
        
        Assert.Equal(2, hullCount);
        Assert.Equal(2, engineCount);
        Assert.Equal(2, wingsCount);
        Assert.Equal(2, thrusterCount);
    }
    [Fact]
    public void ProduceCommand_ShouldAddSpaceshipToInventory()
    {
        var command = new Dictionary<string, int> { { "Explorer", 1 } };

        inventory.ProduceCommand(command);

        Assert.Single(inventory.Spaceships);
    }
}
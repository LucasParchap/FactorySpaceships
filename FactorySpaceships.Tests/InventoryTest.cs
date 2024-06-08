using FactorySpaceships.Models;
namespace FactorySpaceships.Tests;
public class InventoryTest
{
    static Hull hull_HE1 = new Hull("Hull_HE1");
    static Engine engine_EE1 = new Engine("Engine_EE1");
    static Wings wings_WE1 = new Wings("Wings_WE1");
    static Thruster thruster_TE1 = new Thruster("Thruster_TE1");

    static Hull hull_HS1 = new Hull("Hull_HS1");
    static Engine engine_ES1 = new Engine("Engine_ES1");
    static Wings wings_WS1 = new Wings("Wings_WS1");
    static Thruster thruster_TS1 = new Thruster("Thruster_TS1");
    static Thruster thruster_TS1_2 = new Thruster("Thruster_TS1");
    
    static Hull hull_HC1 = new Hull("Hull_HC1");
    static Engine engine_EC1 = new Engine("Engine_EC1");
    static Wings wings_WC1 = new Wings("Wings_WC1");
    static Thruster thruster_TC1 = new Thruster("Thruster_TC1");

    private Spaceship Explorer =
        new Spaceship("Explorer", hull_HE1, engine_EE1, wings_WE1, new List<Thruster>() { thruster_TE1 });

    private Spaceship Speeder = new Spaceship("Speeder", hull_HS1, engine_ES1, wings_WS1,
        new List<Thruster>() { thruster_TS1, thruster_TS1_2 });

    private Spaceship Cargo =
        new Spaceship("Cargo", hull_HC1, engine_EC1, wings_WC1, new List<Thruster>() { thruster_TC1 });

    private Assembly Assembly1 = new Assembly();
    private Assembly Assembly2 = new Assembly("Assembly2");
    

    private Inventory _inventory;
    
    [Fact]
    public void ShouldCorrectlyCountSpaceshipTypes()
    {
        _inventory = new Inventory();
        _inventory.Spaceships.Add(Explorer);
        _inventory.Spaceships.Add(Speeder);
        _inventory.Spaceships.Add(Cargo);
        
        var expectedCounts = new Dictionary<string, int>
        {
            { "Explorer", 1 },
            { "Speeder", 1 },
            { "Cargo", 1 }
        };

        var actualCounts = _inventory.GetSpaceshipTypeCounts();

        Assert.Equal(expectedCounts, actualCounts);
    }
    
    [Fact]
    public void ShouldCorrectlyCountParts()
    {
        _inventory = new Inventory();
        _inventory.Parts.Add(hull_HE1);
        _inventory.Parts.Add(hull_HE1);
        _inventory.Parts.Add(engine_EE1);
        _inventory.Parts.Add(wings_WE1);
        _inventory.Parts.Add(thruster_TE1);
        
        var expectedCounts = new Dictionary<string, int>
        {
            { "Hull_HE1", 2 },
            { "Engine_EE1", 1 },
            { "Wings_WE1", 1 },
            { "Thruster_TE1", 1 }
        };

        var actualCounts = _inventory.GetPartCounts();

        Assert.Equal(expectedCounts, actualCounts);
    }
    
    [Fact]
    public void ShouldCorrectlyDisplayAssemblies()
    {
        _inventory = new Inventory();
        
        Assembly1.AddPart(hull_HE1);
        Assembly1.AddPart(engine_EE1);
        
        Assembly2.AddPart(wings_WE1);
        Assembly2.AddPart(thruster_TE1);


        var expectedDescription1  = "[Hull_HE1, Engine_EE1]";
        var actualCounts1 = Assembly1.ToString();
        
        var expectedDescription2  = "Assembly2";
        var actualDescription2  = Assembly2.ToString();

        Assert.Equal(expectedDescription1 , actualCounts1);
        Assert.Equal(expectedDescription2 , actualDescription2);
    }
    [Fact]
    public void DisplayNeededStocks_ShouldOutputCorrectPartsForSpaceships()
    {
        var inventory = new Inventory();
        Dictionary<string, int> neededStocks = new Dictionary<string, int>
        {
            { "Explorer", 2 },
            { "Cargo", 1 }
        };
    
        string expectedOutput = 
            "2 Explorer:\n" +
            "2 Hull_HE1\n" +
            "2 Engine_EE1\n" +
            "2 Wings_WE1\n" +
            "2 Thruster_TE1\n\n" +
            "1 Cargo:\n" +
            "1 Hull_HC1\n" +
            "1 Engine_EC1\n" +
            "1 Wings_WC1\n" +
            "1 Thruster_TC1\n\n" + 
            "Total:\n" +
            "2 Hull_HE1\n" +
            "2 Engine_EE1\n" +
            "2 Wings_WE1\n" +
            "2 Thruster_TE1\n" +
            "1 Hull_HC1\n" +
            "1 Engine_EC1\n" +
            "1 Wings_WC1\n" +
            "1 Thruster_TC1";
    
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
        
            inventory.DisplayNeededStocks(neededStocks);
        
            var result = sw.ToString().Replace("\r\n", "\n").Trim(); 
            expectedOutput = expectedOutput.Replace("\r\n", "\n").Trim();
            
            Assert.Equal(expectedOutput, result);
        }
    }

    [Fact]
    public void DisplayNeededStocks_ShouldOutputNoConfigFoundMessage()
    {
        // Arrange
        var inventory = new Inventory(); 
        Dictionary<string, int> neededStocks = new Dictionary<string, int>
        {
            { "UnknownType", 3 }
        };

        string expectedMessage = "No configuration found for spaceship type 'UnknownType'.";
        
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            
            inventory.DisplayNeededStocks(neededStocks);
            
            var result = sw.ToString().Trim();
            
            Assert.Contains(expectedMessage, result);
        }
    }
}
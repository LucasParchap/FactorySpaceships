using Xunit;
using FactorySpaceships.Models;
using FactorySpaceships.Models.Commands;
using FactorySpaceships.Models.Observer;
using FactorySpaceships.Models.Factories;

namespace FactorySpaceships.Tests;

public class CommandTests
{
    private readonly Inventory _inventory;
    private ICommand _command;

    public CommandTests()
    {
        _inventory = Inventory.Instance;
        _inventory.Clear();
        _inventory.Attach(new StockLogger());
    }

    [Fact]
    public void StocksCommand_ShouldDisplayCurrentStocks()
    {
        _inventory.ReceiveCommand(new Dictionary<string, int> {{"Hull_HC1", 10}});
        
        _command = new StocksCommand(_inventory);
        _command.Execute();
        
    }

    [Fact]
    public void ReceiveCommand_ShouldUpdateInventoryCorrectly()
    {
        var commandData = new Dictionary<string, int> { { "Hull_HC1", 5 } };
        _command = new ReceiveCommand(_inventory, commandData);
        _command.Execute();
        
        Assert.Equal(5, _inventory.Parts.Count(p => p.Name == "Hull_HC1"));
    }

    [Fact]
    public void ProduceCommand_ShouldProduceSpaceshipsWhenEnoughPartsAvailable()
    {
        Inventory.ConfigureFactories(new HullFactory(), new EngineFactory(), new WingsFactory(), new ThrusterFactory());
        
        _inventory.ReceiveCommand(new Dictionary<string, int> {
            { "Hull_HC1", 1 },
            { "Engine_EC1", 1 },
            { "Wings_WC1", 1 },
            { "Thruster_TC1", 2 }
        });
        var commandData = new Dictionary<string, int> { { "Cargo", 1 } };
        _command = new ProduceCommand(_inventory, commandData);
        _command.Execute();
        
        Assert.Equal(1, _inventory.Spaceships.Count(s => s.Type == "Cargo"));
    }
    
}

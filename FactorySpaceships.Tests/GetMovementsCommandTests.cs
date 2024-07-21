using Xunit;
using FactorySpaceships.Models;
using FactorySpaceships.Models.Commands;
using FactorySpaceships.Models.Observer;
using System.Collections.Generic;
using FactorySpaceships.Models.Factories;

namespace FactorySpaceships.Tests;

public class GetMovementsCommandTests
{
    private readonly Inventory _inventory;
    private readonly MockObserver _observer;

    public class MockObserver : IObserver
    {
        public List<string> Movements { get; private set; } = new List<string>();

        public void Update(string message)
        {
            Movements.Add(message);
        }
    }

    public GetMovementsCommandTests()
    {
        _inventory = Inventory.Instance;
        Inventory.ConfigureFactories(new HullFactory(), new EngineFactory(), new WingsFactory(), new ThrusterFactory());
        _observer = new MockObserver();
        _inventory.Attach(_observer);
        _inventory.ReceiveCommand(new Dictionary<string, int> { { "Hull_HC1", 1 } });
        _inventory.ReceiveCommand(new Dictionary<string, int> { { "Engine_EC1", 1 } });
        _inventory.ReceiveCommand(new Dictionary<string, int> { { "Wings_WC1", 1 } });
        _inventory.ReceiveCommand(new Dictionary<string, int> { { "Thruster_TC1", 1 } });
        _inventory.ProduceCommand(new Dictionary<string, int> { { "Cargo", 1 } });
    }

    [Fact]
    public void GetMovements_ShouldDisplayAllMovements()
    {
        var command = new GetMovementsCommand(_inventory, null);
        command.Execute();
        
        Assert.Contains("RECEIVE 1 Hull_HC1", _observer.Movements);
        Assert.Contains("RECEIVE 1 Engine_EC1", _observer.Movements);
        Assert.Contains("RECEIVE 1 Wings_WC1", _observer.Movements);
        Assert.Contains("RECEIVE 1 Thruster_TC1", _observer.Movements);
        Assert.Contains("PRODUCED 1 Cargo spaceship(s).", _observer.Movements);
    }

    [Fact]
    public void GetMovements_ShouldDisplaySpecificMovements()
    {
        var command = new GetMovementsCommand(_inventory, new[] { "Hull_HC1" });
        command.Execute();
        
        Assert.Contains("RECEIVE 1 Hull_HC1", _observer.Movements);
    }
}

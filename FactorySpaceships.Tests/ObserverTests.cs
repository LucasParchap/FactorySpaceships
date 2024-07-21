using Xunit;
using FactorySpaceships.Models;
using FactorySpaceships.Models.Observer;
using System.Collections.Generic;

namespace FactorySpaceships.Tests;

public class MockObserver : IObserver
{
    public bool Notified { get; set; } = false;

    public void Update(string message)
    {
        Notified = true;
    }
}
public class ObserverTests
{
    private readonly Inventory _inventory;
    private readonly MockObserver _observer;

    public ObserverTests()
    {
        _inventory = Inventory.Instance;
        _observer = new MockObserver();
        _inventory.Attach(_observer);
    }

    [Fact]
    public void Observer_ShouldBeNotifiedWhenInventoryChanges()
    {
        _inventory.ReceiveCommand(new Dictionary<string, int> { { "Hull_HC1", 1 } });
        
        Assert.True(_observer.Notified, "Observer should have been notified of the inventory change.");
    }

    [Fact]
    public void Observer_ShouldNotBeNotifiedAfterDetachment()
    {
        _inventory.Detach(_observer);
        
        _inventory.ReceiveCommand(new Dictionary<string, int> { { "Hull_HC1", 1 } });
        
        _observer.Notified = false;
        
        Assert.False(_observer.Notified, "Observer should not be notified after being detached.");
    }
}
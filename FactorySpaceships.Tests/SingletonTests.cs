using FactorySpaceships.Models;

namespace FactorySpaceships.Tests;

public class SingletonTests
{
    [Fact]
    public void InventorySingleton_ShouldReturnSameInstance()
    {
        var instance1 = Inventory.Instance;
        var instance2 = Inventory.Instance;
    
        Assert.Same(instance1, instance2);
    }
    [Fact]
    public void InventorySingleton_ShouldPreserveStateAcrossInstances()
    {
        var instance1 = Inventory.Instance;
        instance1.Parts.Add(new Hull("Hull_HE1"));

        var instance2 = Inventory.Instance;
        var partNames = instance2.Parts.Select(part => part.Name).ToList();
        Assert.Contains("Hull_HE1", partNames);
    }
    [Fact]
    public void InventorySingleton_ShouldHandleConcurrentRequests()
    {
        Inventory instance1 = null;
        Inventory instance2 = null;

        var thread1 = new Thread(() =>
        {
            instance1 = Inventory.Instance;
        });

        var thread2 = new Thread(() =>
        {
            instance2 = Inventory.Instance;
        });

        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();

        Assert.Same(instance1, instance2);
    }
}
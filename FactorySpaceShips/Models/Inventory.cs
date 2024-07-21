using System;
using System.Collections.Generic;
using System.Text;
using FactorySpaceships.Config;
using FactorySpaceships.Models.Factories;
using FactorySpaceships.Models.Observer;

namespace FactorySpaceships.Models;

public sealed class Inventory : ISubject
{
    private static Inventory _instance;
    private static readonly object _lock = new object();
    private List<IObserver> _observers = new List<IObserver>();
    public List<Spaceship> Spaceships { get; private set; }
    public List<Part> Parts { get; private set; }
    public List<Assembly> Assemblies { get; private set; }
    
    private static SpaceshipFactory spaceshipFactory;
    private static HullFactory hullFactory;
    private static EngineFactory engineFactory;
    private static WingsFactory wingsFactory;
    private static ThrusterFactory thrusterFactory;
    
    private List<SpaceshipConfig.SpaceshipData> _configSpaceships;
    private HashSet<string> _configParts;
    private Inventory()
    {
        Spaceships = new List<Spaceship>();
        Parts = new List<Part>();
        Assemblies = new List<Assembly>();
        SpaceshipConfig spaceshipConfig = SpaceshipConfig.Instance;
        _configSpaceships = spaceshipConfig.LoadSpaceships();
        _configParts = new HashSet<string>(spaceshipConfig.LoadParts());
        
        hullFactory = new HullFactory();
        engineFactory = new EngineFactory();
        wingsFactory = new WingsFactory();
        thrusterFactory = new ThrusterFactory();
    }
    public static Inventory Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Inventory();
                    }
                }
            }
            return _instance;
        }
    }
    public static void ConfigureFactories(IPartFactory<Hull> hullF, IPartFactory<Engine> engineF, IPartFactory<Wings> wingsF, IPartFactory<Thruster> thrusterF)
    {
        spaceshipFactory = new SpaceshipFactory(hullF, engineF, wingsF, thrusterF);
    }
    /**
     * Group all the spaceships into a dictionary  (key = type  | value = counter)
     */
    public Dictionary<string, int> GetSpaceshipTypeCounts()
    {
        var typeCounts = new Dictionary<string, int>();
        foreach (var spaceship in Spaceships)
        {
            if (typeCounts.ContainsKey(spaceship.Type))
            {
                typeCounts[spaceship.Type]++;
            }
            else
            {
                typeCounts.Add(spaceship.Type, 1);
            }
        }
        return typeCounts;
    }
    /*
     * Display the number of spaceships categorized by type
     */
    public void DisplaySpaceshipTypeCounts()
    {
        var typeCounts = GetSpaceshipTypeCounts();
        foreach (var typeCount in typeCounts)
        {
            Console.WriteLine($"{typeCount.Value} {typeCount.Key}");
        }
    }
    /**
     * Group all the parts into a dictionary  (key = name  | value = counter)
     */
    public Dictionary<string, int> GetPartCounts()
    {
        var partCounts = new Dictionary<string, int>();
        foreach (var part in Parts)
        {
            if (partCounts.ContainsKey(part.Name))
            {
                partCounts[part.Name]++;
            }
            else
            {
                partCounts.Add(part.Name, 1);
            }
        }
        return partCounts;
    }
    /*
     * Display the number of parts categorized by name
     */
    public void DisplayPartCounts()
    {
        var partCounts = GetPartCounts();
        foreach (var count in partCounts)
        {
            Console.WriteLine($"{count.Value} {count.Key}");
        }
    }
    public void DisplayAssemblies()
    {
        foreach(var assembly in Assemblies)
        {
            Console.WriteLine(assembly.ToString());
        }
    }
    public void SummarizeInventory()
    {
        DisplaySpaceshipTypeCounts();
        DisplayPartCounts();
        DisplayAssemblies();
    }
    public void DisplayNeededStocks(Dictionary<string, int> neededStocks)
    {
        Dictionary<string, int> totalParts = new Dictionary<string, int>();

        foreach (var stock in neededStocks)
        {
            string spaceshipType = stock.Key;
            int quantity = stock.Value;

            var spaceship = _configSpaceships.Find(s => s.Type.Equals(spaceshipType, StringComparison.OrdinalIgnoreCase));
            if (spaceship != null)
            {
                Console.WriteLine();
                Console.WriteLine($"{quantity} {spaceshipType}:");
                foreach (var part in spaceship.Parts)
                {
                    int totalQuantity = part.Value * quantity;
                    Console.WriteLine($"{totalQuantity} {part.Key}");

                    if (totalParts.ContainsKey(part.Key))
                    {
                        totalParts[part.Key] += totalQuantity;
                    }
                    else
                    {
                        totalParts.Add(part.Key, totalQuantity);
                    }
                }
            }
            else
            {
                Console.WriteLine($"No configuration found for spaceship type '{spaceshipType}'.");
            }
        }

        // Display the total parts required for all spaceships combined
        Console.WriteLine("\nTotal:");
        foreach (var part in totalParts)
        {
            Console.WriteLine($"{part.Value} {part.Key}");
        }
    }
     public void DisplayAssemblyInstructions(Dictionary<string, int> assemblyInstructions)
    {
        foreach (var instruction in assemblyInstructions)
        {
            string spaceshipType = instruction.Key;
            int quantity = instruction.Value;
            var spaceship = _configSpaceships.Find(s => s.Type.Equals(spaceshipType, StringComparison.OrdinalIgnoreCase));

            if (spaceship != null)
            {
                for (int i = 0; i < quantity; i++)
                {
                    Console.WriteLine($"PRODUCING {spaceshipType}");
                    
                    foreach (var part in spaceship.Parts)
                    {
                        Console.WriteLine($"GET_OUT_STOCK {part.Value} {part.Key}");
                    }
                    
                    List<string> tempAssemblies = new List<string>();
                    
                    if (spaceship.Parts.Count > 0)
                    {
                        string lastAssembly = "TMP1";
                        tempAssemblies.Add(lastAssembly);
                        Console.WriteLine($"ASSEMBLE {lastAssembly} {spaceship.Parts.Keys.ElementAt(0)} {spaceship.Parts.Keys.ElementAt(1)}");

                        for (int j = 2; j < spaceship.Parts.Keys.Count; j++)
                        {
                            lastAssembly = $"TMP{j}";
                            tempAssemblies.Add(lastAssembly);
                            Console.WriteLine($"ASSEMBLE {lastAssembly} {tempAssemblies[tempAssemblies.Count - 2]} {spaceship.Parts.Keys.ElementAt(j)}");
                        }
                        
                        if (spaceship.Parts[spaceship.Parts.Keys.Last()] > 1)
                        {
                            for (int k = 1; k < spaceship.Parts[spaceship.Parts.Keys.Last()]; k++)
                            {
                                string newAssembly = $"TMP{tempAssemblies.Count + 1}";
                                Console.WriteLine($"ASSEMBLE {newAssembly} {lastAssembly} {spaceship.Parts.Keys.Last()}");
                                lastAssembly = newAssembly;
                                tempAssemblies.Add(newAssembly);
                            }
                        }
                        Console.WriteLine($"FINISHED {spaceshipType}");
                    }
                    else
                    {
                        Console.WriteLine($"Error: Not enough parts specified for {spaceshipType} to demonstrate complex assembly.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"No configuration found for spaceship type '{spaceshipType}'.");
            }
        }
    }
     
    public Dictionary<string, int> GetPartsInventoryDictionary()
    {
        Dictionary<string, int> partInventory = new Dictionary<string, int>();
        foreach (var part in Parts)
        {
            if (partInventory.ContainsKey(part.Name))
            {
                partInventory[part.Name]++;
            }
            else
            {
                partInventory.Add(part.Name, 1);
            }
        }
        return partInventory;
    }
    public void VerifyCommand(Dictionary<string, int> argumentsDict)
    {
        Dictionary<string, int> partInventory = GetPartsInventoryDictionary();
        bool isUnavailable = false;
        
        foreach (var item in argumentsDict)
        {
            string spaceshipType = item.Key;
            int quantityNeeded = item.Value;
            var spaceship = _configSpaceships.Find(s => s.Type.Equals(spaceshipType, StringComparison.OrdinalIgnoreCase));

            if (spaceship == null)
            {
                Console.WriteLine($"\u001b[31mERROR\u001b[0m `{spaceshipType}` is not a recognized spaceship");
                return;
            }
    
            foreach (var part in spaceship.Parts)
            {
                int requiredQuantity = part.Value * quantityNeeded;
                int availableQuantity = partInventory.ContainsKey(part.Key) ? partInventory[part.Key] : 0;

                if (availableQuantity < requiredQuantity)
                {
                    isUnavailable = true;
                }
            }
        }
        if (isUnavailable)
        {
            Console.WriteLine("\u001b[38;5;214mUNAVAILABLE\u001b[0m"); 
        }
        else
        {
            Console.WriteLine("\u001b[38;5;214mAVAILABLE\u001b[0m");
        }
    }

    public void ProduceCommand(Dictionary<string, int> command)
    {
        if (!VerifyCommandForProduction(command))
        {
            Console.WriteLine("\u001b[31mERROR\u001b[0m: Insufficient materials to produce the requested spaceships.");
            return;
        }

        bool productionCompleted = false;
        StringBuilder productionDetails = new StringBuilder();
        foreach (var item in command)
        {
            string spaceshipType = item.Key;
            int quantityNeeded = item.Value;
            var config = _configSpaceships.Find(s => s.Type.Equals(spaceshipType, StringComparison.OrdinalIgnoreCase));
        
            if (config != null)
            {
                for (int i = 0; i < quantityNeeded; i++)
                {
                    var spaceship = spaceshipFactory.CreateSpaceshipFromConfig(config);
                    Spaceships.Add(spaceship);
                }

                foreach (var part in config.Parts)
                {
                    int requiredQuantity = part.Value * quantityNeeded;
                    ReduceStock(part.Key, requiredQuantity);
                }
                productionCompleted = true;
                productionDetails.AppendFormat("PRODUCED {0} {1} spaceship(s). ", quantityNeeded, spaceshipType);
            }
        }

        if (productionCompleted)
        {
            Console.WriteLine("\u001b[38;5;214mSTOCK_UPDATED\u001b[0m");
            Notify(productionDetails.ToString().TrimEnd());
        }
    }


    private bool VerifyCommandForProduction(Dictionary<string, int> command)
    {
        Dictionary<string, int> partInventory = GetPartsInventoryDictionary();
        
        foreach (var item in command)
        {
            string spaceshipType = item.Key;
            var spaceship = _configSpaceships.Find(s => s.Type.Equals(spaceshipType, StringComparison.OrdinalIgnoreCase));
            if (spaceship == null) return false;

            
            foreach (var part in spaceship.Parts)
            {
                int requiredQuantity = part.Value * item.Value;
                if (!partInventory.TryGetValue(part.Key, out int availableQuantity) || availableQuantity < requiredQuantity)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void ReduceStock(string partName, int quantity)
    {
        for (int i = Parts.Count - 1; i >= 0; i--)
        {
            if (Parts[i].Name.Equals(partName, StringComparison.OrdinalIgnoreCase))
            {
                Parts.RemoveAt(i);
                quantity--;     
                if (quantity <= 0) break;  
            }
        }
    }
    public void ReceiveCommand(Dictionary<string, int> command)
    {
        HashSet<string> unknownItems = new HashSet<string>();
        bool updated = false;

        foreach (var item in command)
        {
            string partName = item.Key;
            int quantity = item.Value;
            bool partAdded = false;

            if (_configParts.Contains(partName))
            {
                for (int i = 0; i < quantity; i++)
                {
                    updated = true;
                    if (partName.StartsWith("Hull"))
                    {
                        Parts.Add(hullFactory.CreatePart(partName));
                    }
                    else if (partName.StartsWith("Engine"))
                    {
                        Parts.Add(engineFactory.CreatePart(partName));
                    }
                    else if (partName.StartsWith("Wings"))
                    {
                        Parts.Add(wingsFactory.CreatePart(partName));
                    }
                    else if (partName.StartsWith("Thruster"))
                    {
                        Parts.Add(thrusterFactory.CreatePart(partName));
                    }
                }
                partAdded = true;
            }
            else if (_configSpaceships.Any(s => s.Type.Equals(partName, StringComparison.OrdinalIgnoreCase)))
            {
                var config = _configSpaceships.Find(s => s.Type.Equals(partName, StringComparison.OrdinalIgnoreCase));
                for (int i = 0; i < quantity; i++)
                {
                    var spaceship = spaceshipFactory.CreateSpaceshipFromConfig(config);
                    if (spaceship != null)
                    {
                        updated = true;
                        Spaceships.Add(spaceship);
                    }
                }
                partAdded = true;
            }
            else
            {
                unknownItems.Add(partName);
            }

            if (partAdded) {
                Notify($"RECEIVE {quantity} {partName}");
            }
        }

        if (unknownItems.Count > 0)
        {
            foreach (var unknownItem in unknownItems)
            {
                Console.WriteLine($"\u001b[31mERROR\u001b[0m : Unknown part or spaceship: {unknownItem}");
            }
        }

        if (updated)
        {
            Console.WriteLine("\u001b[38;5;214mSTOCK_UPDATED\u001b[0m");
        }
        else
        {
            Console.WriteLine("\u001b[38;5;214mSTOCK_NOT_UPDATED\u001b[0m");
        }
    }
    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(string message)
    {
        foreach (IObserver observer in _observers)
        {
            observer.Update(message);
        }
    }
    public void DisplayAllMovements()
    {
        foreach (var observer in _observers)
        {
            if (observer is StockLogger logger)
            {
                logger.DisplayMovements();
            }
        }
    }

    public void DisplaySpecificMovements(string[] args)
    {
        foreach (var observer in _observers)
        {
            if (observer is StockLogger logger)
            {
                logger.DisplaySpecificMovements(args);
            }
        }
    }
    public void Clear()
    {
        Spaceships.Clear();
        Parts.Clear();
        Assemblies.Clear();
        _observers.Clear(); 
    }
    public void RemoveSpaceships(string type, int quantity)
    {
        for (int i = Spaceships.Count - 1; i >= 0 && quantity > 0; i--)
        {
            if (Spaceships[i].Type.Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                Spaceships.RemoveAt(i);
                quantity--;
            }
        }
    }
    public List<string> GetValidSpaceships()
    {
        return _configSpaceships.Select(s => s.Type).ToList();
    }
}
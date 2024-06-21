using System;
using System.Collections.Generic;
using FactorySpaceships.Config;

namespace FactorySpaceships.Models;

public class Inventory
{
    public List<Spaceship> Spaceships { get; private set; }
    public List<Part> Parts { get; private set; }
    public List<Assembly> Assemblies { get; private set; }
    
    private List<SpaceshipConfig.SpaceshipData> _configSpaceships;
    public Inventory()
    {
        Spaceships = new List<Spaceship>();
        Parts = new List<Part>();
        Assemblies = new List<Assembly>(); 
        SpaceshipConfig spaceshipConfig = new SpaceshipConfig();
        _configSpaceships = spaceshipConfig.LoadSpaceships();
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
                Console.WriteLine($"ERROR `{spaceshipType}` is not a recognized spaceship");
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
            Console.WriteLine("UNAVAILABLE");
        }
        else
        {
            Console.WriteLine("AVAILABLE");
        }
    }

    public void ProduceCommand(Dictionary<string, int> command)
    {
        if (!VerifyCommandForProduction(command))
        {
            Console.WriteLine("ERROR: Insufficient materials to produce the requested spaceships.");

            return;
        }

        foreach (var item in command)
        {
            string spaceshipType = item.Key;
            int quantityNeeded = item.Value;
            var config = _configSpaceships.Find(s => s.Type.Equals(spaceshipType, StringComparison.OrdinalIgnoreCase));
            
            for (int i = 0; i < quantityNeeded; i++)
            {
                var spaceship = CreateSpaceshipFromConfig(config);
                Spaceships.Add(spaceship);
            }

            foreach (var part in config.Parts)
            {
                int requiredQuantity = part.Value * quantityNeeded;
                ReduceStock(part.Key, requiredQuantity);
            }
        }

        Console.WriteLine("STOCK_UPDATED");
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
    public Spaceship CreateSpaceshipFromConfig(SpaceshipConfig.SpaceshipData config)
    {
        Hull hull = null;
        Engine engine = null;
        Wings wings = null;
        List<Thruster> thrusters = new List<Thruster>();

        foreach (var part in config.Parts)
        {
            switch (part.Key)
            {
                case "Hull":
                    hull = new Hull(part.Key);
                    break;
                case "Engine":
                    engine = new Engine(part.Key);
                    break;
                case "Wings":
                    wings = new Wings(part.Key);
                    break;
                case "Thruster":
                    for (int i = 0; i < part.Value; i++)
                    {
                        thrusters.Add(new Thruster(part.Key));
                    }
                    break;
            }
        }
        return new Spaceship(config.Type, hull, engine, wings, thrusters);
    }
}
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
}
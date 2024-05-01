using System;
using System.Collections.Generic;

namespace FactorySpaceships.Models;

public class Inventory
{
    public List<Spaceship> Spaceships { get; private set; }
    public List<Part> Parts { get; private set; }
    public List<Assembly> Assemblies { get; private set; }
    
    public Inventory()
    {
        Spaceships = new List<Spaceship>();
        Parts = new List<Part>();
        Assemblies = new List<Assembly>(); 
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
}
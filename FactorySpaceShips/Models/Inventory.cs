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
     * Display the number of spaceships categorized by type
     * This function can be merged with PrintNumberOfPartByName()
     */
    public void PrintNumberOfSpaceshipByType()
    {
        Dictionary<string, int> typeCounts = new Dictionary<string, int>();

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
        
        foreach (var typeCount in typeCounts)
        {
            Console.WriteLine($"{typeCount.Value} {typeCount.Key}");
        }
    }
    /**
     * Group all the parts into a dictionary  (key = name  | value = counter)
     * Display the number of parts categorized by name
     * This function can be merged with PrintNumberOfSpaceshipByType()
     */
    public void PrintNumberOfPartByName()
    {
        Dictionary<string, int> typeCounts = new Dictionary<string, int>();

        foreach (var part in Parts)
        {
            if (typeCounts.ContainsKey(part.Name))
            {
                typeCounts[part.Name]++;
            }
            else
            {
                typeCounts.Add(part.Name, 1);
            }
        }
        
        foreach (var typeCount in typeCounts)
        {
            Console.WriteLine($"{typeCount.Value} {typeCount.Key}");
        }
    }
    public void PrintAssemblies()
    {
        foreach(var assembly in Assemblies)
        {
            Console.WriteLine(assembly.ToString());
        }
    }

    public void SummarizeInventory()
    {
        PrintNumberOfSpaceshipByType();
        PrintNumberOfPartByName();
        PrintAssemblies();
    }
}
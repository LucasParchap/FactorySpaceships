namespace FactorySpaceships.Models;

public class Assembly
{
    private static int _nextId = 0;
    public int Id { get; private set; }
    private string? Name { get; set; }
    private Hull? Hull { get; set; }
    private Engine? Engine { get; set; }
    private Wings? Wings { get; set; }
    private List<Thruster>? Thrusters { get; set; }
    
    public Assembly(string? name = null)
    {
        Id = ++_nextId;
        Name = name;
        Thrusters = new List<Thruster>();
    }
    /*
     * Adds a part to the assembly based on it's type. Only one part of each type (Hull, Engine, Wings) can be added
     * Except for Thrusters, which can be added in multiple quantities
     */
    public void AddPart(Part part)
    {
        switch (part)
        {
            case Hull hull when Hull == null:
                Hull = hull;
                break;
            case Engine engine when Engine == null:
                Engine = engine;
                break;
            case Wings wings when Wings == null:
                Wings = wings;
                break;
            case Thruster thruster:
                Thrusters.Add(thruster);
                break;
        }
    }
    /*
     * This method returns a string representation of an Assembly
     * If a name is specified, it returns that name
     * Otherwise, it builds and returns a string listing all parts in the assembly
     */
    public override string ToString()
    {
        if (Name != null)
        {
            return Name;
        }
        var description = new System.Text.StringBuilder();
        description.Append('[');
    
        var parts = new List<string>();

        if (Hull != null)
            parts.Add(Hull.Name);
        if (Engine != null)
            parts.Add(Engine.Name);
        if (Wings != null)
            parts.Add(Wings.Name);
        if (Thrusters != null)
            parts.AddRange(Thrusters.Select(t => t.Name));
    
        description.Append(string.Join(", ", parts));

        description.Append(']');
        return description.ToString();
    }
}
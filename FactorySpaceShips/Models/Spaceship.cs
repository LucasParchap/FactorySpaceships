using System.Collections.Generic;

namespace FactorySpaceships.Models;

public class Spaceship
{
    private static int _nextId = 0;
    public int Id { get; private set; }
    public string Type { get; private set; }
    public Hull Hull { get; private set; }
    public Engine Engine { get; private set; }
    public Wings Wings { get; private set; }
    public List<Thruster> Thrusters { get; private set; }

    public Spaceship(string type,Hull hull, Engine engine, Wings wings, List<Thruster> thrusters)
    {
        Id = ++_nextId;
        Type = type;
        Hull = hull;
        Engine = engine;
        Wings = wings;
        Thrusters = thrusters;
    }
}
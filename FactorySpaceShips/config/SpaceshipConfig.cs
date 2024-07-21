using System;
using System.Collections.Generic;
using System.IO;
using FactorySpaceships.Models;
using Newtonsoft.Json;

namespace FactorySpaceships.Config
{
    public class SpaceshipConfig
    {
        private static SpaceshipConfig _instance;
        private static readonly object _lock = new object();
        private readonly string _filePath;
        
        private SpaceshipConfig()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\FactorySpaceships\Config\spaceships.json");
        }
        public static SpaceshipConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SpaceshipConfig();
                        }
                    }
                }
                return _instance;
            }
        }
		public class SpaceshipsWrapper
        {
            public List<SpaceshipData> Spaceships { get; set; } = new List<SpaceshipData>();
        }
		public class SpaceshipData
		{
    		public string Type { get; set; } = string.Empty; 
    		public Dictionary<string, int> Parts { get; set; } = new Dictionary<string, int>(); 
		}
        public class PartsWrapper
        {
            public List<string> Parts { get; set; } = new List<string>();
        }

        public List<SpaceshipData> LoadSpaceships()
        {
            try
            {
                var jsonText = File.ReadAllText(_filePath);
                var wrapper = JsonConvert.DeserializeObject<SpaceshipsWrapper>(jsonText);
                if (wrapper != null)
                {
                    return wrapper.Spaceships;
                }
                else
                {
                    throw new InvalidOperationException("Failed to load spaceship data from configuration.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading or parsing the config file: " + ex.Message);
                throw; 
            }
        }
        public List<string> LoadParts()
        {
            try
            {
                var jsonText = File.ReadAllText(_filePath);
                var wrapper = JsonConvert.DeserializeObject<PartsWrapper>(jsonText);
                if (wrapper != null)
                {
                    return wrapper.Parts;
                }
                else
                {
                    throw new InvalidOperationException("Failed to load part data from configuration.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading or parsing the config file: " + ex.Message);
                throw;
            }
        }
    }
}
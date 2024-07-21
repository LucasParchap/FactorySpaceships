using System;
using FactorySpaceships.Models;

namespace FactorySpaceships
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintRocketLaunch();
            CommandPrompt.Launch();
        }
        static void PrintRocketLaunch()
        {
            string[] rocket = {
                "         ^",
                "        / \\",
                "       /___\\",
                "      |=   =|",
                "      |     |",
                "      |     |",
                "      |     |",
                "      |     |",
                "     /|##!##|\\",
                "    / |##!##| \\",
                "   /  |##!##|  \\",
                "  |  / ^ | ^ \\  |",
                "  | /  ( | )  \\ |",
                "  |/   ( | )   \\|",
                "      ((   ))",
                "     ((  :  ))",
                "     ((  :  ))",
                "      ((   ))",
                "       (( ))",
                "        ( )",
                "         .",
                "         .",
                "         ."
            };

            foreach (string line in rocket)
            {
                Console.WriteLine(line);
                Thread.Sleep(100);
            }

            Thread.Sleep(3000);
            Console.Clear();
        }
    }
}
 
///////////////////////////////////////////////////////////////////////////////
//
// Author: Brandon Linkous, zbll17@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 3 - Warehouse Simulation
// Description: Simulation of trucks unloading crates at warehouse docks
//
///////////////////////////////////////////////////////////////////////////////
namespace WarehouseSimulation
{
    /// <summary>
    /// Handles initiation of simulation
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point for the program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            const int numDocks = 8;

            var docks = GenerateDocks(numDocks);

            List<Truck> trucks = GenerateRandomTrucks();
            
            var warehouse = new Warehouse(docks, trucks);

            warehouse.Run();
            warehouse.GenerateReport();
        }

        /// <summary>
        /// Handles random truck generation
        /// </summary>
        /// <returns>List of generated trucks</returns>
        private static List<Truck> GenerateRandomTrucks()
        {
            List<string> drivers = new List<string> { "Liam", "Noah", "Oliver", "James", "Sophia", "William", "Henry", "Lucas", "Benjamin", "Luna" };
            List<string> companies = new List<string> { "Estes", "FedEx", "Knight", "Old Dominion", "Swift" };

            Random random = new();
            HashSet<int> usedTimeIncrements = new();

            int numTrucks = random.Next(1, 41);
            List<Truck> trucks = new();

            for (int i = 0; i < numTrucks; i++)
            {
                var truck = new Truck(drivers[random.Next(drivers.Count)], companies[random.Next(companies.Count)]);
                int timeIncrement;

                do
                {
                    timeIncrement = GenerateSkewedTimeIncrement(random);
                }
                while (usedTimeIncrements.Contains(timeIncrement));
                
                usedTimeIncrements.Add(timeIncrement);
                truck.TimeIncrement = timeIncrement;
                trucks.Add(truck);
            }

            return trucks;
        }

        /// <summary>
        /// Skews truck arrival towards middle of simulation
        /// </summary>
        /// <param name="random">Random number object</param>
        /// <returns>Skewed value</returns>
        private static int GenerateSkewedTimeIncrement(Random random)
        {
            int min = 1;
            int max = 48;
            int mid = (min + max) / 2;
            int value = random.Next(min, max + 1);

            if (value <= mid)
            {
                return min + (int)Math.Sqrt((value - min) * (mid - min));
            }
            else
            {
                return max - (int)Math.Sqrt((max - value) * (max - mid));
            }
        }

        /// <summary>
        /// Handles generation of docks and assigns dock name
        /// </summary>
        /// <param name="numDocks">Number of desired docks</param>
        /// <returns>List of generated docks</returns>
        private static List<Dock> GenerateDocks(int numDocks)
        {
            var docks = new List<Dock>();

            for (int i = 0; i < numDocks; i++)
            {
                string dockName = "Dock" + (i + 1).ToString("D2");
                docks.Add(new Dock(dockName));
            }

            return docks;
        }
    }
}
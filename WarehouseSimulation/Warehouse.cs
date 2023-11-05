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
    /// Handles properties of warehouse
    /// </summary>
    public class Warehouse
    {
        public List<Dock> Docks { get; set; }
        public Queue<Truck> Entrance { get; set; }
        private int longestLine;
        private List<Truck> trucks;

        /// <summary>
        /// Warehouse constructor
        /// </summary>
        /// <param name="docks">List of available docks</param>
        /// <param name="trucks">List of trucks at entrance</param>
        public Warehouse(List<Dock> docks, List<Truck> trucks)
        {
            Docks = docks;
            Entrance = new(trucks);
            this.trucks = trucks;
        }

        /// <summary>
        /// Processes main logic of warehouse simulation, CSV file generation, and metric tracking
        /// </summary>
        public void Run()
        {
            using (StreamWriter writer = new StreamWriter("crate_unloading_log.csv"))
            {
                writer.WriteLine("Time Increment,Driver,Delivery Company,Crate ID,Crate Value,Scenario");

                int maxLineCount = 0;
                int totalOperatingCost = 0;
                var timeIncrementsInUse = new HashSet<int>();

                foreach (var dock in Docks)
                {
                    dock.TimeInUse = 0;
                }

                var trucksByTimeIncrement = new Dictionary<int, List<Truck>>();

                foreach (var truck in trucks)
                {
                    if (!trucksByTimeIncrement.ContainsKey(truck.TimeIncrement))
                    {
                        trucksByTimeIncrement[truck.TimeIncrement] = new List<Truck>();
                    }

                    trucksByTimeIncrement[truck.TimeIncrement].Add(truck);
                }

                HashSet<Truck> enqueuedTrucks = new();
                
                for (int timeStep = 1; timeStep <= 48; timeStep++)
                {
                    foreach (var truck in trucks)
                    {
                        if (truck.TimeIncrement == timeStep && !enqueuedTrucks.Contains(truck))
                        {
                            Entrance.Enqueue(truck);
                            enqueuedTrucks.Add(truck);
                        }
                    }

                    foreach (var truck in trucks)
                    {
                        if ((truck.TimeIncrement + 1) == timeStep && !truck.IsAssignedToDock)
                        {
                            Dock shortestLineDock = Docks.OrderBy(dock => dock.Line.Count).First();
                            shortestLineDock.JoinLine(truck);
                            shortestLineDock.TotalTrucks++;
                            truck.IsAssignedToDock = true;

                            timeIncrementsInUse.Add(timeStep);
                        }
                    }
                    
                    if (Entrance.Count > 0)
                    {
                        var truck = Entrance.Peek();

                        if ((truck.TimeIncrement + 1) == timeStep)
                        {
                            Entrance.Dequeue();
                        }
                    }

                    foreach (var dock in Docks)
                    {
                        maxLineCount = Math.Max(maxLineCount, dock.Line.Count);

                        if (dock.Line.Count > 0)
                        {
                            Truck truck = dock.Line.Peek();

                            if (truck.Trailer.Count > 0)
                            {
                                dock.TimeInUse++;
                                
                                Crate crate = truck.Unload();
                                dock.TotalCrates++;
                                dock.TotalSales += crate.Price;

                                string scenario = "Crate unloaded, More crates to unload";

                                if (truck.Trailer.Count == 0)
                                {
                                    if (dock.Line.Count > 1)
                                    {
                                        scenario = "Crate unloaded, All crates unloaded, Another truck is in the dock";
                                    }
                                    else
                                    {
                                        scenario = "Crate unloaded, All crates unloaded, No other truck is in the dock";
                                    }
                                }

                                writer.WriteLine($"{timeStep},{truck.Driver},{truck.DeliveryCompany},{crate.Id},${crate.Price:F2},{scenario}");
                            }
                        }

                        if (dock.Line.Count > 0 && dock.Line.Peek().Trailer.Count == 0)
                        {
                            dock.SendOff();
                        }
                    }

                    longestLine = maxLineCount;
                }

                totalOperatingCost = timeIncrementsInUse.Count * 100;
            }
        }

        /// <summary>
        /// Generates end of simulation report containing relevant metrics
        /// </summary>
        public void GenerateReport()
        {
            Console.WriteLine("Simulation Report:");
            Console.WriteLine("------------------");
            Console.WriteLine($"Number Of Docks Open: {Docks.Count}");
            Console.WriteLine($"Longest Line At Any Dock: {longestLine}");

            int totalTrucksProcessed = Docks.Sum(dock => dock.TotalTrucks);
            int totalCratesUnloaded = Docks.Sum(dock => dock.TotalCrates);
            double totalCratesValue = Docks.Sum(dock => dock.TotalSales);
            double averageCrateValue = totalCratesValue / totalCratesUnloaded;
            double averageTruckValue = totalCratesValue / totalTrucksProcessed;

            Console.WriteLine($"Total Number Of Trucks Processed: {totalTrucksProcessed}");
            Console.WriteLine($"Total Number Of Crates Unloaded: {totalCratesUnloaded}");
            Console.WriteLine($"Total Value Of Crates Unloaded: ${totalCratesValue:F2}");
            Console.WriteLine($"Average Value Per Crate: ${averageCrateValue:F2}");
            Console.WriteLine($"Average Value Per Truck: ${averageTruckValue:F2}");

            int totalDocksTimeInUse = 0;

            foreach (var dock in Docks)
            {
                if (dock.TimeInUse > 0)
                {
                    Console.WriteLine($"{dock.Id} Was In Use For {dock.TimeInUse} Time Increments");
                }

                if ((48 - dock.TimeInUse) < 48)
                {
                    Console.WriteLine($"{dock.Id} Was Not In Use For {(48 - dock.TimeInUse)} Time Increments");
                }

                if (dock.TimeInUse == 0)
                {
                    Console.WriteLine($"{dock.Id} Was Not Used");
                }
                
                totalDocksTimeInUse += dock.TimeInUse;
            }

            double averageUsePerDock = (double)totalDocksTimeInUse / Docks.Count;

            Console.WriteLine($"Average Use Per Dock: {averageUsePerDock:F2} Time Increments");

            int totalOperatingCost = totalDocksTimeInUse * 100;
            double totalRevenue = totalCratesValue - totalOperatingCost;

            Console.WriteLine($"Total Operating Cost: ${totalOperatingCost:F2}");
            Console.WriteLine($"Total Revenue: ${totalRevenue:F2}");
        }

    }
}
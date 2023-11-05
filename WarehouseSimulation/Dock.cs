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
    /// Handles properties of dock
    /// </summary>
    public class Dock
    {
        public string Id { get; set; }
        public Queue<Truck> Line { get; set; }
        public double TotalSales { get; set; }
        public int TotalCrates { get; set; }
        public int TotalTrucks { get; set; }
        public int TimeInUse { get; set; }
        public int TimeNotInUse { get; set; }

        /// <summary>
        /// Dock constructor
        /// </summary>
        /// <param name="id">Dock id</param>
        public Dock(string id)
        {
            Id = id;
            Line = new();
        }

        /// <summary>
        /// Handles a truck joining a dock's line
        /// </summary>
        /// <param name="truck">Truck to be enqueued at dock</param>
        public void JoinLine(Truck truck)
        {
            Line.Enqueue(truck);
        }

        /// <summary>
        /// Handles sending trucks away from a dock's line
        /// </summary>
        /// <returns>Truck to be removed from dock</returns>
        public Truck SendOff()
        {
            if (Line.Count > 0)
            {
                return Line.Dequeue();
            }

            return null;
        }
    }
}
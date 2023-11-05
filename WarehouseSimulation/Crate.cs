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
    /// Handles properties of crate
    /// </summary>
    public class Crate
    {
        public string Id { get; set; }
        public double Price { get; set; }

        /// <summary>
        /// Crate constructor
        /// </summary>
        /// <param name="id">Crate id</param>
        public Crate(string id)
        {
            Id = id;
            Price = GenerateRandomValue();
        }

        /// <summary>
        /// Handles random value generation for crates
        /// </summary>
        /// <returns>Random crate value between 5 and 500</returns>
        private double GenerateRandomValue()
        {
            Random random = new();
            return 5.00 + (random.NextDouble() * (500.00 - 5.00));
        }
    }
}
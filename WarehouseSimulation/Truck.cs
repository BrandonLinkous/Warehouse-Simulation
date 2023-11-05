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
    /// Handles properties of truck
    /// </summary>
    public class Truck
    {
        public string Driver { get; set; }
        public string DeliveryCompany { get; set; }
        public Stack<Crate> Trailer { get; set; }
        public int TimeIncrement { get; set; }
        
        private bool isAssignedToDock = false;

        public bool IsAssignedToDock
        {
            get { return isAssignedToDock; }
            set { isAssignedToDock = value; }
        }

        /// <summary>
        /// Truck constructor
        /// </summary>
        /// <param name="driver">Driver name</param>
        /// <param name="deliveryCompany">Delivery company name</param>
        public Truck(string driver, string deliveryCompany)
        {
            Driver = driver;
            DeliveryCompany = deliveryCompany;
            Trailer = new();
            LoadRandomCrates();
        }

        /// <summary>
        /// Handles loading of crates into truck's trailer
        /// </summary>
        /// <param name="crate">Crate to be loaded</param>
        public void Load(Crate crate)
        {
            Trailer.Push(crate);
        }

        /// <summary>
        /// Handles unloading of crates from truck's trailer
        /// </summary>
        /// <returns>Crate to be unloaded</returns>
        public Crate Unload()
        {
            if (Trailer.Count > 0)
            {
                return Trailer.Pop();
            }

            return null;
        }

        /// <summary>
        /// Handles randomization of crates per truck
        /// </summary>
        private void LoadRandomCrates()
        {
            Random random = new();
            int numberOfCrates = random.Next(1, 21);

            for (int i = 0; i < numberOfCrates; i++)
            {
                Crate crate = new($"Crate{i + 1}");
                Trailer.Push(crate);
            }
        }
    }
}
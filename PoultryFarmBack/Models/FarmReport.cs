namespace PoultryFarmBack.Models
{
    public class FarmReport
    {
        public double AverageEggsPerChicken { get; set; } 
        public int TotalEggs { get; set; } 
        public decimal TotalEggsValue { get; set; }
        public Dictionary<string, int> EggsCollectedByEmployee { get; set; } 
        public List<int> LowProductivityChickens { get; set; }
        public int MostProductiveCageId { get; set; } 
        public Dictionary<string, int> ChickensPerEmployee { get; set; } 
    }
}

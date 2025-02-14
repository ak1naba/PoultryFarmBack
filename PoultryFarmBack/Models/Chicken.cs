namespace PoultryFarmBack.Models
{
    public class Chicken
    {
        // Main Params
        public int Id { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public int EggsPerMonth { get; set; }

        // Foreing classes
        public int BreedId { get; set; }
        public int CageId { get; set; }

    }
}


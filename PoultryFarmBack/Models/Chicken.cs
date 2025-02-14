namespace PoultryFarmBack.Models
{
    public class Chicken
    {
        // Main Params
        public int Id { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public int EggsPerMonth { get; set; }

        // Foreing keys
        public int BreedId { get; set; }
        public int CageId { get; set; }

        // Foreign classes
        public Breed? Breed { get; set; }
        public Cage? Cage { get; set; }
    }
}


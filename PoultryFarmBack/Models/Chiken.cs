namespace PoultryFarmBack.Models
{
    public class Chiken
    {
        // Main Params
        public int Id { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public int EggsPerMounth { get; set; }

        // Foreing classes
        public int BreedId { get; set; }
        public int CageId { get; set; }

    }
}


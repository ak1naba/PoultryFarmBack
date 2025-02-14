namespace PoultryFarmBack.Models
{
    public class Cage
    {
        // Main params
        public int Id { set; get; }
        public bool IsOccupied { get; set; }

        // Foreing classes
        public int? ChikenId { get; set; }
    }
}

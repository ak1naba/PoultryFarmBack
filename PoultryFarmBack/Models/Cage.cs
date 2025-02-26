namespace PoultryFarmBack.Models
{
    public class Cage
    {
        // Main params
        public int Id { set; get; }
        public bool IsOccupied { get; set; }

        // Foreing classes
        public int? ChickenId { get; set; }

        public int EmployeeId {get; set; }
    }
}

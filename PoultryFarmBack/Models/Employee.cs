namespace PoultryFarmBack.Models
{
    public class Employee
    {
        // Main params
        public int Id { get; set; } 
        public string FullName { get; set; } 
        public decimal Salary { get; set; } 

        // Foreing classes
        public List<Cage>? Cages { get; set; }
    }
}

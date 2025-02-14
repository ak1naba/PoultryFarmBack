using PoultryFarmBack.Models;

namespace PoultryFarmBack.Services
{
    public class ChickenService
    {
        private readonly List<Chicken> _chickens;

        public ChickenService()
        {
            _chickens = new List<Chicken>();
        }

        public List<Chicken> GetAllChickens() => _chickens;

        public Chicken GetChickenById(int id) => _chickens.FirstOrDefault(c => c.Id == id);

        public void AddChicken(Chicken chicken)
        {
            chicken.Id = _chickens.Any() ? _chickens.Max(c => c.Id) + 1 : 1;
            _chickens.Add(chicken);
        }

        public void UpdateChicken(Chicken chicken)
        {
            var existingChicken = _chickens.FirstOrDefault(c => c.Id == chicken.Id);
            if (existingChicken != null)
            {
                existingChicken.Weight = chicken.Weight;
                existingChicken.Age = chicken.Age;
                existingChicken.EggsPerMonth = chicken.EggsPerMonth;
                existingChicken.BreedId = chicken.BreedId;
                existingChicken.CageId = chicken.CageId;
            }
        }

        public void DeleteChicken(int id)
        {
            var chicken = _chickens.FirstOrDefault(c => c.Id == id);
            if (chicken != null)
            {
                _chickens.Remove(chicken);
            }
        }
    }
}

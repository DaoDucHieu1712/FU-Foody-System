using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls {
    public class ComboRepository : EntityRepository<Combo, int>, IComboRepository {
        public ComboRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void AddComboFood(int comboId, int storeId, List<int> idFoods)
        {
            foreach (int id in idFoods)
            {
                FoodCombo foodCombo = new FoodCombo()
                {
                    ComboId = comboId,
                    StoreId = storeId,
                    FoodId = id,
                };
                _context.Add(foodCombo);
            }
            _context.SaveChanges();
        }

        public void DeleteCombo(int comboId)
        {
            Combo combo = _context.Combos.FirstOrDefault(x => x.Id == comboId);
            List<FoodCombo> foodCombos = _context.FoodCombos.Where(x => x.Id == comboId).ToList();
            foreach (FoodCombo cb in foodCombos)
            {
                cb.IsDelete = true;
            }
            combo.IsDelete = true;
            _context.SaveChanges();
        }

        public void UpdateComboFood(int comboId, int storeId, List<int> idFoods)
        {
            var existingFoodCombos = _context.FoodCombos
                .Where(fc => fc.ComboId == comboId && fc.StoreId == storeId)
                .ToList();

            _context.RemoveRange(existingFoodCombos);

            foreach (int id in idFoods)
            {
                FoodCombo foodCombo = new FoodCombo()
                {
                    ComboId = comboId,
                    StoreId = storeId,
                    FoodId = id,
                };
                _context.Add(foodCombo);
            }

            _context.SaveChanges();
        }
    }
}

using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories {
    public interface IComboRepository : IRepository<Combo, int> {
        void AddComboFood(int comboId, int storeId, List<int> idFoods);
        void UpdateComboFood(int comboId, int storeId, List<int> idFoods);
        void DeleteCombo(int comboId);


    }
}

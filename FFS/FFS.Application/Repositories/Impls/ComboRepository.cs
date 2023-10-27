using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls {
    public class ComboRepository : EntityRepository<Combo, int>, IComboRepository {
        public ComboRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

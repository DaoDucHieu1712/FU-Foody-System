using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories.Impls
{
    public class ReactPostRepository : EntityRepository<ReactPost, int>, IReactPostRepository
    {
        public ReactPostRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

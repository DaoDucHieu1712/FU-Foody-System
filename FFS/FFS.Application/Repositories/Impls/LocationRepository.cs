using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories.Impls
{
    public class LocationRepository :EntityRepository<Location, int>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext _dbContext) : base(_dbContext) { }


    }
}

using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories
{
    public interface IOrderRepository : IRepository<Order, int>
    {
    }
}

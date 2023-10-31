using FFS.Application.DTOs.Order;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories
{
    public interface IOrderRepository : IRepository<Order, int>
    {
        Task<OrderDTO> CreateOrder(OrderRequestDTO orderRequestDTO);
        Task AddOrder(List<OrderDetailDTO> orderDetailDTOs);
    }
}

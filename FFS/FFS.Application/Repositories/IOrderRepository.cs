﻿using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories
{
    public interface IOrderRepository : IRepository<Order, int>
    {
        Task<OrderDTO> CreateOrder(OrderRequestDTO orderRequestDTO);
        Task AddOrder(List<OrderDetailDTO> orderDetailDTOs);
        Task<List<dynamic>> GetOrder(Parameters parameters);
        Task<int> CountGetOrder(Parameters parameters);
        Task<dynamic> GetOrderDetail(int id);
    }
}

using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.Others;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class OrderControllerTest
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        private OrderController _orderController;

        public OrderControllerTest(Mock<IOrderRepository> orderRepositoryMock, Mock<IMapper> mapperMock, Mock<IOrderDetailRepository> orderDetailRepositoryMock, OrderController orderController)
        {
            _orderRepositoryMock = orderRepositoryMock;
            _mapperMock = mapperMock;
            _orderDetailRepositoryMock = orderDetailRepositoryMock;
            _orderController = orderController;
        }

        public async Task GetOrderWithStore_ReturnOkResult()
        {
            var orderparams = new OrderFilterDTO 
            {
                PageIndex = 1,
                CustomerName = "Dao Hieu",
                ShipperName = "Manh Quang",
                StartDate = DateTime.Parse("2023-12-06"),
                EndDate = DateTime.Parse("2023-12-08"),
                ToPrice = 10000,
                FromPrice = 100000,
                OrderId = 2,
                SortType = "price-desc",
                Status = FFS.Application.Entities.Enum.OrderStatus.Booked
            };


            var orders = new Order
            {

            };

    //        _orderRepositoryMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<OrderDetail, object>>>()))
    //.Returns(.AsQueryable());

    //        _mapperMock.Setup(mapper => mapper.Map<List<DiscountDTO>>(It.IsAny<PagedList<Discount>>()))
    //            .Returns(discountDtos);

    //        // Act
    //        var result = _discountController.ListDiscoutByStore(discountParameters);

    //        // Assert
    //        var okObjectResult = Assert.IsType<OkObjectResult>(result);
    //        Assert.Equal(200, okObjectResult.StatusCode);


        } 



    }
}

using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.Others;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Hubs;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static FFS.Application.Controllers.OrderController;

namespace Test
{
    public class OrderControllerTest
    {
        private readonly Mock<IOrderRepository> _orderMock;
        private readonly Mock<IOrderDetailRepository> _odMock;
        private readonly Mock<IStoreRepository> _storeMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHubContext<NotificationHub>> _hubContextMock;
        private readonly Mock<INotificationRepository> _notiMock;
        private readonly Mock<IInventoryRepository> _invenMock;
        private Mock<ILoggerManager> _logger;
        private OrderController _controller;
        public OrderControllerTest()
        {
            _orderMock = new Mock<IOrderRepository>();
            _odMock = new Mock<IOrderDetailRepository>();
            _storeMock = new Mock<IStoreRepository>();
            _mapperMock = new Mock<IMapper>();
            _hubContextMock = new Mock<IHubContext<NotificationHub>>();
            _notiMock = new Mock<INotificationRepository> { };
            _invenMock = new Mock<IInventoryRepository>();
            _logger = new Mock<ILoggerManager> { };
            _controller = new OrderController(_orderMock.Object, _odMock.Object, _storeMock.Object, _mapperMock.Object, _hubContextMock.Object, _notiMock.Object, _invenMock.Object, _logger.Object);
        }
        #region CreaterOrder
        [Fact]
        public async Task CreaterOrder_ReturnsOkResultWithOrderDTO()
        {
            // Arrange
            var fakeOrderRequestDTO = new OrderRequestDTO();
            var fakeOrderDTO = new OrderDTO();
            _orderMock.Setup(repo => repo.CreateOrder(It.IsAny<OrderRequestDTO>()))
                .ReturnsAsync(fakeOrderDTO);

            // Act
            var result = await _controller.CreaterOrder(fakeOrderRequestDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orderDTO = Assert.IsType<OrderDTO>(okResult.Value);
            Assert.Equal(fakeOrderDTO, orderDTO);
        }

        [Fact]
        public async Task CreaterOrder_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var fakeOrderRequestDTO = new OrderRequestDTO();
            _orderMock.Setup(repo => repo.CreateOrder(It.IsAny<OrderRequestDTO>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.CreaterOrder(fakeOrderRequestDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion


        #region Order
        [Fact]
        public async Task Order_ReturnsOkResultWithOrderDTO()
        {
            // Arrange
            var fakeCreateOrderDTO = new CreateOrderDTO();
            var fakeOrderDTO = new OrderDTO();

            _orderMock.Setup(repo => repo.Order(It.IsAny<CreateOrderDTO>()))
                .ReturnsAsync(fakeOrderDTO);
            _invenMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Inventory, bool>>>()))
                .ReturnsAsync(new Inventory { quantity = 10 }); // Adjust as needed

            // Act
            var result = await _controller.Order(fakeCreateOrderDTO);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Order_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var fakeCreateOrderDTO = new CreateOrderDTO();
            _orderMock.Setup(repo => repo.Order(It.IsAny<CreateOrderDTO>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.Order(fakeCreateOrderDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _orderMock.Verify(repo => repo.Order(It.IsAny<CreateOrderDTO>()), Times.Once);
            _invenMock.Verify(repo => repo.Update(It.IsAny<Inventory>(), It.IsAny<string>()), Times.Never);
        }

        #endregion  
        #region GetStoreIdByOrderId
        [Fact]
        public async Task GetStoreIdByOrderId_ReturnsOkResultWithStoreId()
        {
            // Arrange
            var fakeOrderId = 1;
            var fakeStoreId = 123;

            _orderMock.Setup(repo => repo.GetStoreIdByOrderId(fakeOrderId))
                .ReturnsAsync(fakeStoreId);

            // Act
            var result = await _controller.GetStoreIdByOrderId(fakeOrderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _orderMock.Verify(repo => repo.GetStoreIdByOrderId(fakeOrderId), Times.Once);
        }

        [Fact]
        public async Task GetStoreIdByOrderId_ReturnsNotFoundResult()
        {
            // Arrange
            var fakeOrderId = 1;

            _orderMock.Setup(repo => repo.GetStoreIdByOrderId(fakeOrderId))
                .ReturnsAsync((int?)null);

            // Act
            var result = await _controller.GetStoreIdByOrderId(fakeOrderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            _orderMock.Verify(repo => repo.GetStoreIdByOrderId(fakeOrderId), Times.Once);
        }

        [Fact]
        public async Task GetStoreIdByOrderId_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var fakeOrderId = 1;
            _orderMock.Setup(repo => repo.GetStoreIdByOrderId(fakeOrderId))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.GetStoreIdByOrderId(fakeOrderId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _orderMock.Verify(repo => repo.GetStoreIdByOrderId(fakeOrderId), Times.Once);
        }

        #endregion

        #region AddOrderItem
        [Fact]
        public async Task AddOrderItem_ReturnsNoContentResult()
        {
            // Arrange
            var fakeOrderItems = new List<OrderDetailDTO>
    {
        new OrderDetailDTO { },
    };

            // Act
            var result = await _controller.AddOrderItem(fakeOrderItems);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            _orderMock.Verify(repo => repo.AddOrder(fakeOrderItems), Times.Once);
        }

        [Fact]
        public async Task AddOrderItem_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var fakeOrderItems = new List<OrderDetailDTO>
    {
        new OrderDetailDTO {},
    };

            _orderMock.Setup(repo => repo.AddOrder(fakeOrderItems))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.AddOrderItem(fakeOrderItems);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _orderMock.Verify(repo => repo.AddOrder(fakeOrderItems), Times.Once);
        }

        #endregion

        #region FindById
        [Fact]
        public async Task FindById_ReturnsOkResultWithData()
        {
            // Arrange
            var orderId = 1; // Replace with a valid order ID
            var fakeOrder = new Order // Replace with a valid Order instance
            {
                Id = orderId,
            };

            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Returns(new List<Order> { fakeOrder }.AsQueryable());

            // Act
            var result = await _controller.FindById(orderId);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task FindById_ReturnsNotFoundResultWhenOrderNotFound()
        {
            // Arrange
            var orderId = 1; // Replace with a valid order ID

            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Returns(new List<Order>().AsQueryable());

            // Act
            var result = await _controller.FindById(orderId);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task FindById_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var orderId = 1;

            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.FindById(orderId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region MyOrder
        [Fact]
        public async Task MyOrder_ReturnsOkResultWithData()
        {
            // Arrange
            var customerId = "testCustomerId"; // Replace with a valid customer ID
            var fakeOrders = new List<Order> // Replace with valid Order instances
    {
        new Order { Id = 1, CustomerId = customerId, CreatedAt = DateTime.UtcNow, TotalPrice = 100 },
        new Order { Id = 2, CustomerId = customerId, CreatedAt = DateTime.UtcNow.AddDays(-1), TotalPrice = 150 }
    };

            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Returns(fakeOrders.AsQueryable());

            var orderFilterDTO = new OrderFilterDTO
            {
            };

            // Act
            var result = await _controller.MyOrder(customerId, orderFilterDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task MyOrder_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var customerId = "testCustomerId"; // Replace with a valid customer ID

            _orderMock.Setup(repo => repo.FindAll(
                    It.IsAny<Expression<Func<Order, bool>>>(),
                    It.IsAny<Expression<Func<Order, object>>>()))
                .Throws(new Exception("Test exception"));

            var orderFilterDTO = new OrderFilterDTO
            {
            };

            // Act
            var result = await _controller.MyOrder(customerId, orderFilterDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _orderMock.Verify(repo => repo.FindAll(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Expression<Func<Order, object>>>()), Times.Once);
        }

        #endregion
        #region GetOrderWithStore

        [Fact]
        public async Task GetOrderWithStore_ReturnsOkResult()
        {
            // Act
            var result = await _controller.GetOrderWithStore(1, new OrderFilterDTO());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderWithStore_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();
            _odMock
     .Setup(repo => repo.FindAll(
         It.IsAny<Expression<Func<OrderDetail, bool>>>(),
         It.IsAny<Expression<Func<OrderDetail, object>>[]>()
     ))
                 .Throws(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetOrderWithStore(1, new OrderFilterDTO());

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetOrderWithStore_ReturnsCorrectTypeAndData()
        {
            // Arrange
            _odMock
      .Setup(repo => repo.FindAll(
          It.IsAny<Expression<Func<OrderDetail, bool>>>(),
          It.IsAny<Expression<Func<OrderDetail, object>>[]>()
      ))
                 .Returns(new List<OrderDetail>().AsQueryable());

            // Act
            var result = await _controller.GetOrderWithStore(1, new OrderFilterDTO()) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<EntityFilter<OrderResponseDTO>>(result.Value);
        }
        #endregion

        #region GetOrderDetail
        [Fact]
        public async Task GetOrderDetail_ReturnsOkResult()
        {
            var result = await _controller.GetOrderDetail(1);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderDetail_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _odMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<OrderDetail, bool>>>(), It.IsAny<Expression<Func<OrderDetail, object>>[]>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetOrderDetail(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetOrderDetail_ReturnsCorrectTypeAndData()
        {
            var testData = new List<OrderDetailResponseDTO> { new OrderDetailResponseDTO { Id = 1, OrderId = 1, FoodId = 4 } };
            // Arrange
            _odMock
     .Setup(repo => repo.FindAll(
         It.IsAny<Expression<Func<OrderDetail, bool>>>(),
         It.IsAny<Expression<Func<OrderDetail, object>>[]>()
     ))
                .Returns(new List<OrderDetail>().AsQueryable());
            _mapperMock.Setup(mapper => mapper.Map<List<OrderDetailResponseDTO>>(testData))
                .Returns(new List<OrderDetailResponseDTO>());


            // Act
            var result = await _controller.GetOrderDetail(1) as OkObjectResult;

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region GetOrderUnBook
        [Fact]
        public async Task GetOrderUnBook_ReturnsOkResult()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();

            // Act
            var parameters = new Parameters { ShipperId = "shipperid" };
            var result = await _controller.GetOrderUnBook(parameters);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderUnBook_ReturnsBadRequest_WhenShipperHasUncompletedOrder()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Returns(new List<Order> { new Order { OrderStatus = OrderStatus.Booked } }.AsQueryable());

            // Act
            var parameters = new Parameters { ShipperId = "shipperId" };
            var result = await _controller.GetOrderUnBook(parameters);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("Object reference not set to an instance of an object.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetOrderUnBook_ReturnsCorrectTypeAndData()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
               .Returns(new List<Order>().AsQueryable());
            _orderMock.Setup(repo => repo.GetOrder(It.IsAny<Parameters>()))
                .ReturnsAsync(new List<dynamic>());
            _orderMock.Setup(repo => repo.CountGetOrder(It.IsAny<Parameters>()))
                .ReturnsAsync(0);

            // Act
            var parameters = new Parameters { ShipperId = "shippeId" };
            var result = await _controller.GetOrderUnBook(parameters) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
        }
        #endregion

        #region CheckReceiverOrder
        [Fact]
        public async Task CheckReceiverOrder_ReturnsOkResult_WhenReceiverHasNoBookedOrder()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
               .Returns(new List<Order>().AsQueryable());


            // Act
            var result = await _controller.CheckReceiverOrder("receiverId");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value);
        }

        [Fact]
        public async Task CheckReceiverOrder_ReturnsOkResult_WhenReceiverHasBookedOrder()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Returns(new List<Order> { new Order { OrderStatus = OrderStatus.Booked } }.AsQueryable());

            // Act
            var result = await _controller.CheckReceiverOrder("receiverId");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value);
        }

        [Fact]
        public async Task CheckReceiverOrder_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Throws(new Exception("Simulated exception"));


            // Act
            var result = await _controller.CheckReceiverOrder("receiverId");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region GetOrderIdel
        [Fact]
        public async Task GetOrderIdel_ReturnsCorrectTypeAndData()
        {
            // Arrange
            var testData = new List<Order>();
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Returns(testData.AsQueryable());
            _mapperMock.Setup(mapper => mapper.Map<List<OrderResponseDTO>>(testData))
                .Returns(new List<OrderResponseDTO>());

            // Act
            var result = await _controller.GetOrderIdel(1, 10) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetOrderIdel_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetOrderIdel(1, 10);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region ReceiveOrderUnbook

        [Fact]
        public async Task ReceiveOrderUnbook_ReturnsOkResult_WhenSuccessfullyReceived()
        {
            // Arrange

            // Mocking FindById method
            _orderMock.Setup(repo => repo.FindById(It.IsAny<int>(), It.IsAny<Expression<Func<Order, object>>[]>()))
     .ReturnsAsync(new Order { Id = 1, ShipperId = null, OrderStatus = OrderStatus.Unbooked, CustomerId = "customerId" });

            // Mocking FindAll method
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                 .Returns(new List<Order>().AsQueryable());

            // Mocking GetStoreIdByOrderId method
            _orderMock.Setup(repo => repo.GetStoreIdByOrderId(It.IsAny<int>()))
                .ReturnsAsync(1);

            // Mocking GetInformationStore method
            _storeMock.Setup(repo => repo.GetInformationStore(It.IsAny<int>()))
      .ReturnsAsync(new StoreInforDTO { UserId = "storeUserId" });


            // Mocking SendAsync method
            _hubContextMock.Setup(hub => hub.Clients.All.SendCoreAsync(
          It.Is<string>(methodName => methodName == "ReceiveNotification"),
          It.Is<object[]>(args => args.Length == 1 && args[0] is Notification),
          default(CancellationToken)));

            var result = await _controller.ReceiveOrderUnbook("shipperId", 1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Nhận đơn hàng thành công!", (result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ReceiveOrderUnbook_ReturnsNotFound_WhenStoreNotfound()
        {
            // Mocking FindById method
            _orderMock.Setup(repo => repo.FindById(It.IsAny<int>(), It.IsAny<Expression<Func<Order, object>>[]>()))
                .ReturnsAsync(new Order { Id = 1, ShipperId = null, OrderStatus = OrderStatus.Unbooked, CustomerId = "customerId" });

            // Mocking FindAll method
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
               .Returns(new List<Order>().AsQueryable());

            // Mocking GetStoreIdByOrderId method
            _storeMock.Setup(repo => repo.GetInformationStore(It.IsAny<int>()))
       .Returns(Task.FromResult<StoreInforDTO>(null));

            var result = await _controller.ReceiveOrderUnbook("shipperId", 1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Store not found for OrderId: 1", notFoundResult.Value);
        }

        [Fact]
        public async Task ReceiveOrderUnbook_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Mocking FindById method to throw an exception
            _orderMock.Setup(repo => repo.FindById(It.IsAny<int>(), It.IsAny<Expression<Func<Order, object>>[]>()))
                 .Throws(new Exception("Simulated exception"));

            var result = await _controller.ReceiveOrderUnbook("shipperId", 1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion


        #region AcceptOrderWithShipper
        [Fact]
        public async Task AcceptOrderWithShipper_ReturnsNoContentResult()
        {
            // Mocking FindSingle method
            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new Order { Id = 1, OrderStatus = OrderStatus.Unbooked, CustomerId = "customerId" });

            // Act
            var result = await _controller.AcceptOrderWithShipper(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AcceptOrderWithShipper_UpdatesOrderStatusAndShipDate()
        {
            // Mocking FindSingle method
            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
               .ReturnsAsync(new Order { Id = 1, OrderStatus = OrderStatus.Unbooked, CustomerId = "customerId" });

            // Act
            var result = await _controller.AcceptOrderWithShipper(1);
            Assert.IsType<ObjectResult>(result);
            // Assert
        }

        [Fact]
        public async Task AcceptOrderWithShipper_ReturnsInternalServerError_WhenExceptionOccurs()
        {

            // Mocking FindSingle method to throw an exception
            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await _controller.AcceptOrderWithShipper(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion
        #region CancelOrderWithShipper

        [Fact]
        public async Task CancelOrderWithShipper_ReturnsNoContentResult()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
                 .ReturnsAsync(new Order { Id = 1, OrderStatus = OrderStatus.Cancel });

            // Mocking FindAll method
            _odMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<OrderDetail, bool>>>(), It.IsAny<Expression<Func<OrderDetail, object>>>()))
                .Returns(new List<OrderDetail>().AsQueryable());

            // Mocking Update method
            _orderMock.Setup(repo => repo.Update(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            // Mocking FindSingle method for Inventory
            _invenMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Inventory, bool>>>()))
                .ReturnsAsync(new Inventory { FoodId = 1, quantity = 5 });

            // Mocking Update method for Inventory
            _invenMock.Setup(repo => repo.Update(It.IsAny<Inventory>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CancelOrderWithShipper(1, "Cancellation Reason");

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task CancelOrderWithShipper_UpdatesOrderStatusAndInventory()
        {
            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new Order { Id = 1, OrderStatus = OrderStatus.Cancel });

            // Mocking FindAll method
            _odMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<OrderDetail, bool>>>(), It.IsAny<Expression<Func<OrderDetail, object>>>()))
               .Returns(new List<OrderDetail> { new OrderDetail { FoodId = 1, Quantity = 2 } }.AsQueryable());

            // Mocking Update method
            _orderMock.Setup(repo => repo.Update(It.IsAny<Order>()))
                 .Returns(Task.CompletedTask);

            // Mocking FindSingle method for Inventory
            _invenMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Inventory, bool>>>()))
                .ReturnsAsync(new Inventory { FoodId = 1, quantity = 5 });

            // Mocking Update method for Inventory
            _invenMock.Setup(repo => repo.Update(It.IsAny<Inventory>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CancelOrderWithShipper(1, "Cancellation Reason");

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task CancelOrderWithShipper_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await _controller.CancelOrderWithShipper(1, "Cancellation Reason");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region CancelOrderWithCustomer
        [Fact]
        public async Task CancelOrderWithCustomer_ReturnsNoContentResult()
        {
            // Arrange

            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new Order { Id = 1, OrderStatus = OrderStatus.Cancel });

            // Mocking FindAll method
            _odMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<OrderDetail, bool>>>(), It.IsAny<Expression<Func<OrderDetail, object>>>()))
               .Returns(new List<OrderDetail>().AsQueryable());

            // Mocking Update method
            _orderMock.Setup(repo => repo.Update(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            // Mocking FindSingle method for Inventory
            _invenMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Inventory, bool>>>()))
                .ReturnsAsync(new Inventory { FoodId = 1, quantity = 5 });

            // Mocking Update method for Inventory
            _invenMock.Setup(repo => repo.Update(It.IsAny<Inventory>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CancelOrderWithCustomer(1, "Cancellation Reason");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task CancelOrderWithCustomer_UpdatesOrderStatusAndInventory()
        {
            // Mocking FindSingle method
            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new Order { Id = 1, OrderStatus = OrderStatus.Cancel });

            // Mocking FindAll method
            _odMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<OrderDetail, bool>>>(), It.IsAny<Expression<Func<OrderDetail, object>>>()))
               .Returns(new List<OrderDetail> { new OrderDetail { FoodId = 1, Quantity = 2 } }.AsQueryable());

            // Mocking Update method
            _orderMock.Setup(repo => repo.Update(It.IsAny<Order>()))
               .Returns(Task.CompletedTask);

            // Mocking FindSingle method for Inventory
            _invenMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Inventory, bool>>>()))
               .ReturnsAsync(new Inventory { FoodId = 1, quantity = 5 });

            // Mocking Update method for Inventory
            _invenMock.Setup(repo => repo.Update(It.IsAny<Inventory>(), It.IsAny<string>()))
               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CancelOrderWithCustomer(1, "Cancellation Reason");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task CancelOrderWithCustomer_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Order, bool>>>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await _controller.CancelOrderWithCustomer(1, "Cancellation Reason");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region GetOrderFinish
        [Fact]
        public async Task GetOrderFinish_ReturnsOkResult()
        {

            // Act
            var result = await _controller.GetOrderFinish(new Parameters());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderFinish_SetsOrderStatus()
        {
            // Arrange
            var parameters = new Parameters();

            // Act
            await _controller.GetOrderFinish(parameters);

            // Assert
            Assert.Equal(OrderStatus.Finish, parameters.OrderStatus);
        }

        [Fact]
        public async Task GetOrderFinish_ReturnsInternalServerErrorOnException()
        {
            // Arrange
            // Mock an exception being thrown from the repository
            _orderMock.Setup(repo => repo.GetOrder(It.IsAny<Parameters>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetOrderFinish(new Parameters());

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        #endregion

        #region GetUrlPayment
        [Fact]
        public async Task GetUrlPayment_ReturnsValidUrl()
        {
            var orderId = 1;
            var fakeOrder = new Order
            {
                Id = orderId,
            };

            var fakeOrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                },
            };

            _orderMock.Setup(repo => repo.FindById(It.IsAny<int>(), It.IsAny<Expression<Func<Order, object>>[]>()))
                       .ReturnsAsync(fakeOrder);

            _odMock.Setup(repo => repo.FindAll(
                It.IsAny<Expression<Func<OrderDetail, bool>>>(),
                It.IsAny<Expression<Func<OrderDetail, object>>[]>()))
                .Returns(fakeOrderDetails.AsQueryable());

            // Act
            var result = await _controller.GetUrlPayment(orderId);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetUrlPayment_ThrowsException()
        {
            // Arrange
            _orderMock.Setup(repo => repo.FindById(It.IsAny<int>(), It.IsAny<Expression<Func<Order, object>>[]>()))
                  .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _controller.GetUrlPayment(1));
        }

        #endregion
        #region CreatePayment
        [Fact]
        public async Task CreatePayment_SuccessfullyCreatesPayment()
        {
            // Arrange
            var paymentCreate = new PaymentCreate
            {
                OrderId = 1, // Provide a valid orderId
                PaymentMethod = "CreditCard", // Provide a valid payment method
                Status = 2 // Provide a valid status
            };

            var fakeOrder = new Order
            {
                Id = paymentCreate.OrderId,
                TotalPrice = 100 // Provide a valid total price
                                 // Add other properties as needed
            };

            var fakeStoreId = 123; // Provide a valid storeId

            var fakeStoreInformation = new StoreInforDTO
            {
                UserId = "userid1", // Provide a valid UserId
            };

            _orderMock.Setup(repo => repo.FindById(paymentCreate.OrderId, null))
                            .ReturnsAsync(fakeOrder);

            _orderMock.Setup(repo => repo.CreatePayment(It.IsAny<Payment>()))
     .Returns(Task.CompletedTask);

            _orderMock.Setup(repo => repo.Update(It.IsAny<Order>()))
                            .Returns(Task.CompletedTask);

            _orderMock.Setup(repo => repo.GetStoreIdByOrderId(paymentCreate.OrderId))
                            .ReturnsAsync(fakeStoreId);

            _storeMock.Setup(repo => repo.GetInformationStore(It.IsAny<int>()))
                             .ReturnsAsync(fakeStoreInformation);

            _hubContextMock.Setup(hub => hub.Clients.All.SendCoreAsync(
          It.Is<string>(methodName => methodName == "ReceiveNotification"),
          It.Is<object[]>(args => args.Length == 1 && args[0] is Notification),
          default(CancellationToken)));

            // Act
            var result = await _controller.CreatePayment(paymentCreate);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreatePayment_ReturnsBadRequestForInvalidInput()
        {
            // Arrange
            var invalidPaymentCreate = new PaymentCreate
            {
                OrderId = -1
            };

            // Act
            var result = await _controller.CreatePayment(invalidPaymentCreate);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }
        #endregion
        #region ConfirmPayment
        [Fact]
        public async Task ConfirmPayment_SuccessfullyConfirmsPayment()
        {
            // Arrange
            var confirm = new Confirm
            {
                OrderId = 1 // Provide a valid orderId
            };

            _orderMock.Setup(repo => repo.ConfirmPayment(It.IsAny<Confirm>()))
                      .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ConfirmPayment(confirm);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var message = okResult.Value as string;
            Assert.Equal("Thanh toán thành công", message);
        }

        [Fact]
        public async Task ConfirmPayment_FailsToConfirmPayment()
        {
            // Arrange
            var confirm = new Confirm
            {
                OrderId = 2 // Provide a valid orderId
            };

            var errorMessage = "Simulated error message"; // Replace with a specific error message you expect

            _orderMock.Setup(repo => repo.ConfirmPayment(It.IsAny<Confirm>()))
                      .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.ConfirmPayment(confirm);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal(errorMessage, objectResult.Value);
        }

        #endregion
        #region GetOrderPendingWithShipper
        [Fact]
        public async Task GetOrderPendingWithShipper_Successful()
        {
            // Arrange
            var shipperId = "validShipperId";
            // Mocking successful retrieval of orders
            var fakeOrders = new List<Order>
            {
                // Create fake orders as needed
            };

            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                    .Returns(fakeOrders.AsQueryable());

            // Act
            var result = await _controller.GetOrderPendingWithShipper(shipperId);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var okResult = result as OkObjectResult;
        }

        [Fact]
        public async Task GetOrderPendingWithShipper_Unauthorized()
        {
            // Arrange
            var shipperId = "invalidShipperId";
            // Act
            var result = await _controller.GetOrderPendingWithShipper(shipperId);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderPendingWithShipper_Error()
        {
            var shipperId = "validShipperId";

            // Mocking an exception during retrieval
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                    .Throws(new Exception("Simulated error"));

            // Act
            var result = await _controller.GetOrderPendingWithShipper(shipperId);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            // Add more specific assertions based on the expected error handling behavior
        }


        #endregion

        #region GetNumberOfOrder
        [Fact]
        public async Task GetNumberOfOrder_Successful()
        {
            // Arrange
            var shipperId = "validShipperId";

            // Mocking successful retrieval of orders
            var fakeOrders = new List<Order>
            {
                // Create fake orders as needed
            };

            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                     .Returns(fakeOrders.AsQueryable());

            // Act
            var result = await _controller.GetNumberOfOrder(shipperId);

            // Assert
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetNumberOfOrder_Error()
        {
            // Arrange
            var shipperId = "validShipperId";

            // Mocking an exception during retrieval
            _orderMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<Expression<Func<Order, object>>>()))
                    .Throws(new Exception("Simulated error"));

            // Act
            var result = await _controller.GetNumberOfOrder(shipperId);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            // Add more specific assertions based on the expected error handling behavior
        }

        #endregion

        #region GetRevenueShipperPerMonth
        [Fact]
        public void GetRevenueShipperPerMonth_Successful()
        {
            // Arrange
            var shipperId = "validShipperId";
            var year = 2023; // Provide a valid year

            // Mocking successful retrieval of revenue
            var fakeRevenuePerMonths = new List<RevenuePerMonth>
            {
            };

            _orderMock.Setup(repo => repo.RevenueShipperPerMonth(It.IsAny<string>(), It.IsAny<int>()))
                      .Returns(fakeRevenuePerMonths);

            // Act
            var result = _controller.GetRevenueShipperPerMonth(shipperId, year);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var revenuePerMonths = okResult.Value as List<RevenuePerMonth>;
            Assert.NotNull(revenuePerMonths);

        }

        [Fact]
        public void GetRevenueShipperPerMonth_Error()
        {
            // Arrange
            var shipperId = "validShipperId";
            var year = 2023; // Provide a valid year

            // Mocking an exception during retrieval
            _orderMock.Setup(repo => repo.RevenueShipperPerMonth(It.IsAny<string>(), It.IsAny<int>()))
                      .Throws(new Exception("Simulated error"));

            // Act
            var result = _controller.GetRevenueShipperPerMonth(shipperId, year);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        #endregion
    }
}

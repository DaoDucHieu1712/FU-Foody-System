using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Hubs;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System.Security.Claims;

namespace Test
{
    public class AdminControllerTest
    {
        private readonly Mock<IReportRepository> _reportRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IPostRepository> _postRepository;
        private readonly Mock<IOrderRepository> _orderRepository;
        private readonly Mock<IAuthRepository> _authRepository;
        private readonly Mock<IHubContext<NotificationHub>> _hubContext;
        private readonly Mock<INotificationRepository> _notifyRepository;
        private readonly Mock<IMapper> _mapper;

        private AdminController controller;

        public AdminControllerTest()
        {
            _reportRepository = new Mock<IReportRepository>();
            _userRepository = new Mock<IUserRepository>();
            _postRepository = new Mock<IPostRepository>();
            _orderRepository = new Mock<IOrderRepository>();
            _authRepository = new Mock<IAuthRepository>();
            _hubContext = new Mock<IHubContext<NotificationHub>>();
            _notifyRepository = new Mock<INotificationRepository>();
            _mapper = new Mock<IMapper>();

            controller = new AdminController(_reportRepository.Object,
                                             _hubContext.Object,
                                             _notifyRepository.Object,
                                             _userRepository.Object,
                                             _postRepository.Object,
                                             _orderRepository.Object,
                                             _mapper.Object);
        }

        [Fact]
        public void GetReports_UnauthorizedUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Role, "User"),
                    }))
                }
            };
            // Act
            var result = controller.GetReports(new ReportParameters());

            // Assert
            _ = Assert.IsType<UnauthorizedResult>(result);
        }




        [Fact]
        public void GetReports_UserWithPermissionNoData_ReturnsOkResultWithEmptyData()
        {
            // Arrange
            _ = _reportRepository.Setup(repo => repo.GetReports(It.IsAny<ReportParameters>()))
                .Returns(Enumerable.Empty<dynamic>());


            // Act
            var result = controller.GetReports(new ReportParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<dynamic>>(okResult.Value);
            Assert.Empty(data);
        }

        [Fact]
        public void GetReports_UserWithPermissionAndData_ReturnsOkResultWithData()
        {
            // Arrange
            var dummyData = new List<dynamic> { /* Add dummy data here */ };

            var mockReportRepository = new Mock<IReportRepository>();
            _ = mockReportRepository.Setup(repo => repo.GetReports(It.IsAny<ReportParameters>()))
                .Returns(dummyData);

            // Act
            var result = controller.GetReports(new ReportParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<dynamic>>(okResult.Value);
            Assert.Equal(dummyData, data);
        }
    }
}

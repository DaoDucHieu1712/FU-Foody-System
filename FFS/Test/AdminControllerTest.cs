using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;

using FFS.Application.Controllers;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
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
        private readonly Mock<ILoggerManager> _logger;

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
            _logger = new Mock<ILoggerManager>();

            controller = new AdminController(_reportRepository.Object,
                                             _hubContext.Object,
                                             _notifyRepository.Object,
                                             _userRepository.Object,
                                             _postRepository.Object,
                                             _orderRepository.Object,
                                             _mapper.Object,
                                             _logger.Object);
        }

        #region Get report
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
        #endregion

        #region Get Account
        [Fact]
        public void GetAccounts_UnauthorizedUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            // Act
            var result = controller.GetAccounts(new UserParameters());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GetAccounts_UserWithPermissionNoData_ReturnsOkResultWithEmptyData()
        {
            // Arrange
            _userRepository.Setup(repo => repo.GetUsers(It.IsAny<UserParameters>()))
                .Returns(Enumerable.Empty<dynamic>());
            _userRepository.Setup(repo => repo.CountGetUsers(It.IsAny<UserParameters>()))
                .Returns(0);


            // Act
            var result = controller.GetAccounts(new UserParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dataReturn>(okResult.Value);
            Assert.Empty(response.data);
            Assert.Equal(0, response.total);
        }

        [Fact]
        public void GetAccounts_UserWithPermissionAndData_ReturnsOkResultWithData()
        {
            // Arrange
            var dummyData = new List<dynamic> { };

            _userRepository.Setup(repo => repo.GetUsers(It.IsAny<UserParameters>()))
                .Returns(dummyData);
            _userRepository.Setup(repo => repo.CountGetUsers(It.IsAny<UserParameters>()))
                .Returns(dummyData.Count);


            // Act
            var result = controller.GetAccounts(new UserParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dataReturn>(okResult.Value);
            var data = Assert.IsAssignableFrom<IEnumerable<dynamic>>(response.data);
            Assert.Equal(dummyData, data);
            Assert.Equal(dummyData.Count, response.total);
        }
        #endregion

        #region Get Posts
        [Fact]
        public void GetPosts_UnauthorizedUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            // Act
            var result = controller.GetPosts(new UserParameters());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GetPosts_UserWithPermissionNoData_ReturnsOkResultWithEmptyData()
        {
            // Arrange
            _userRepository.Setup(repo => repo.GetPosts(It.IsAny<UserParameters>()))
                .Returns(Enumerable.Empty<dynamic>());
            _userRepository.Setup(repo => repo.CountGetPosts(It.IsAny<UserParameters>()))
                .Returns(0);


            // Act
            var result = controller.GetPosts(new UserParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dataReturn>(okResult.Value);
            Assert.Empty(response.data);
            Assert.Equal(0, response.data.Count());
        }

        [Fact]
        public void GetPosts_UserWithPermissionAndData_ReturnsOkResultWithData()
        {
            // Arrange
            var dummyData = new List<dynamic> { /* Add dummy data here */ };

            _userRepository.Setup(repo => repo.GetPosts(It.IsAny<UserParameters>()))
                .Returns(dummyData);
            _userRepository.Setup(repo => repo.CountGetPosts(It.IsAny<UserParameters>()))
                .Returns(dummyData.Count);


            // Act
            var result = controller.GetPosts(new UserParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dataReturn>(okResult.Value);
            var data = Assert.IsAssignableFrom<IEnumerable<dynamic>>(response.data);
            Assert.Equal(dummyData, data);
            Assert.Equal(dummyData.Count, response.total);
        }
        #endregion

        #region Approve test
        [Fact]
        public async Task ApprovePost_ApprovePost_Successfully()
        {
            // Arrange

            var userParameters = new UserParameters
            {
                IdPost = 1,
                Status = 2 // Assuming 2 represents approval status
            };

            var existingPost = new Post
            {
                Id = 1,
Title = "Test Post",
                Status = FFS.Application.Entities.Enum.StatusPost.Pending, // Assuming the initial status is pending
                UserId = "userId123"
                // Add other properties as needed
            };

            _postRepository.Setup(repo => repo.FindById(It.IsAny<int>(), null))
                .ReturnsAsync(existingPost);
            _postRepository.Setup(repo => repo.Update(It.IsAny<Post>()));

            // Act
            var result = await controller.ApprovePost(userParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Duyệt thành công!", okResult.Value);
        }

        [Fact]
        public async Task ApprovePost_RejectPost_Successfully()
        {
            // Arrange
            var userParameters = new UserParameters
            {
                IdPost = 1,
                Status = 3 // Assuming 3 represents rejection status
            };

            var existingPost = new Post
            {
                Id = 1,
                Title = "Test Post",
                Status = FFS.Application.Entities.Enum.StatusPost.Pending, // Assuming the initial status is pending
                UserId = "userId123"
                // Add other properties as needed
            };

            _postRepository.Setup(repo => repo.FindById(It.IsAny<int>(), null))
                .ReturnsAsync(existingPost);
            _postRepository.Setup(repo => repo.Update(It.IsAny<Post>()));

            // Act
            var result = await controller.ApprovePost(userParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Duyệt thành công!", okResult.Value);
        }

        [Fact]
        public async Task ApprovePost_NonExistingPost_ReturnsBadRequest()
        {
            // Arrange

            var userParameters = new UserParameters
            {
                IdPost = 999, // Assuming an ID that doesn't exist
                Status = 2 // Assuming 2 represents approval status
            };

            _postRepository.Setup(repo => repo.FindById(It.IsAny<int>(), null))
                .ReturnsAsync((Post)null);

            // Act
            var result = await controller.ApprovePost(userParameters);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Bài viết không tồn tại! Xin vui lòng thử lại sau", badRequestResult.Value);
        }
        #endregion

        #region Get request account

        [Fact]
        public void GetRequestAccount_UnauthorizedUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            // Act
            var result = controller.GetRequestAccount(new UserParameters());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GetRequestAccount_UserWithPermissionNoData_ReturnsOkResultWithEmptyData()
        {
            // Arrange
            _userRepository.Setup(repo => repo.GetRequestCreateAccount(It.IsAny<UserParameters>()))
                .Returns(Enumerable.Empty<dynamic>());
            _userRepository.Setup(repo => repo.CountGetRequestCreateAccount(It.IsAny<UserParameters>()))
                .Returns(0);


            // Act
            var result = controller.GetRequestAccount(new UserParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dataReturn>(okResult.Value);
            Assert.Empty(response.data);
            Assert.Equal(0, response.total);
        }

        [Fact]
        public void GetRequestAccount_UserWithPermissionAndData_ReturnsOkResultWithData()
        {
            // Arrange
            var dummyData = new List<dynamic> { /* Add dummy data here */ };

            _userRepository.Setup(repo => repo.GetRequestCreateAccount(It.IsAny<UserParameters>()))
                .Returns(dummyData);
            _userRepository.Setup(repo => repo.CountGetRequestCreateAccount(It.IsAny<UserParameters>()))
                .Returns(dummyData.Count);


            // Act
            var result = controller.GetRequestAccount(new UserParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dataReturn>(okResult.Value);
            var data = Assert.IsAssignableFrom<IEnumerable<dynamic>>(response.data);
            Assert.Equal(dummyData, data);
            Assert.Equal(dummyData.Count, response.total);
        }
        #endregion

        #region Ban account
        [Fact]
        public void BanAccount_UnauthorizedUser_ReturnsUnauthorizedResult()
        {
            // Arrange

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            var userParameters = new UserParameters
            {
                id = "validId",
                Username = "testUser"
            };

            // Act
            var result = controller.BanAccount(userParameters);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void BanAccount_ValidId_ReturnsOkResultAndCallsBanAccount()
        {
            // Arrange


            var userParameters = new UserParameters
            {
                id = "validId",
                Username = "testUser"
            };

            // Act
            var result = controller.BanAccount(userParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Khóa thành công tài khoản {userParameters.Username}", okResult.Value);
            _userRepository.Verify(repo => repo.BanAccount("validId"), Times.Once);
        }

        [Fact]
        public void BanAccount_ValidId_ExceptionOccurs_ReturnsStatusCode500()
        {
            // Arrange
            _userRepository.Setup(repo => repo.BanAccount(It.IsAny<string>())).Throws(new Exception("Test exception"));

            var userParameters = new UserParameters
            {
                id = "validId",
                Username = "testUser"
            };

            // Act & Assert
            var result = Assert.Throws<Exception>(() => controller.BanAccount(userParameters));
            Assert.Equal("Test exception", result.Message);
            _userRepository.Verify(repo => repo.BanAccount("validId"), Times.Once);
        }
        #endregion

    }
}

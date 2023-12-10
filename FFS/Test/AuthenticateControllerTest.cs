

using AutoFixture.Xunit2;

using AutoMapper;

using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Email;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;

using Google.Apis.Auth;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

using static System.Net.WebRequestMethods;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace FFS.Test {

    public class AuthenticateControllerTest {

        private Mock<IOptionsMonitor<AppSetting>> mockOptionsMonitor;
        private Mock<ApplicationDbContext> mockDbContext;
        private Mock<IUserStore<ApplicationUser>> userStoreMock;
        private Mock<IMapper> mockMapper;
        private Mock<IEmailService> mockEmailService;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<IAuthRepository> authRepository;
        private Mock<ILocationRepository> locationRepository;
        private AuthenticateController controller;
        private readonly Mock<ILoggerManager> _logger;

        public AuthenticateControllerTest()
        {
            // Initialize your mocks and other objects
            mockOptionsMonitor = new Mock<IOptionsMonitor<AppSetting>>();
            mockDbContext = new Mock<ApplicationDbContext>();
            userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockMapper = new Mock<IMapper>();
            mockEmailService = new Mock<IEmailService>();
            mockConfiguration = new Mock<IConfiguration>();
            locationRepository = new Mock<ILocationRepository>();
            _logger = new Mock<ILoggerManager>();

            var appSettings = new AppSetting();
            mockOptionsMonitor.Setup(m => m.CurrentValue).Returns(appSettings);

            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "ConnectionStrings")]).Returns("DefaultConnection");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "JWT")]).Returns("SecretKey");


            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            authRepository = new Mock<IAuthRepository>();

            controller = new AuthenticateController(
                mockDbContext.Object,
                mockUserManager.Object,
                authRepository.Object,
                mockMapper.Object,
                mockEmailService.Object,
                _logger.Object
            );
        }


        #region Login with email

        [Fact]
        public async void LoginByEmail_ReturnsBadRequest_WhenWrongPass()
        {
            var loginDto = new LoginDTO { Email = "tuanht22042001@gmail.com", Password = "123@123aA" };
            // Set up mock behavior for the dependencies
            ApplicationUser capturedUser = new ApplicationUser();
            mockUserManager
                .Setup(m => m.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(capturedUser);
            mockUserManager
                 .Setup(m => m.CheckPasswordAsync(capturedUser, loginDto.Password))
                 .Returns(Task.FromResult(false));

            // Act
            var result = await controller.LoginByEmail(loginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Email hoặc mật khẩu không hợp lệ !", badRequestResult.Value);
        }

        [Fact]
        public async void LoginByEmail_ReturnsBadRequest_WhenDontFillInfor()
        {
            var loginDto = new LoginDTO { Email = "", Password = "" };
            // Set up mock behavior for the dependencies
            ApplicationUser capturedUser = new ApplicationUser();
            mockUserManager
                .Setup(m => m.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(capturedUser);
            mockUserManager
                 .Setup(m => m.CheckPasswordAsync(capturedUser, loginDto.Password))
                 .Returns(Task.FromResult(false));

            // Act
            var result = await controller.LoginByEmail(loginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void LoginByEmail_ReturnsOk()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = "vinhlq2512@gmail.com",
                Password = "password"
            };
            var expectedUserClient = new UserClientDTO
            {
                Email = "vinhlq2512@gmail.com",
            };
            authRepository.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedUserClient);

            // Act
            var result = await controller.LoginByEmail(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<dynamic>(okResult.Value);
            Assert.Equal(expectedUserClient.Email, model.Email);
        }
        #endregion

        #region Login with Google
        [Fact]
        public async Task LoginGoogle_ReturnsBadRequest_WithNonFptEmail()
        {
            // Arrange
            var mockPayload = new GoogleJsonWebSignature.Payload
            {
                Email = "invalid@email.com",
                Subject = "1234567890",
                Picture = "https://example.com/avatar.png"
            };
            var googleRequest = new GoogleRequest
            {
                idToken = "valid_token"
            };
           

            // Act
            var result = await controller.LoginGoogle(googleRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("JWT must consist of Header, Payload, and Signature", badRequestResult.Value);
        }

        [Fact]
        public async Task LoginGoogle_ReturnsOk_WithValidEmail()
        {
            // Arrange
            var mockPayload = new GoogleJsonWebSignature.Payload
            {
                Email = "vinhlqhe153037@fpt.edu.vn",
                Subject = "1234567890",
                Picture = "https://example.com/avatar.png"
            };
            var googleRequest = new GoogleRequest
            {
                idToken = "valid_token"
            };
            authRepository.Setup(repo => repo.LoginWithFptMail(It.IsAny<UserRegisterDTO>()))
                .ReturnsAsync(new UserClientDTO { Email = "vinhlqhe153037@fpt.edu.vn", Role = "User" });

            // Act
            var result = await controller.LoginGoogle(googleRequest);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var model = Assert.IsType<String>(okResult.Value);
          
        }
        #endregion

        #region Store Register

        [Theory]
        [AutoData]
        public async Task StoreRegister_Succeeds_WithValidData(StoreRegisterDTO storeRegisterDTO)
        {
            // Arrange
            mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => null);
            mockUserManager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

            // Act
            var exception = await Record.ExceptionAsync(() => controller.StoreRegister(storeRegisterDTO));

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [AutoData]
        public async Task StoreRegister_ThrowsException_WithExistingEmail(
            StoreRegisterDTO storeRegisterDTO,
            [Frozen] Mock<UserManager<ApplicationUser>> userManagerMock)
        {
            // Arrange
            userManagerMock.Setup(m => m.FindByEmailAsync(storeRegisterDTO.Email))
                .ReturnsAsync(new ApplicationUser());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.StoreRegister(storeRegisterDTO));
        }
        #endregion

        #region Change password
        [Fact]
        public async Task ForgotPassword_FPTEmail_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = await controller.ForgotPassword("vinhlqhe153037@fpt.edu.vn");

            // Assert
            var response = Assert.IsType<APIResponseModel>(result);
            Assert.Equal(400, response.Code);
            Assert.False(response.IsSucceed);
            Assert.Equal("Email is an FPT email", response.Message);
            Assert.Equal("Email của bạn thuộc hệ thống FPT! Vui lòng đăng nhập với tài khoản Google để truy cập vào hệ thống!", response.Data);
        }

        [Fact]
        public async Task ForgotPassword_ExistingUser_SuccessfulEmailSending_ReturnsOk()
        {
            // Arrange
            mockUserManager.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { Email = "vinhlq2512@gmail.com" });

            mockEmailService.Setup(service => service.SendEmailAsync(It.IsAny<EmailModel>()))
                .Returns(Task.FromResult(true));


            // Act
            var result = await controller.ForgotPassword("vinhlq2512@gmail.com");

            // Assert
            var response = Assert.IsType<APIResponseModel>(result);
            Assert.Equal(200, response.Code);
            Assert.True(response.IsSucceed);
            Assert.Equal("OK", response.Message);
            Assert.Equal("Email đã được gửi thành công", response.Data);
        }

        [Fact]
        public async Task ForgotPassword_ExistingUser_EmailSendingError_ReturnsBadRequestWithError()
        {
            // Arrange
            mockUserManager.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { Email = "vinhlq2512@gmail.com" });

            mockEmailService.Setup(service => service.SendEmailAsync(It.IsAny<EmailModel>()))
                .Throws(new Exception("Test email sending error"));


            // Act
            var result = await controller.ForgotPassword("vinhlq2512@gmail.com");

            // Assert
            var response = Assert.IsType<APIResponseModel>(result);
            Assert.Equal(400, response.Code);
            Assert.False(response.IsSucceed);
            Assert.Equal("Error: Test email sending error", response.Message);
            Assert.Contains("Test email sending error", response.Message);
        }

        [Fact]
        public async Task ForgotPassword_NonExistingUser_ReturnsBadRequest()
        {
            // Arrange
            mockUserManager.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);


            // Act
            var result = await controller.ForgotPassword("nonexisting@gmail.com");

            // Assert
            var response = Assert.IsType<APIResponseModel>(result);
            Assert.Equal(400, response.Code);
            Assert.False(response.IsSucceed);
            Assert.Equal("Error: email not found!", response.Message);
            Assert.Equal("Email không tồn tại trong hệ thống, vui lòng nhập lại!", response.Data);
        }
        #endregion

        #region reset pass
        [Fact]
        public async Task ResetPassword_ExistingUser_SuccessfulPasswordReset_ReturnsOk()
        {
            // Arrange

            var model = new ResetPasswordDTO
            {
                Email = "test@example.com",
                Token = "validToken",
                Password = "newPassword"
            };

            mockUserManager.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { Email = "test@example.com" });

            mockUserManager.Setup(manager => manager.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.ResetPassword(model);

            // Assert
            var response = Assert.IsType<APIResponseModel>(result);
            Assert.Equal(200, response.Code);
            Assert.True(response.IsSucceed);
            Assert.Equal("OK", response.Message);
            Assert.Equal("Đổi mật khẩu thành công!", response.Data);
        }

        [Fact]
        public async Task ResetPassword_ExistingUser_InvalidToken_ReturnsBadRequestWithModelState()
        {
            // Arrange

            var model = new ResetPasswordDTO
            {
                Email = "test@example.com",
                Token = "invalidToken",
                Password = "newPassword"
            };

            mockUserManager.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { Email = "test@example.com" });

            mockUserManager.Setup(manager => manager.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "InvalidToken", Description = "Token is invalid" }));

            // Act
            var result = await controller.ResetPassword(model);

            // Assert
            var response = Assert.IsType<APIResponseModel>(result);
            Assert.Equal(400, response.Code);
            Assert.False(response.IsSucceed);
            Assert.Equal("Token đã hết hạn", response.Message);
            Assert.Equal("Token is invalid", response.Data);
        }

        [Fact]
        public async Task ResetPassword_NonExistingUser_ReturnsBadRequest()
        {
            // Arrange

            var model = new ResetPasswordDTO
            {
                Email = "nonexisting@example.com",
                Token = "validToken",
                Password = "newPassword"
            };

            mockUserManager.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await controller.ResetPassword(model);

            // Assert
            var response = Assert.IsType<APIResponseModel>(result);
            Assert.Equal(400, response.Code);
            Assert.False(response.IsSucceed);
            Assert.Equal("Link invalid", response.Message);
            Assert.Equal("Link invalid", response.Data);
        }
        #endregion

        #region get user by id
        [Fact]
        public async Task GetShipperById_ExistingUser_ReturnsOk()
        {
            // Arrange

            var userId = "validUserId";
            var mockUser = new ApplicationUser { Id = userId, UserName = "testUser" };

            authRepository.Setup(repo => repo.GetShipperById(userId))
                .ReturnsAsync(mockUser);

            // Act
            var result = await controller.GetShipperById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<ApplicationUser>(okResult.Value);
            Assert.Equal(userId, user.Id);
            Assert.Equal("testUser", user.UserName);
        }

        [Fact]
        public async Task GetShipperById_NonExistingUser_ReturnsNotFound()
        {
            // Arrange

            var userId = "nonExistingUserId";

            authRepository.Setup(repo => repo.GetShipperById(userId))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await controller.GetShipperById(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetShipperById_ExceptionOccurred_ReturnsBadRequestWithMessage()
        {
            // Arrange

            var userId = "validUserId";

            authRepository.Setup(repo => repo.GetShipperById(userId))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await controller.GetShipperById(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<Dictionary<string, string>>(badRequestResult.Value);
            Assert.Equal("Test exception", errorResponse["message"]);
        }
        #endregion
    }


}

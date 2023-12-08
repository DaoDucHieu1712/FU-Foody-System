

using AutoFixture.Xunit2;

using AutoMapper;

using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.DTOs.Common;
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
                mockEmailService.Object
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
            Assert.Equal("Mật khẩu không đúng !", badRequestResult.Value);
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
            var model = Assert.IsType<UserClientDTO>(okResult.Value);
            Assert.Equal(expectedUserClient, model);
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
            Assert.Equal("Email của bạn không thuộc hệ thống FPT! Vui lòng thử lại!", badRequestResult.Value);
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
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<UserClientDTO>(okResult.Value);
            Assert.Equal("User", model.Role);
            Assert.Equal("vinhlqhe153037@fpt.edu.vn", model.Email);
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
    }
    #endregion
}

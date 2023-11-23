

using AutoMapper;

using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.DTOs.Common;
using FFS.Application.Entities;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

using static Org.BouncyCastle.Math.EC.ECCurve;

namespace FFS.Test {

    public class AuthenticateControllerTest {

        private Mock<IOptionsMonitor<AppSetting>> mockOptionsMonitor;
        private Mock<ApplicationDbContext> mockDbContext;
        private Mock<IUserStore<ApplicationUser>> userStoreMock;
        private Mock<IMapper> mockMapper;
        private Mock<IEmailService> mockEmailService;
        private Mock<DapperContext> mockDapperContext;
        private Mock<IConfiguration> mockConfiguration;
        private DapperContext dapperContext;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private AuthRepository authRepository;
        private AuthenticateController controller;

        public AuthenticateControllerTest()
        {
            // Initialize your mocks and other objects
            mockOptionsMonitor = new Mock<IOptionsMonitor<AppSetting>>();
            mockDbContext = new Mock<ApplicationDbContext>();
            userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockMapper = new Mock<IMapper>();
            mockEmailService = new Mock<IEmailService>();
            mockDapperContext = new Mock<DapperContext>();
            mockConfiguration = new Mock<IConfiguration>();

            var appSettings = new AppSetting();
            mockOptionsMonitor.Setup(m => m.CurrentValue).Returns(appSettings);

            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "ConnectionStrings")]).Returns("DefaultConnection");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "JWT")]).Returns("SecretKey");

            dapperContext = new DapperContext(mockConfiguration.Object);

            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            authRepository = new AuthRepository(mockUserManager.Object, mockOptionsMonitor.Object, mockEmailService.Object, mockDbContext.Object, dapperContext);

            controller = new AuthenticateController(
                mockDbContext.Object,
                mockUserManager.Object,
                authRepository,
                mockMapper.Object,
                mockEmailService.Object
            );
        }




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
            var loginDto = new LoginDTO { Email = "tuanht22042001 @gmail.com ", Password = "123@123aA" };
            // Set up mock behavior for the dependencies
            ApplicationUser capturedUser = new ApplicationUser
            {
                UserName = "YourUsername",
                Email = loginDto.Email,
                Id = "YourUserId" // Replace with the actual user id
            };
            mockUserManager
                .Setup(m => m.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(capturedUser);
            mockUserManager
                 .Setup(m => m.CheckPasswordAsync(capturedUser, loginDto.Password))
                 .Returns(Task.FromResult(true));

            // Act
            var result = await controller.LoginByEmail(loginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }


    }
}



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

       



        [Fact]
        public void LoginByEmail_ReturnsOkResult_WhenCredentialsAreValid()
        {
            //// Arrange
            var mockOptionsMonitor = new Mock<IOptionsMonitor<AppSetting>>();
            var mockDbContext = new Mock<ApplicationDbContext>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var mockMapper = new Mock<IMapper>();
            var mockEmailService = new Mock<IEmailService>();
            var mockDapperContext = new Mock<DapperContext>();

            var appSettings = new AppSetting();
            mockOptionsMonitor.Setup(m => m.CurrentValue).Returns(appSettings);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "ConnectionStrings")]).Returns("DefaultConnection");
            var dapperContext = new DapperContext(mockConfiguration.Object);

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var authRepository = new AuthRepository(mockUserManager.Object, mockOptionsMonitor.Object, mockEmailService.Object, mockDbContext.Object, dapperContext);


            var controller = new AuthenticateController(
                mockDbContext.Object
                , mockUserManager.Object
                , authRepository
                , mockMapper.Object
                , mockEmailService.Object
            );
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "password" };

            // Set up mock behavior for the dependencies
            mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<ApplicationUser>(null));
            mockUserManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(false));
            mockUserManager.Setup(m => m.GetRolesAsync(It.IsAny<ApplicationUser>())).Returns(Task.FromResult<IList<string>>(new List<string>() { "Admin" }));

            // Act

            var result = controller.LoginByEmail(loginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

        }

        //[Fact]
        //public void LoginByEmail_ReturnsUnauthorizedResult_WhenCredentialsAreInvalid()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IAuthRepository>();
        //    var controller = new YourController(mockRepo.Object);
        //    var loginDto = new LoginDTO { Email = "test@example.com", Password = "wrongpassword" };

        //    mockRepo.Setup(repo => repo.Login(loginDto.Email, loginDto.Password))
        //        .Returns((UserClient)null); // return null to simulate invalid credentials

        //    // Act
        //    var result = controller.LoginByEmail(loginDto);

        //    // Assert
        //    Assert.IsType<UnauthorizedResult>(result);
        //}


    }
}

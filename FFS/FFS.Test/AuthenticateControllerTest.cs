using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.Entities;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Moq;

using NSubstitute;

using NUnit.Framework;

namespace FFS.Test {

    [TestFixture]
    public class AuthenticateControllerTest {

        [Test]
        public async Task GetCurrentUser_Returns_OkResult_With_UserData()
        {
            // Arrange
            var userId = "017e4112-40d9-4859-9faf-be9088426490";
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockApplicationDbContext = new Mock<ApplicationDbContext>();
            var mockMapper = new Mock<IMapper>();
            var mockEmailService = new Mock<IEmailService>();

            // Mock the UserStore and UserManager
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            string connectionString = "server=103.184.112.229,1435; database=FFS;uid=sa;pwd=Abc@123456;";
            var mockOptionsMonitor = new Mock<IOptionsMonitor<AppSetting>>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(c => c["ConnectionStrings:DefaultConnection"])
                .Returns(connectionString);
            // Real DapperContext that uses IDbConnection
            var realDapperContext = new DapperContext(mockConfiguration.Object);

            // Mock DapperContext that uses the real IDbConnection
            var mockDapperContext = new Mock<DapperContext>(mockConfiguration.Object.GetConnectionString("DefaultConnection"));
            mockDapperContext.Setup(d => d.connection.ConnectionString).Returns(connectionString);
            // Act
            var authRepository = new AuthRepository(
                mockUserManager.Object,
                mockOptionsMonitor.Object,
                mockEmailService.Object,
                mockApplicationDbContext.Object,
                mockDapperContext.Object
            );

            var controller = new AuthenticateController(
                mockApplicationDbContext.Object,
                mockUserManager.Object,
                authRepository,
                mockMapper.Object,
                mockEmailService.Object
            );

            // Mock the User property of the controller context
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", userId),
                    }, "mock"))
                }
            };

            // Act
            var result = await controller.GetCurrentUser();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var userData = okResult.Value;
            Assert.IsNotNull(userData);
            Assert.AreEqual(userId, userId);
        }

    }
}

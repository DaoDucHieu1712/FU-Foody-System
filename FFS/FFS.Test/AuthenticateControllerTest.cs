using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
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
        [TestCase("test@example.com", "password123", 200, typeof(object), null, TestName = "ValidModel_ReturnsOk")]
        [TestCase("", "password123", 400, typeof(string), "Lỗi đăng nhập !", TestName = "InvalidEmail_ReturnsBadRequest")]
        [TestCase("invalid@example.com", "invalidpassword", 401, typeof(string), "Email hoặc mật khẩu không hợp lệ !", TestName = "InvalidCredentials_ReturnsUnauthorized")]
        public async Task LoginByEmail_Returns_OkResult_With_UserData(string email, string password, int expectedStatusCode, Type expectedValueType, string expectedErrorMessage)
        {
            // Arrange
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
            mockConfiguration.SetReturnsDefault(connectionString);

            // Mock DapperContext that uses the real IDbConnection
            var mockDapperContext = new Mock<DapperContext>(mockConfiguration.Object);
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
            controller.ModelState.AddModelError("Email", "Invalid Email");
            if (expectedErrorMessage != null)
            {
                controller.ModelState.AddModelError("Email", "Invalid Email");
            }

            if (expectedValueType == typeof(object))
            {
                mockAuthRepository.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                                  .Returns(new object());
            }
            else
            {
                mockAuthRepository.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                                  .Returns((object)null);
            }

            var loginDto = new LoginDTO { Email = email, Password = password };

            // Act
            var result = controller.LoginByEmail(loginDto) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedStatusCode, (result as ObjectResult)?.StatusCode);

            if (expectedErrorMessage == null)
            {
                Assert.IsNotNull(result.Value);
                Assert.IsInstanceOf(expectedValueType, result.Value);
            }
            else
            {
                Assert.IsNotNull(result.Value);
                Assert.IsInstanceOf<string>(result.Value);
                Assert.AreEqual(expectedErrorMessage, result.Value);
            }
        }

    }
}

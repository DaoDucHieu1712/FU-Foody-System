//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using FFS.Application.Data;
//using FFS.Application.DTOs.Common;
//using FFS.Application.Entities;
//using FFS.Application.Repositories.Impls;
//using FFS.Application.Repositories;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Options;
//using Moq;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Configuration;
//using AutoFixture;
//using DocumentFormat.OpenXml.Wordprocessing;

//namespace Test {
//    public class AuthRepositoryTest {

//        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
//        private readonly Mock<IOptionsMonitor<AppSetting>> optionsMonitorMock;
//        private readonly Mock<IEmailService> emailServiceMock;
//        private readonly Mock<ApplicationDbContext> contextMock;
//        private readonly Mock<IConfiguration> configurationMock;
//        private readonly DapperContext dapperContext;  // Use the actual class instance
//        private readonly AuthRepository authRepositoryMock;
//        private readonly Mock<IAuthRepository> iAuthRepositoryMock;


//        public AuthRepositoryTest()
//        {
//            userManagerMock = new Mock<UserManager<ApplicationUser>>(
//           new Mock<IUserStore<ApplicationUser>>().Object,
//           new Mock<IOptions<IdentityOptions>>().Object,
//           new Mock<IPasswordHasher<ApplicationUser>>().Object,
//           new IUserValidator<ApplicationUser>[0],
//           new IPasswordValidator<ApplicationUser>[0],
//           new Mock<ILookupNormalizer>().Object,
//           new Mock<IdentityErrorDescriber>().Object,
//           new Mock<IServiceProvider>().Object,
//           new Mock<ILogger<UserManager<ApplicationUser>>>().Object
//       );

//            optionsMonitorMock = new Mock<IOptionsMonitor<AppSetting>>();
//            emailServiceMock = new Mock<IEmailService>();
//            contextMock = new Mock<ApplicationDbContext>();
//            configurationMock = new Mock<IConfiguration>();
//            iAuthRepositoryMock = new Mock<IAuthRepository>();
//            // Configure your IConfiguration mock to provide a connection string
//            configurationMock.Setup(x => x["ConnectionStrings:DefaultConnection"])
//                             .Returns("server=103.184.112.229,1435; database=FFS;uid=sa;pwd=Abc@123456;");
//            optionsMonitorMock.Setup(x => x.CurrentValue)
//            .Returns(new AppSetting { SecretKey = "your_test_secret_key" });
//            // Instantiate the actual DapperContext class
//            dapperContext = new DapperContext(configurationMock.Object);

//            authRepositoryMock = new AuthRepository(
//                userManagerMock.Object,
//                optionsMonitorMock.Object,
//                emailServiceMock.Object,
//                contextMock.Object,
//                dapperContext
//            );
//        }

//        private ApplicationUser CreateTestUser(string email, string password)
//        {
//            var fixture = new Fixture();
//            var user = fixture.Build<ApplicationUser>()
//                .With(u => u.Email, email)
//                .With(u => u.PasswordHash, password)
//                .With(u => u.UserName, "vinhlqhe153037")
//                .Create();

//            return user;
//        }


//        [Fact]
//        public async Task Login_ValidCredentials_ReturnsUserClientDTO()
//        {
//            // Arrange
//            var email = "vinhlqhe153037@fpt.edu.vn";
//            var password = "testPassword";
//            var user = new ApplicationUser { Id = "testUserId", UserName = "vinhlqhe153037", Email = email, Allow = true };

//            userManagerMock.Setup(x => x.FindByEmailAsync(email))
//                .ReturnsAsync(user);

//            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), password))
//                .ReturnsAsync(true);

//            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
//                .ReturnsAsync(new[] { "Admin" });


//            iAuthRepositoryMock.Setup(x => x.GenerateToken(It.IsAny<ApplicationUser>()))
//                .ReturnsAsync("testToken");
//            userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
//               .ReturnsAsync(user);


//            // Mock any other dependencies if needed

//            // Act
//            var result = await authRepositoryMock.Login(email, password);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("testUserId", result.UserId);
//            Assert.Equal(email, result.Email);
//            Assert.Equal("Admin", result.Role);
//            Assert.NotNull(result.Token);
//        }

//        [Fact]
//        public async Task Login_WithWrongPass_ReturnsErrorString()
//        {
//            // Arrange
//            var email = "vinhlqhe153037@fpt.edu.vn";
//            var invalidPassword = "testPassword";
//            var user = new ApplicationUser { Id = "testUserId", UserName = "vinhlqhe153037", Email = email, Allow = true };

//            userManagerMock.Setup(x => x.FindByEmailAsync(email))
//                .ReturnsAsync(user);

//            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), invalidPassword))
//                .ReturnsAsync(false);

//            // Act
//            var exception = await Assert.ThrowsAsync<Exception>(async () => await authRepositoryMock.Login(email, invalidPassword));

//            // Assert
//            Assert.Equal("Mật khẩu không đúng !", exception.Message);
//            //Mật khẩu không đúng !
//        }

//        [Fact]
//        public async Task Login_WithWrongEmail_ReturnsErrorString()
//        {
//            // Arrange
//            var invalidEmail = "vinhl153037@fpt.edu.vn";
//            var password = "testPassword";


//            userManagerMock.Setup(x => x.FindByEmailAsync(invalidEmail))
//                .ReturnsAsync(It.IsAny<ApplicationUser>());

//            // Act
//            var exception = await Assert.ThrowsAsync<Exception>(async () => await authRepositoryMock.Login(invalidEmail, password));

//            // Assert
//            Assert.Equal("Email không tồn tại !", exception.Message);
//            //Mật khẩu không đúng !
//        }

//        [Theory]
//        [InlineData("vinhl153037@fpt.edu.vn", "testPassword")]
//        [InlineData("vinhl153027@fpt.edu.vn", "testPassword")]
//        [InlineData("vinhl1537@fpt.edu.vn", "testPassword", true)]
//        [InlineData("vinhl1037@fpt.edu.vn", "testPassword", true)]
//        [InlineData("253037@fpt.edu.vn", "testPassword", true)]
//        [InlineData("v3153037@fpt.edu.vn", "testPassword")]
//        public async Task Login_WithDontAllow_ReturnsErrorString(string email, string password, bool? allow = false)
//        {
//            // Arrange
//            var fixture = new Fixture();
//            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
//            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//            ApplicationUser user = fixture.Build<ApplicationUser>()
//                                            .With(x => x.Email, email)
//                                            .With(x => x.PasswordHash, password)
//                                            .With(x => x.Allow, allow)
//                                            .WithAutoProperties()
//                                            .Create();


//            userManagerMock.Setup(x => x.FindByEmailAsync(email))
//                .ReturnsAsync(user);

//            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), password))
//               .ReturnsAsync(true);


//            // Act
//            var exception = await Assert.ThrowsAsync<Exception>(async () => await authRepositoryMock.Login(email, password));

//            // Assert
//            Assert.Equal("Tài khoản của bạn tạm thời bị khóa! Xin vui lòng liên hệ admin để biết thêm chi tiết!", exception.Message);
//            //Mật khẩu không đúng !
//        }
//    }
//}

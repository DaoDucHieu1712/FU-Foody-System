

using AutoMapper;

using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.Entities;
using FFS.Application.Repositories;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FFS.Test {    

    public class AuthenticateControllerTest {


        [Fact]
        public void LoginByEmail_ReturnsOkResult_WhenCredentialsAreValid()
        {

            string value = null;
            Assert.Null(value);
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

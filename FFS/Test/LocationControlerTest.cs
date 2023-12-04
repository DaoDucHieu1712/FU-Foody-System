//using AutoMapper;
//using FFS.Application.Controllers;
//using FFS.Application.Data;
//using FFS.Application.DTOs.Common;
//using FFS.Application.Entities;
//using FFS.Application.Repositories.Impls;
//using FFS.Application.Repositories;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using FFS.Application.Infrastructure.Interfaces;
//using DocumentFormat.OpenXml.Spreadsheet;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq.Expressions;
//using Location = FFS.Application.Entities.Location;
//using FFS.Application.DTOs.Location;

//namespace Test
//{
//    public class LocationControlerTest
//    {
//        private readonly Mock<ILocationRepository> locationRepoMock;
//        private readonly Mock<IMapper> mapperMock;
//        private readonly LocationController locationController;

//        public LocationControlerTest()
//        {
//            locationRepoMock = new Mock<ILocationRepository>();
//            mapperMock = new Mock<IMapper>();
//            locationController = new LocationController(locationRepoMock.Object, mapperMock.Object);
//        }

//        [Fact]
//        public async Task ListLocation_ReturnsOkResult_WithValidData()
//        {
//            // Arrange
//            var email = "tungnthe151063@gmail.com";
//            var locations = new List<Location>
//    {
//        new Location { Id = 1, User = new ApplicationUser { Email = email }, IsDelete = false },
//        new Location { Id = 2, User = new ApplicationUser { Email = email }, IsDelete = false },
//    };
//            locationRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Location, bool>>>(), It.IsAny<Expression<Func<Location, object>>>()))
//                .ReturnsAsync(locations);

//            // Act
//            var result = await locationController.ListLocation(email);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result.Result);
//            var model = Assert.IsType<List<Location>>(okResult.Value);
//            Assert.Equal(locations.Count, model.Count);
//        }

//        [Fact]
//        public async Task ListLocation_ReturnsInternalServerError_OnException()
//        {
//            // Arrange
//            var email = "tungnthe151063@gmail.com";
//            locationRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Location, bool>>>(), It.IsAny<Expression<Func<Location, object>>>()))
//                .ThrowsAsync(new Exception("Test exception"));

//            // Act
//            var result = await locationController.ListLocation(email);

//            // Assert
//            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
//            Assert.Equal(500, statusCodeResult.StatusCode);
//        }

//        [Fact]
//        public async Task AddLocation_ReturnsOkResult_OnSuccessfulAddition()
//        {
//            // Arrange
//            var locationDTO = new LocationDTO
//            {
//                Address = "Test",
//                Description = "Test",
//                Email = "Test@gmail.com",
//                PhoneNumber = "0123456789",
//                Receiver = "Test",
//                IsDefault = false
//            };
//            var locationEntity = new Location
//            {
//                UserId = "1",
//                Address = "Test",
//                Description = "Test",
//                PhoneNumber = "0123456789",
//                Receiver = "Test",
//                IsDefault = false
//            };
//            mapperMock.Setup(mapper => mapper.Map<Location>(locationDTO)).Returns(locationEntity);

//            // Act
//            var result = await locationController.AddLocation(locationDTO);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<Location>>(result);
//            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
//            Assert.Equal("Thêm thành công", okResult.Value);
//            locationRepoMock.Verify(repo => repo.Add(locationEntity), Times.Once);
//        }

//        [Fact]
//        public async Task AddLocation_ReturnsInternalServerError_OnException()
//        {
//            // Arrange
//            var locationDTO = new LocationDTO
//            {
//                Address = "Test",
//                Description = "Test",
//                Email = "Test@gmail.com",
//                PhoneNumber = "0123456789",
//                Receiver = "Test",
//                IsDefault = false
//            };
//            mapperMock.Setup(mapper => mapper.Map<Location>(locationDTO)).Throws(new Exception("Test exception"));

//            // Act
//            var result = await locationController.AddLocation(locationDTO);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<Location>>(result);
//            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
//            Assert.Equal(500, statusCodeResult.StatusCode);
//        }

//    }
//}

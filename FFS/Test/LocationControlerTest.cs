using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.Entities;
using FFS.Application.Repositories.Impls;
using FFS.Application.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FFS.Application.Infrastructure.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Location = FFS.Application.Entities.Location;
using FFS.Application.DTOs.Location;

namespace Test
{
    public class LocationControlerTest
    {
        private readonly Mock<ILocationRepository> locationRepoMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ILoggerManager> logger;
        private readonly Mock<IStoreRepository> storeRepository;
        private readonly LocationController locationController;

        public LocationControlerTest()
        {
            locationRepoMock = new Mock<ILocationRepository>();
            mapperMock = new Mock<IMapper>();
            logger = new Mock<ILoggerManager>();
            storeRepository = new Mock<IStoreRepository>();
            locationController = new LocationController(locationRepoMock.Object, mapperMock.Object, storeRepository.Object, logger.Object);
        }
        #region ListLocation
        [Fact]
        public async Task ListLocation_ReturnsOkResult_WithValidData()
        {
            // Arrange
            var email = "tungnthe151063@gmail.com";
            var locations = new List<Location>
    {
        new Location { Id = 1, User = new ApplicationUser { Email = email }, IsDelete = false },
        new Location { Id = 2, User = new ApplicationUser { Email = email }, IsDelete = false },
    };
            locationRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Location, bool>>>(), It.IsAny<Expression<Func<Location, object>>>()))
                .ReturnsAsync(locations);

            // Act
            var result = await locationController.ListLocation(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<List<Location>>(okResult.Value);
            Assert.Equal(locations.Count, model.Count);
        }

        [Fact]
        public async Task ListLocation_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var email = "tungnthe151063@gmail.com";
            locationRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Location, bool>>>(), It.IsAny<Expression<Func<Location, object>>>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await locationController.ListLocation(email);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion
        #region AddLocation
        [Fact]
        public async Task AddLocation_ReturnsOkResult_OnSuccessfulAddition()
        {
            // Arrange
            var locationDTO = new LocationDTO
            {
                Address = "Test",
                Description = "Test",
                PhoneNumber = "0123456789",
                Receiver = "Test",
                IsDefault = false
            };
            var locationEntity = new Location
            {
                UserId = "1",
                Address = "Test",
                Description = "Test",
                PhoneNumber = "0123456789",
                Receiver = "Test",
                IsDefault = false
            };
            mapperMock.Setup(mapper => mapper.Map<Location>(locationDTO)).Returns(locationEntity);

            // Act
            var result = await locationController.AddLocation(locationEntity);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Location>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal("Thêm thành công", okResult.Value);
            locationRepoMock.Verify(repo => repo.Add(locationEntity), Times.Once);
        }

        [Fact]
        public async Task AddLocation_ExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            var locationEntity = new Location
            {
                UserId = "1",
                Address = "Test",
                Description = "Test",
                PhoneNumber = "0123456789",
                Receiver = "Test",
                IsDefault = false
            };
            locationRepoMock.Setup(repo => repo.Add(locationEntity))
                .Throws(new Exception("Simulated exception during addition"));

            // Act
            var result = await locationController.AddLocation(locationEntity);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion
        #region UpdateLocation
        [Fact]
        public async Task UpdateLocation_SuccessfulUpdate_ReturnsOk()
        {
            // Arrange
            int locationId = 1;
            var locationDTO = new LocationDTO();
            var existingLocation = new Location();

            locationRepoMock.Setup(repo => repo.FindById(locationId, null))
                .ReturnsAsync(existingLocation);

            mapperMock.Setup(mapper => mapper.Map(locationDTO, existingLocation))
                .Verifiable();

            locationRepoMock.Setup(repo => repo.Update(existingLocation))
                .Verifiable();

            // Act
            var result = await locationController.UpdateLocation(locationId, locationDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateLocation_LocationNotFound_ReturnsNotFound()
        {
            // Arrange
            int locationId = 2;
            var locationDTO = new LocationDTO();

            locationRepoMock.Setup(repo => repo.FindById(locationId, null))
                .ReturnsAsync((Location)null);

            // Act
            var result = await locationController.UpdateLocation(locationId, locationDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateLocation_ExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            int locationId = 3;
            var locationDTO = new LocationDTO();
            var existingLocation = new Location();

            locationRepoMock.Setup(repo => repo.FindById(locationId, null))
                .ReturnsAsync(existingLocation);

            mapperMock.Setup(mapper => mapper.Map(locationDTO, existingLocation))
                .Throws(new Exception("Simulated exception during update"));

            // Act
            var result = await locationController.UpdateLocation(locationId, locationDTO);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, (result as ObjectResult)?.StatusCode);
        }
        #endregion
        #region DeleteLocation
        [Fact]
        public async Task DeleteLocation_SuccessfulDeletion_ReturnsOk()
        {
            // Arrange
            int locationId = 1;
            var existingLocation = new Location();

            locationRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>()))
                .ReturnsAsync(existingLocation);

            locationRepoMock.Setup(repo => repo.Remove(existingLocation))
                .Verifiable();

            // Act
            var result = await locationController.DeleteLocation(locationId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteLocation_LocationNotFound_ReturnsNotFound()
        {
            // Arrange
            int locationId = 2;

            locationRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>()))
                .ReturnsAsync((Location)null);

            // Act
            var result = await locationController.DeleteLocation(locationId);

            // Assert
            locationRepoMock.Verify(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>()), Times.Once);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteLocation_ExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            int locationId = 3;
            var existingLocation = new Location();

            locationRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>()))
                .ReturnsAsync(existingLocation);

            locationRepoMock.Setup(repo => repo.Remove(existingLocation))
                .Throws(new Exception("Simulated exception during deletion"));

            // Act
            var result = await locationController.DeleteLocation(locationId);

            // Assert
            locationRepoMock.Verify(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>()), Times.Once);
            locationRepoMock.Verify(repo => repo.Remove(existingLocation), Times.Once);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, (result as ObjectResult)?.StatusCode);
        }
        #endregion

        #region GetLocation
        [Fact]
        public async Task GetLocation_SuccessfulRetrieval_ReturnsOk()
        {
            // Arrange
            int storeId = 1;
            var store = new Store { Id = storeId, UserId = "017e4112-40d9-4859-9faf-be9088426490" };
            var location = new Location();

            storeRepository.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Store, bool>>>()))
                .ReturnsAsync(store);

            locationRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>(), null))
                .ReturnsAsync(location);

            // Act
            var result = await locationController.GetLocation(storeId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLocation_StoreNotFound_ReturnsNotFound()
        {
            // Arrange
            int storeId = 2;

            storeRepository.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Store, bool>>>()))
                .ReturnsAsync((Store)null);

            // Act
            var result = await locationController.GetLocation(storeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetLocation_LocationNotFound_ReturnsNotFound()
        {
            // Arrange
            int storeId = 3;
            var store = new Store { Id = storeId, UserId = "017e4112-40d9-4859-9faf-be9088426490" };

            storeRepository.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Store, bool>>>()))
                .ReturnsAsync(store);

            locationRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>(), null))
                .ReturnsAsync((Location)null);

            // Act
            var result = await locationController.GetLocation(storeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetLocation_ExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            int storeId = 4;

            storeRepository.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Store, bool>>>()))
                .Throws(new Exception("Simulated exception during retrieval"));

            // Act
            var result = await locationController.GetLocation(storeId);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, (result as ObjectResult)?.StatusCode);
        }
        #endregion

        #region UpdateDefaultLocation
        [Fact]
        public async Task UpdateDefaultLocation_ValidData_ReturnsOk()
        {
            // Arrange
            int locationId = 1;
            string userEmail = "test@example.com";
            var mockLocation = new Location { Id = locationId, User = new ApplicationUser { Email = userEmail } };
            locationRepoMock.Setup(repo => repo.FindById(locationId, null))
                .ReturnsAsync(mockLocation);
            locationRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>()))
                .ReturnsAsync((Location)null);

            // Act
            var result = await locationController.UpdateDefaultLocation(locationId, userEmail);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            locationRepoMock.Verify(repo => repo.Update(mockLocation), Times.Once);
        }

        [Fact]
        public async Task UpdateDefaultLocation_LocationNotFound_ReturnsNotFound()
        {
            // Arrange
            int locationId = 1;
            string userEmail = "test@example.com";
            locationRepoMock.Setup(repo => repo.FindById(locationId, null))
                .ReturnsAsync((Location)null);

            // Act
            var result = await locationController.UpdateDefaultLocation(locationId, userEmail);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            locationRepoMock.Verify(repo => repo.Update(It.IsAny<Location>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDefaultLocation_Exception_ReturnsInternalServerError()
        {
            // Arrange
            int locationId = 1;
            string userEmail = "test@example.com";
            var mockLocation = new Location { Id = locationId, User = new ApplicationUser { Email = userEmail } };
            locationRepoMock.Setup(repo => repo.FindById(locationId, null))
                .ReturnsAsync(mockLocation);
            locationRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>()))
                .ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await locationController.UpdateDefaultLocation(locationId, userEmail);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Some error message", statusCodeResult.Value);
        }
        #endregion
    }
}

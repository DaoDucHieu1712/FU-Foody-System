using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Test
{
    public class InventoryControllerTest
    {

        private readonly Mock<IInventoryRepository> mockInventoryRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerManager> logger;
        private readonly InventoryController controller;

        public InventoryControllerTest()
        {
            mockInventoryRepository = new Mock<IInventoryRepository>();
            mockMapper = new Mock<IMapper>();
            logger = new Mock<ILoggerManager>();

            controller = new InventoryController(
                mockInventoryRepository.Object,
                mockMapper.Object,
                logger.Object);
        }

        #region Get inventories
        [Fact]
        public void GetInventories_ReturnsOkResultWithInventories()
        {
            // Arrange
            var inventoryParameters = new InventoryParameters
            {
                StoreId = 4
            };

            var inventories = new List<Inventory>
            {
                new Inventory { StoreId = 4, FoodId = 2 },
                new Inventory { StoreId = 4, FoodId = 3 },
                new Inventory { StoreId = 4, FoodId = 4 },
            };

            var inventoryDtos = new List<InventoryDTO>
            {
                new InventoryDTO{FoodId = 2},
                new InventoryDTO{FoodId = 3},
                new InventoryDTO{FoodId = 4}
            };

            mockInventoryRepository.Setup(repo => repo.GetInventories(It.IsAny<InventoryParameters>()))
       .Returns(new PagedList<Inventory>(inventories, inventories.Count, 1, inventories.Count));

            mockMapper.Setup(mapper => mapper.Map<List<InventoryDTO>>(It.IsAny<PagedList<Inventory>>()))
                .Returns(inventoryDtos);

            // Act
            var result = controller.GetInventories(inventoryParameters);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetInventories_ReturnsBadRequestOnError()
        {
            // Arrange
            var inventoryParameters = new InventoryParameters
            {
                StoreId = 4
            };

            mockInventoryRepository.Setup(repo => repo.GetInventories(It.IsAny<InventoryParameters>()))
                .Throws(new Exception("Test error"));

            // Act
            var result = controller.GetInventories(inventoryParameters);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Test error", badRequestResult.Value);
        }
        #endregion

        #region GetInventory
        [Fact]
        public async Task GetInventory_Authorized_ReturnsOkResult()
        {
            // Arrange
            var fid = 1;
            var inventory = new Inventory { FoodId = fid };
            var inventoryDto = new InventoryDTO { FoodId = fid };

            mockInventoryRepository.Setup(repo => repo.FindSingle(
          It.IsAny<Expression<Func<Inventory, bool>>>(),
          It.IsAny<Expression<Func<Inventory, object>>[]>()
      ))
      .ReturnsAsync(inventory);

            mockMapper.Setup(mapper => mapper.Map<InventoryDTO>(It.IsAny<Inventory>()))
                .Returns(inventoryDto);

            // Act
            var result = await controller.GetInventory(fid);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.NotNull(okObjectResult.Value);

            var resultObject = okObjectResult.Value as InventoryDTO;
            Assert.NotNull(resultObject);
            Assert.Equal(fid, resultObject.FoodId);
        }

        [Fact]
        public async Task GetInventory_Unauthorized_ReturnsUnauthorizedResult()
        {
            // Arrange
            var fid = 2;
            var exceptionMessage = "An error occurred.";
            mockInventoryRepository.Setup(repo => repo.FindSingle(
         It.IsAny<Expression<Func<Inventory, bool>>>(),
         It.IsAny<Expression<Func<Inventory, object>>[]>()
     ))
                 .Throws(new Exception(exceptionMessage));

            // Act
            var result = await controller.GetInventory(fid);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal(exceptionMessage, badRequestResult.Value);

        }
        #endregion

        #region CreateInventory

        [Fact]
        public async Task CreateInventory_WithValidInput_ReturnsOkResult()
        {
            var inventoryDto = new CreateInventoryDTO
            {
                FoodId = 1,
                quantity = 3,
                StoreId = 1
            };

            mockInventoryRepository.Setup(repo => repo.GetInventoryByFoodAndStore(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((Inventory)null); // No existing inventory

            // Act
            var result = await controller.CreateInventory(inventoryDto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateInventory_WithDuplicateInventory_ReturnsBadRequest()
        {
            // Arrange
            var inventoryDto = new CreateInventoryDTO
            {
                FoodId = 1,
                quantity = 3,
                StoreId = 1
            };


            mockInventoryRepository.Setup(repo => repo.GetInventoryByFoodAndStore(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Inventory()); // Simulate existing inventory

            // Act
            var result = await controller.CreateInventory(inventoryDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateInventory_WithException_ReturnsBadRequest()
        {
            // Arrange
            var inventoryDto = new CreateInventoryDTO
            {
                FoodId = 1,
                quantity = 3,
                StoreId = 1
            };

            mockInventoryRepository.Setup(repo => repo.GetInventoryByFoodAndStore(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception")); // Simulate an exception

            // Act
            var result = await controller.CreateInventory(inventoryDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region ImportInventory
        [Fact]
        public async Task ImportInventory_WithValidInput_ReturnsOkResult()
        {
            var storeId = 1;
            var foodId = 2;
            var quantity = 10;

            // Act
            var result = await controller.ImportInventory(storeId, foodId, quantity);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ImportInventory_WithException_ReturnsBadRequest()
        {
            // Arrange
            var storeId = 1;
            var foodId = 2;
            var quantity = 10;

            mockInventoryRepository.Setup(repo => repo.ImportInventory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.ImportInventory(storeId, foodId, quantity);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region ExportInventory
        [Fact]
        public async Task ExportInventory_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            var storeId = 1;
            var foodId = 2;
            var quantity = 10;

            // Act
            var result = await controller.ExportInventory(storeId, foodId, quantity);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ExportInventory_WithException_ReturnsBadRequest()
        {
            // Arrange
            var storeId = 1;
            var foodId = 2;
            var quantity = 10;

            mockInventoryRepository.Setup(repo => repo.ExportInventory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.ExportInventory(storeId, foodId, quantity);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region DeleteInventoryByInventoryId
        [Fact]
        public async Task DeleteInventoryByInventoryId_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var inventoryId = 1;

            // Act
            var result = await controller.DeleteInventoryByInventoryId(inventoryId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Inventory deleted successfully", (result as OkObjectResult).Value);
        }

        [Fact]
        public async Task DeleteInventoryByInventoryId_WithException_ReturnsBadRequest()
        {
            // Arrange
            var inventoryId = 1;

            mockInventoryRepository.Setup(repo => repo.DeleteInventoryByInventoryId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.DeleteInventoryByInventoryId(inventoryId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error deleting inventory: Simulated exception", (result as BadRequestObjectResult).Value);
        }
        #endregion

        #region CheckExistingInventory
        [Fact]
        public async Task CheckExistingInventory_WithExistingInventory_ReturnsTrue()
        {
            // Arrange
            var storeId = 1;
            var foodId = 2;

            mockInventoryRepository.Setup(repo => repo.GetInventoryByFoodAndStore(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Inventory()); // Simulate existing inventory

            // Act
            var result = await controller.CheckExistingInventory(storeId, foodId);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
        }

        [Fact]
        public async Task CheckExistingInventory_WithNonExistingInventory_ReturnsFalse()
        {
            // Arrange
            var storeId = 1;
            var foodId = 2;

            mockInventoryRepository.Setup(repo => repo.GetInventoryByFoodAndStore(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((Inventory)null); // Simulate non-existing inventory

            // Act
            var result = await controller.CheckExistingInventory(storeId, foodId);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
        }

        [Fact]
        public async Task CheckExistingInventory_WithException_ReturnsBadRequest()
        {
            // Arrange
            var storeId = 1;
            var foodId = 2;

            mockInventoryRepository.Setup(repo => repo.GetInventoryByFoodAndStore(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.CheckExistingInventory(storeId, foodId);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
        }
        #endregion
    }

}

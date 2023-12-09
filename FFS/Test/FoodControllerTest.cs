using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Migrations;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class FoodControllerTest
    {
        private Mock<IFoodRepository> _foodRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IComboRepository> _comboRepoMock;
        private Mock<ICommentRepository> _commentRepoMock;
        private Mock<IWishlistRepository> _wishlistRepoMock;
        private Mock<IOrderRepository> _orderRepoMock;
        private Mock<IInventoryRepository> _inventoryRepoMock;

        private FoodController _foodController;

        public FoodControllerTest()
        {
            _foodRepoMock = new Mock<IFoodRepository>();
            _mapperMock = new Mock<IMapper>();
            _comboRepoMock = new Mock<IComboRepository>();
            _orderRepoMock = new Mock<IOrderRepository>();
            _wishlistRepoMock = new Mock<IWishlistRepository>();
            _inventoryRepoMock = new Mock<IInventoryRepository>();
            _commentRepoMock = new Mock<ICommentRepository>();
            _foodController = new FoodController(_foodRepoMock.Object, _comboRepoMock.Object, _commentRepoMock.Object, _wishlistRepoMock.Object, _orderRepoMock.Object,  _mapperMock.Object, _inventoryRepoMock.Object);
        }
        #region ListFood
        [Fact]
        public void ListFood_ReturnsOkResult()
        {
            // Arrange
            var foodParameters = new FoodParameters
            {
                uId = "a8ace8f5-a853-40d8-9f4e-1e722c200769",
                FoodName = "Bánh",
                PageSize = 20, // Set your desired page size for testing
                PageNumber = 2 // Set your desired page number for testing
            };

            var mockFoods = new List<Food>(); // Mock data for foods
            var total = mockFoods.Count;

            _foodRepoMock.Setup(repo => repo.GetFoods(It.IsAny<FoodParameters>())).Returns(mockFoods);
            _foodRepoMock.Setup(repo => repo.CountGetFoods(It.IsAny<FoodParameters>())).Returns(total);

            // Act
            var result = _foodController.ListFood(foodParameters) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void ListFood_ReturnsInternalServerErrorOnException()
        {
            // Arrange
            var foodParameters = new FoodParameters
            {
                uId = "a8ace8f5-a853-40d8-9f4e-1e722c200769",
                FoodName = "Bánh",
                PageSize = 20, // Set your desired page size for testing
                PageNumber = 2 // Set your desired page number for testing
            };

            _foodRepoMock.Setup(repo => repo.GetFoods(It.IsAny<FoodParameters>())).Throws(new Exception("Simulated exception"));

            // Act
            var result = _foodController.ListFood(foodParameters) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);

            var exceptionMessage = result.Value as string;
            Assert.NotNull(exceptionMessage);
            Assert.Contains("Simulated exception", exceptionMessage);
        }
        #endregion

        #region GetFoodById
        [Fact]
        public async Task GetFoodById_ReturnsOkResultWithData()
        {
            // Arrange
            var foodId = 1;
            var mockFood = new Food { Id = foodId, FoodName = "TestFood" };

            _foodRepoMock.Setup(repo => repo.GetFoodById(foodId)).ReturnsAsync(mockFood);

            // Act
            var result = await _foodController.GetFoodById(foodId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var data = result.Value as Food;
            Assert.NotNull(data);
            Assert.Equal(mockFood.Id, data.Id);
            Assert.Equal(mockFood.FoodName, data.FoodName);
        }

        [Fact]
        public async Task GetFoodById_ReturnsNotFoundResult()
        {
            // Arrange
            var foodId = 2;

            _foodRepoMock.Setup(repo => repo.GetFoodById(foodId)).ReturnsAsync((Food)null);

            // Act
            var result = await _foodController.GetFoodById(foodId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetFoodById_ReturnsInternalServerErrorOnException()
        {
            // Arrange
            var foodId = 3;

            _foodRepoMock.Setup(repo => repo.GetFoodById(foodId)).ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _foodController.GetFoodById(foodId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);

            var exceptionMessage = result.Value as string;
            Assert.NotNull(exceptionMessage);
            Assert.Contains("Simulated exception", exceptionMessage);
        }
        #endregion

        #region GetFoodByCategoryid
        [Fact]
        public async Task GetFoodByCategoryId_ReturnsOkResultWithData()
        {
            // Arrange
            var categoryId = 1;
            var mockFoods = new List<Food>
            {
                new Food { Id = 1, FoodName = "Food1", CategoryId = categoryId, IsDelete = false },
                new Food { Id = 2, FoodName = "Food2", CategoryId = categoryId, IsDelete = false }
            };

            _foodRepoMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Food, bool>>>())).Returns(mockFoods.AsQueryable());

            // Act
            var result = await _foodController.GetFoodByCategoryid(categoryId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetFoodByCategoryId_ReturnsOkResultWithEmptyList()
        {
            // Arrange
            var categoryId = 2;

            _foodRepoMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Food, bool>>>())).Returns(new List<Food>().AsQueryable());

            // Act
            var result = await _foodController.GetFoodByCategoryid(categoryId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetFoodByCategoryId_ReturnsInternalServerErrorOnException()
        {
            // Arrange
            var categoryId = 3;

            _foodRepoMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Food, bool>>>())).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _foodController.GetFoodByCategoryid(categoryId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);

            var exceptionMessage = result.Value as string;
            Assert.NotNull(exceptionMessage);
            Assert.Contains("Simulated exception", exceptionMessage);
        }
        #endregion

        #region AddFood
        [Fact]
        public async Task AddFood_ReturnsOkResult()
        {
            // Arrange
            var foodDTO = new FoodDTO
            {
                CategoryId = 1,
                StoreId = 1,
                FoodName = "TestFood",
                ImageURL = "test.jpg",
                Description = "Description",
                Price = 10.99m
            };

            var addedFoodId = 1; // Assuming this is the ID assigned to the new food after adding.

            var food = new Food();
            _mapperMock.Setup(x => x.Map<Food>(It.IsAny<FoodDTO>())).Returns(food);
            _foodRepoMock.Setup(repo => repo.Add(It.IsAny<Food>()))
                         .Callback<Food>(food => { food.Id = addedFoodId; })
                         .Returns(Task.CompletedTask);

            _inventoryRepoMock.Setup(repo => repo.Add(It.IsAny<Inventory>()))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _foodController.AddFood(foodDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task AddFood_ReturnsInternalServerErrorOnFoodAddException()
        {
            // Arrange
            var foodDTO = new FoodDTO
            {
                CategoryId = 1,
                StoreId = 1,
                FoodName = "TestFood",
                ImageURL = "test.jpg",
                Description = "Description",
                Price = 10.99m
            };

            _foodRepoMock.Setup(repo => repo.Add(It.IsAny<Food>()))
                         .ThrowsAsync(new Exception("Simulated food add exception"));

            // Act
            var result = await _foodController.AddFood(foodDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);

            var exceptionMessage = result.Value as string;
            Assert.NotNull(exceptionMessage);
            Assert.Contains("Simulated food add exception", exceptionMessage);
        }

        #endregion

        #region UpdateFood
        [Fact]
        public async Task UpdateFood_WithExistingFood_ReturnsOkResult()
        {
            // Arrange
            int foodId = 1;
            var foodDTO = new FoodDTO
            {
                CategoryId = 2,
                Description = "Updated Description",
                FoodName = "Updated Food",
                ImageURL = "http://example.com/updated-image.jpg",
                Price = 9.99m
            };
            var existingFood = new Food { Id = foodId };

            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync(existingFood);

            // Act
            var result = await _foodController.UpdateFood(foodId, foodDTO);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateFood_WithNonExistingFood_ReturnsNotFoundResult()
        {
            // Arrange
            int foodId = 1;
            var foodDTO = new FoodDTO();

            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync((Food)null);

            // Act
            var result = await _foodController.UpdateFood(foodId, foodDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task UpdateFood_WithValidData_UpdatesFoodProperties()
        {
            // Arrange
            int foodId = 1;
            var foodDTO = new FoodDTO
            {
                CategoryId = 2,
                Description = "Updated Description",
                FoodName = "Updated Food",
                ImageURL = "http://example.com/updated-image.jpg",
                Price = 9.99m
            };
            var existingFood = new Food { Id = foodId };

            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync(existingFood);

            // Act
            var result = await _foodController.UpdateFood(foodId, foodDTO);

            // Assert
            Assert.Equal(foodDTO.CategoryId, existingFood.CategoryId);
            Assert.Equal(foodDTO.Description, existingFood.Description);
            Assert.Equal(foodDTO.FoodName, existingFood.FoodName);
            Assert.Equal(foodDTO.ImageURL, existingFood.ImageURL);
            Assert.Equal(foodDTO.Price, existingFood.Price);
        }
        [Fact]
        public async Task UpdateFood_UpdatesImageURLIfNotNullOrEmpty()
        {
            // Arrange
            var foodDTO = new FoodDTO
            {
                CategoryId = 1,
                FoodName = "UpdatedFood",
                ImageURL = "updated.jpg",
                Description = "UpdatedDescription",
                Price = 15.99m
            };

            var foodId = 1;
            var existingFood = new Food
            {
                Id = foodId,
                CategoryId = 1,
                FoodName = "OriginalFood",
                ImageURL = "original.jpg",
                Description = "OriginalDescription",
                Price = 10.99m
            };

            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync(existingFood);

            // Setup IMapper
            _mapperMock.Setup(mapper => mapper.Map<Food>(foodDTO))
                       .Returns(new Food // Return a new instance of Food with mapped values
                       {
                           Id = foodId,
                           CategoryId = foodDTO.CategoryId ?? 0,
                           FoodName = foodDTO.FoodName,
                           ImageURL = foodDTO.ImageURL,
                           Description = foodDTO.Description,
                           Price = foodDTO.Price ?? 0
                       });

            _foodRepoMock.Setup(repo => repo.Update(existingFood, It.IsAny<string[]>()))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _foodController.UpdateFood(foodId, foodDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateFood_DoesNotUpdateImageURLIfNullOrEmpty()
        {
            // Arrange
            var foodDTO = new FoodDTO
            {
                CategoryId = 1,
                FoodName = "UpdatedFood",
                ImageURL = null, // or string.Empty
                Description = "UpdatedDescription",
                Price = 15.99m
            };

            var foodId = 1;
            var existingFood = new Food
            {
                Id = foodId,
                CategoryId = 1,
                FoodName = "OriginalFood",
                ImageURL = "original.jpg",
                Description = "OriginalDescription",
                Price = 10.99m
            };

            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync(existingFood);

            // Setup IMapper
            _mapperMock.Setup(mapper => mapper.Map<Food>(foodDTO))
                       .Returns(new Food // Return a new instance of Food with mapped values
                       {
                           Id = foodId,
                           CategoryId = foodDTO.CategoryId ?? 0,
                           FoodName = foodDTO.FoodName,
                           ImageURL = foodDTO.ImageURL,
                           Description = foodDTO.Description,
                           Price = foodDTO.Price ?? 0
                       });

            _foodRepoMock.Setup(repo => repo.Update(existingFood, It.IsAny<string[]>()))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _foodController.UpdateFood(foodId, foodDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateFood_WithException_ReturnsInternalServerError()
        {
            // Arrange
            int foodId = 1;
            var foodDTO = new FoodDTO();
            var existingFood = new Food { Id = foodId };

            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync(existingFood);
            _foodRepoMock.Setup(repo => repo.Update(existingFood)).ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await _foodController.UpdateFood(foodId, foodDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region DeleteFood
        [Fact]
        public async Task DeleteFood_ReturnsOkResult()
        {
            // Arrange
            var foodId = 1;
            var existingFood = new Food
            {
                Id = foodId,
                FoodName = "TestFood",
                IsDelete = false
            };

            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync(existingFood);
            _foodRepoMock.Setup(repo => repo.Update(existingFood)).Returns(Task.CompletedTask);

            // Act
            var result = await _foodController.DeleteFood(foodId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
        [Fact]
        public async Task DeleteFood_ReturnsNotFoundResult()
        {
            // Arrange
            var foodId = 2; // Assuming this food ID doesn't exist
            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync((Food)null);

            // Act
            var result = await _foodController.DeleteFood(foodId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
        [Fact]
        public async Task DeleteFood_ReturnsInternalServerErrorOnException()
        {
            // Arrange
            var foodId = 3;
            var existingFood = new Food
            {
                Id = foodId,
                FoodName = "TestFood",
                IsDelete = false
            };

            _foodRepoMock.Setup(repo => repo.FindById(foodId, null)).ReturnsAsync(existingFood);
            _foodRepoMock.Setup(repo => repo.Update(existingFood)).ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _foodController.DeleteFood(foodId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);

            var exceptionMessage = result.Value as string;
            Assert.NotNull(exceptionMessage);
            Assert.Contains("Simulated exception", exceptionMessage);
        }
        #endregion

        #region GetListCombo
        [Fact]
        public async Task GetListCombo_ReturnsOkResult()
        {
            // Arrange
            var idStore = 1;
            var combos = new List<Combo>
            {
                new Combo { Id = 1, StoreId = idStore, IsDelete = false },
                new Combo { Id = 2, StoreId = idStore, IsDelete = false }
            };

            var comboDetails = new List<dynamic>
            {
                new { ComboId = 1, Detail = "Detail1" },
                new { ComboId = 2, Detail = "Detail2" }
            };

            _comboRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Combo, bool>>>()))
                      .ReturnsAsync(combos);

            _comboRepoMock.Setup(repo => repo.GetDetail(It.IsAny<int>()))
                       .ReturnsAsync((int comboId) => comboDetails.FindAll(detail => (int)detail.ComboId == comboId));

            // Act
            var result = await _foodController.GetListCombo(idStore) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseBody = result.Value as List<dynamic>;
            Assert.NotNull(responseBody);
            Assert.Equal(combos.Count, responseBody.Count);
        }
        [Fact]
        public async Task GetListCombo_ThrowsException()
        {
            // Arrange
            var idStore = 1;

            // Setup GetList to throw an exception
            _comboRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Combo, bool>>>()))
                      .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _foodController.GetListCombo(idStore));
        }
        #endregion

        #region GetDetailCombo
        #endregion
    }
}

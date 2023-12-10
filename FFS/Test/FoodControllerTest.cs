using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Migrations;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly Mock<ILoggerManager> _logger;
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
            _logger = new Mock<ILoggerManager>();
            _foodController = new FoodController(_foodRepoMock.Object, _comboRepoMock.Object, _commentRepoMock.Object, _wishlistRepoMock.Object, _orderRepoMock.Object, _mapperMock.Object, _inventoryRepoMock.Object, _logger.Object);
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
            Assert.IsType<OkObjectResult>(result);
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
        [Fact]
        public async Task GetDetailCombo_ReturnsOkResult()
        {
            // Arrange
            var comboId = 1;
            var comboDetails = new List<dynamic>
            {
                new { Id = comboId, Detail = "Detail1" },
                new { Id = comboId, Detail = "Detail2" }
            };

            _comboRepoMock.Setup(repo => repo.GetDetail(It.IsAny<int>()))
                               .ReturnsAsync(comboDetails);

            // Act
            var result = await _foodController.GetDetailCombo(comboId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseBody = result.Value as List<dynamic>;
            Assert.NotNull(responseBody);
            Assert.Equal(comboDetails.Count, responseBody.Count);
        }
        [Fact]
        public async Task GetDetailCombo_ThrowsException()
        {
            // Arrange
            var comboId = 1;

            // Setup GetDetail to throw an exception
            _comboRepoMock.Setup(repo => repo.GetDetail(It.IsAny<int>()))
                               .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _foodController.GetDetailCombo(comboId));
        }

        #endregion

        #region CreateCombo
        [Fact]
        public async Task CreateCombo_ReturnsOkResult()
        {
            // Arrange
            var comboFoodDTO = new ComboFoodDTO
            {
                Name = "TestCombo",
                StoreId = 1,
                Percent = 10,
                Image = "combo.jpg",
                IdFoods = new List<int> { 1, 2, 3 }
            };

            // Setup Add and AddComboFood to succeed
            _comboRepoMock.Setup(repo => repo.Add(It.IsAny<Combo>()))
                          .Returns(Task.FromResult(new Combo { Id = 1 })); // Simulate the assigned ID after adding to the repository

            _comboRepoMock.Setup(repo => repo.AddComboFood(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _foodController.CreateCombo(comboFoodDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Tạo thành công Combo!", result.Value);
        }
        [Fact]
        public async Task CreateCombo_ThrowsException()
        {
            // Arrange
            var comboFoodDTO = new ComboFoodDTO
            {
                Name = "TestCombo",
                StoreId = 1,
                Percent = 10,
                Image = "combo.jpg",
                IdFoods = new List<int> { 1, 2, 3 }
            };

            // Setup Add to succeed but AddComboFood to throw an exception
            _comboRepoMock.Setup(repo => repo.Add(It.IsAny<Combo>()))
                           .Returns(Task.FromResult(new Combo { Id = 1 })); // Simulate the assigned ID after adding to the repository

            _comboRepoMock.Setup(repo => repo.AddComboFood(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
                               .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _foodController.CreateCombo(comboFoodDTO));
        }
        #endregion

        #region UpdateCombo
        [Fact]
        public async Task UpdateCombo_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var comboFoodDTO = new ComboFoodDTO
            {
                Name = "UpdatedCombo",
                StoreId = 1,
                Percent = 20,
                IdFoods = new List<int> { 1, 2, 3 }
            };

            // Setup Update and UpdateComboFood to succeed
            _comboRepoMock.Setup(repo => repo.Update(It.IsAny<Combo>()))
                               .Returns(Task.CompletedTask);

            _comboRepoMock.Setup(repo => repo.UpdateComboFood(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
                      .Callback<int, int, List<int>>((comboId, storeId, idFoods) =>
                      {
                          Console.WriteLine($"ComboId: {comboId}, StoreId: {storeId}, IdFoods: {string.Join(", ", idFoods)}");
                      });

            // Act
            var result = await _foodController.UpdateCombo(id, comboFoodDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Cập nhật thành công Combo!", result.Value);
        }
        [Fact]
        public async Task UpdateCombo_ThrowsException()
        {
            // Arrange
            var id = 1;
            var comboFoodDTO = new ComboFoodDTO
            {
                Name = "UpdatedCombo",
                StoreId = 1,
                Percent = 20,
                IdFoods = new List<int> { 1, 2, 3 }
            };

            // Setup Update to succeed but UpdateComboFood to throw an exception
            _comboRepoMock.Setup(repo => repo.Update(It.IsAny<Combo>()))
                               .Returns(Task.CompletedTask);

            _comboRepoMock.Setup(repo => repo.UpdateComboFood(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
                               .Throws(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _foodController.UpdateCombo(id, comboFoodDTO));
        }
        #endregion

        #region DeleteCombo
        [Fact]
        public async Task DeleteCombo_ReturnsOkResult()
        {
            // Arrange
            var id = 1;

            // Setup DeleteCombo to succeed
            _comboRepoMock.Setup(repo => repo.DeleteCombo(It.IsAny<int>()))
                               .Callback<int>(comboId =>
                               {
                                   // Perform custom action, if needed
                                   Console.WriteLine($"DeleteCombo method called with comboId: {comboId}");
                               });

            // Act
            var result = await _foodController.DeleteCombo(id) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
        [Fact]
        public async Task DeleteFood_FoodNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingFoodId = 999; // Assuming this ID does not exist
            _foodRepoMock.Setup(repo => repo.FindById(nonExistingFoodId, null))
                            .ReturnsAsync((Food)null); // Simulate the case where food is not found

            // Act
            var result = await _foodController.DeleteFood(nonExistingFoodId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCombo_ThrowsException()
        {
            // Arrange
            var id = 1;

            // Setup DeleteCombo to throw an exception
            _comboRepoMock.Setup(repo => repo.DeleteCombo(It.IsAny<int>()))
                               .Throws(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _foodController.DeleteCombo(id));
        }

        #endregion

        #region
        [Fact]
        public async Task GetFoodByStoreId_ValidStoreId_ReturnsOkResult()
        {
            // Arrange
            int validStoreId = 1;
            var foodList = new List<Food> { new Food { Id = 1 } };
            _foodRepoMock.Setup(repo => repo.GetFoodListByStoreId(validStoreId))
                                .ReturnsAsync(foodList);

            // Act
            var result = await _foodController.GetFoodByStoreId(validStoreId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            // Add more assertions based on the expected result content
        }

        [Fact]
        public async Task GetFoodByStoreId_EmptyList_ReturnsNotFoundResult()
        {
            // Arrange
            int storeIdWithNoFood = 2; // Assuming this store has no food items
            _foodRepoMock.Setup(repo => repo.GetFoodListByStoreId(storeIdWithNoFood))
                                .ReturnsAsync(new List<Food>());

            // Act
            var result = await _foodController.GetFoodByStoreId(storeIdWithNoFood) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetFoodByStoreId_NullList_ReturnsNotFoundResult()
        {
            // Arrange
            int storeIdWithNullList = 3; // Assuming this store has a null list of food items
            _foodRepoMock.Setup(repo => repo.GetFoodListByStoreId(storeIdWithNullList))
                                .ReturnsAsync((List<Food>)null);

            // Act
            var result = await _foodController.GetFoodByStoreId(storeIdWithNullList) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetFoodByStoreId_ExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            int storeId = 1;
            _foodRepoMock.Setup(repo => repo.GetFoodListByStoreId(storeId))
                                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _foodController.GetFoodByStoreId(storeId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Error: Simulated exception", result.Value);
        }
        #endregion

        #region RatingFood
        [Fact]
        public async Task RatingFood_Rate5AndWishlistEmpty_AddsToWishlistAndRatesFood_ReturnsOkResult()
        {
            // Arrange
            var foodRatingDTO = new FoodRatingDTO
            {
                UserId = "testUserId",
                FoodId = 1,
                Rate = 5
            };
            _wishlistRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Wishlist, bool>>>()))
                                   .ReturnsAsync(new List<Wishlist>());

            // Act
            var result = await _foodController.RatingFood(foodRatingDTO) as ObjectResult;

            // Assert
            _wishlistRepoMock.Verify(repo => repo.AddToWishlist(foodRatingDTO.UserId, foodRatingDTO.FoodId), Times.Once);
            _commentRepoMock.Verify(repo => repo.RatingFood(It.IsAny<Comment>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True((bool)result.Value.GetType().GetProperty("IsCanRate").GetValue(result.Value));
        }
        [Fact]
        public async Task RatingFood_Rate5AndWishlistNotEmpty_DoesNotAddToWishlist_ReturnsOkResult()
        {
            // Arrange
            var foodRatingDTO = new FoodRatingDTO
            {
                UserId = "testUserId",
                FoodId = 1,
                Rate = 5
            };
            _wishlistRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Wishlist, bool>>>()))
                                   .ReturnsAsync(new List<Wishlist> { new Wishlist() });

            // Act
            var result = await _foodController.RatingFood(foodRatingDTO) as ObjectResult;

            // Assert
            _wishlistRepoMock.Verify(repo => repo.AddToWishlist(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
            _commentRepoMock.Verify(repo => repo.RatingFood(It.IsAny<Comment>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True((bool)result.Value.GetType().GetProperty("IsCanRate").GetValue(result.Value));
        }
        [Fact]
        public async Task RatingFood_ExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var foodRatingDTO = new FoodRatingDTO
            {
                UserId = "testUserId",
                FoodId = 1,
                Rate = 5
            };
            _wishlistRepoMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Wishlist, bool>>>()))
                                   .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _foodController.RatingFood(foodRatingDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Simulated exception", result.Value);
        }
        #endregion
        #region ListAllFood
        [Fact]
        public void ListAllFood_ValidParameters_ReturnsOkResult()
        {
            // Arrange
            var allFoodParameters = new AllFoodParameters
            {
                // Set valid parameters
                PageNumber = 1,
                PageSize = 10,
                // Set other parameters based on your scenario
            };

            var foods = new PagedList<Food>(new List<Food>
    {
        // Set paginated food data
        new Food { Id = 1, FoodName = "A" },
        new Food { Id = 2, FoodName = "B" },
        // Add more foods as needed
    }, 1, 1, 10); // Assuming 1 is the total count, 10 is the page size

            var foodDTOs = new List<AllFoodDTO>
    {
        // Create AllFoodDTO objects or use AutoMapper to map from Food to AllFoodDTO
        new AllFoodDTO { Id = 1, FoodName = "A" },
        new AllFoodDTO { Id = 2, FoodName = "B" },
    };

            _foodRepoMock.Setup(repo => repo.GetAllFoods(allFoodParameters))
                               .Returns(foods);

            _mapperMock.Setup(mapper => mapper.Map<List<AllFoodDTO>>(foods))
                       .Returns(foodDTOs);

            // Act
            var result = _foodController.ListAllFood(allFoodParameters) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public void ListAllFood_ExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var allFoodParameters = new AllFoodParameters
            {
                // Set valid or invalid parameters
            };

            _foodRepoMock.Setup(repo => repo.GetAllFoods(allFoodParameters))
                               .Throws(new Exception("Simulated exception"));

            // Act
            var result = _foodController.ListAllFood(allFoodParameters) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Simulated exception", result.Value);
        }
        #endregion

        #region GetFoodRecommend
        [Fact]
        public async Task GetFoodRecommend_ReturnsOkResult()
        {
            // Arrange
            var homeFood = new List<Food>
    {
        new Food { Id = 1, FoodName = "A" },
        new Food { Id = 2, FoodName = "B" },
    };

            var mappedFoodDTOs = new List<AllFoodDTO>
    {
        new AllFoodDTO { Id = 1, FoodName = "A" },
        new AllFoodDTO { Id = 2, FoodName = "B" },
    };

            _foodRepoMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Food, bool>>>()))
                         .Returns(homeFood.AsQueryable());

            _mapperMock.Setup(mapper => mapper.Map<List<AllFoodDTO>>(It.IsAny<List<Food>>()))
                       .Returns(mappedFoodDTOs);

            // Act
            var result = await _foodController.GetFoodRecommend() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public async Task GetFoodRecommend_ExceptionThrown_ReturnsStatusCode500()
        {
            // Arrange
            _foodRepoMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Food, bool>>>()))
                               .Throws(new Exception("Simulated exception"));

            // Act
            var result = await _foodController.GetFoodRecommend() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Simulated exception", result.Value);
        }
        #endregion

        #region CommentFood
        [Fact]
        public async Task CommentFood_Success_ReturnsNoContent()
        {
            // Arrange
            var comment = new Comment { Id = 1, Content = "Hay" };

            _commentRepoMock.Setup(repo => repo.Add(comment)).Verifiable();

            // Act
            var result = await _foodController.CommentFood(comment) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);

            _commentRepoMock.Verify(repo => repo.Add(comment), Times.Once);
        }

        [Fact]
        public async Task CommentFood_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var comment = new Comment {Id = 1, Content = "Hay"};

            _commentRepoMock.Setup(repo => repo.Add(comment))
                            .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _foodController.CommentFood(comment) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            // Additional assertions based on your exception handling logic
            Assert.Contains("Simulated exception", result.Value.ToString());
        }
        #endregion
    }
}


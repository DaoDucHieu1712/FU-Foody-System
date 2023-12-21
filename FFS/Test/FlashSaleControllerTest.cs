using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.FlashSale;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class FlashSaleControllerTest
    {
        private Mock<IFlashSaleRepository> _fsRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IFlashSaleDetailRepository> _fsDetailRepoMock;
        private Mock<ApplicationDbContext> mockDbContext;
        private FlashSaleController _fsController;
        private readonly Mock<ILoggerManager> _logger;

        public FlashSaleControllerTest()
        {
            _fsRepoMock = new Mock<IFlashSaleRepository>();
            _mapperMock = new Mock<IMapper>();
            _fsDetailRepoMock = new Mock<IFlashSaleDetailRepository>();
            mockDbContext = new Mock<ApplicationDbContext>();
            _logger = new Mock<ILoggerManager>();
            _fsController = new FlashSaleController(mockDbContext.Object, _fsRepoMock.Object, _fsDetailRepoMock.Object, _mapperMock.Object, _logger.Object);
        }

        #region ListFoodAvailable
        [Fact]
        public void ListFoodAvailable_ValidParameters_ReturnsOkResult()
        {
            // Arrange
            var parameters = new CheckFoodFlashSaleParameters
            {
                StoreId = 1,
                FoodName = "SampleFood",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                PageNumber = 1,
                PageSize = 10
            };

            var foods = new PagedList<Food>(new List<Food>
             {
                new Food { Id = 1, FoodName = "A" },
                new Food { Id = 2, FoodName = "B" },
            }, 1, 1, 10);
            var fsFoodDTOs = new List<FoodFlashSaleDTO>
    {
        // Create AllFoodDTO objects or use AutoMapper to map from Food to AllFoodDTO
        new FoodFlashSaleDTO { Id = 1, FoodName = "A" },
        new FoodFlashSaleDTO { Id = 2, FoodName = "B" },
    };

            _fsRepoMock.Setup(repo => repo.ListFoodAvailable(parameters))
                       .Returns(foods);

            _mapperMock.Setup(mapper => mapper.Map<List<FoodFlashSaleDTO>>(It.IsAny<PagedList<Food>>()))
                       .Returns(fsFoodDTOs);

            // Act
            var result = _fsController.ListFoodAvailable(parameters) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void ListFoodAvailable_ExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var parameters = new CheckFoodFlashSaleParameters
            {
                StoreId = 1,
                FoodName = "SampleFood",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                PageNumber = 1,
                PageSize = 10
            };

            _fsRepoMock.Setup(repo => repo.ListFoodAvailable(parameters))
                       .Throws(new Exception("Simulated exception"));

            // Act
            var result = _fsController.ListFoodAvailable(parameters) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
        #endregion

        #region CreateFlashSale
        [Fact]
        public async Task CreateFlashSale_ValidData_ReturnsOkResult()
        {
            // Arrange
            var flashSaleDTO = new FlashSaleDTO
            {
                Id = 1,
                StoreId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                FlashSaleDetails = new List<FlashSaleDetailDTO>
                {
                    new FlashSaleDetailDTO { FlashSaleId = 1,  FoodId = 1, Quantity = 5 },
                    new FlashSaleDetailDTO { FlashSaleId = 1, FoodId = 2, Quantity = 10 }
                },
            };

            var flashSaleEntity = new FlashSale
            {
                Id = 1,
                StoreId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
            };

            var flashSaleDetailEntity = new FlashSaleDetail
            {
                FlashSaleId = 1,
                FoodId = 1,
                NumberOfProductSale = 5
            };

            _mapperMock.Setup(mapper => mapper.Map<FlashSale>(It.IsAny<FlashSaleDTO>()))
              .Returns(flashSaleEntity);

            _fsDetailRepoMock.Setup(repo => repo.GetFlashSaleDetail(It.IsAny<int>(), It.IsAny<int>()))
                     .ReturnsAsync((int foodId, int flashSaleId) =>
                     {
                         // Handle different scenarios based on foodId and flashSaleId
                         if (foodId == 1 && flashSaleId == 1)
                         {
                             return null; // Simulate not found
                         }

                         // Return appropriate FlashSaleDetailEntity for other cases
                         return flashSaleDetailEntity;
                     });

            _mapperMock.Setup(mapper => mapper.Map<FlashSaleDetail>(It.IsAny<FlashSaleDetailDTO>()))
               .Returns(flashSaleDetailEntity);

            // Act
            var result = await _fsController.CreateFlashSale(flashSaleDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
        [Fact]
        public async Task CreateFlashSale_ValidData2_ReturnsOkResult()
        {
            // Arrange
            var flashSaleDTO = new FlashSaleDTO
            {
                Id = 1,
                StoreId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                FlashSaleDetails = new List<FlashSaleDetailDTO>
                {
                    new FlashSaleDetailDTO { FlashSaleId = 1,  FoodId = 1, Quantity = 5 },
                    new FlashSaleDetailDTO { FlashSaleId = 1, FoodId = 2, Quantity = 10 }
                },
            };

            var flashSaleEntity = new FlashSale
            {
                Id = 1,
                StoreId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
            };

            var flashSaleDetailEntity = new FlashSaleDetail
            {
                FlashSaleId = 1,
                FoodId = 2,
                NumberOfProductSale = 5
            };

            _mapperMock.Setup(mapper => mapper.Map<FlashSale>(It.IsAny<FlashSaleDTO>()))
              .Returns(flashSaleEntity);

            _fsDetailRepoMock.Setup(repo => repo.GetFlashSaleDetail(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns((int foodId, int flashSaleId) => Task.FromResult<FlashSaleDetail?>(null));

            _mapperMock.Setup(mapper => mapper.Map<FlashSaleDetail>(It.IsAny<FlashSaleDetailDTO>()))
               .Returns(flashSaleDetailEntity);

            // Act
            var result = await _fsController.CreateFlashSale(flashSaleDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
        [Fact]
        public async Task CreateFlashSale_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var flashSaleDTO = new FlashSaleDTO
            {
                Id = 1,
                StoreId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                FlashSaleDetails = new List<FlashSaleDetailDTO>
                {
                    new FlashSaleDetailDTO { FlashSaleId = 1,  FoodId = 1, Quantity = 5 },
                    new FlashSaleDetailDTO { FlashSaleId = 1, FoodId = 2, Quantity = 10 }
                },
            };

            _mapperMock.Setup(mapper => mapper.Map<FlashSale>(It.IsAny<FlashSaleDTO>()))
               .Returns(new FlashSale());

            _fsRepoMock.Setup(repo => repo.Add(It.IsAny<FlashSale>()))
               .ThrowsAsync(new Exception("Simulated exception"));
            // Act
            var result = await _fsController.CreateFlashSale(flashSaleDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Simulated exception", result.Value);
        }


        #endregion

        #region UpdateFlashSale
        [Fact]
        public async Task UpdateFlashSale_ExistingId_ReturnsOk()
        {
            // Arrange
            int existingFlashSaleId = 1;
            var flashSaleDTO = new FlashSaleDTO
            {
                Id = 1,
                StoreId = 4
            };

            var existingFlashSale = new FlashSale
            {
                Id = existingFlashSaleId,
                StoreId = 4
            };

            _fsRepoMock.Setup(repo => repo.FindSingle(
    It.IsAny<Expression<Func<FlashSale, bool>>>(),
    It.IsAny<Expression<Func<FlashSale, object>>[]>())
)
.ReturnsAsync(existingFlashSale);


            // Act
            var result = await _fsController.UpdateFlashSale(existingFlashSaleId, flashSaleDTO);

            // Assert
            _fsRepoMock.Verify(repo => repo.Update(It.IsAny<FlashSale>()), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateFlashSale_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingFlashSaleId = 2;
            var flashSaleDTO = new FlashSaleDTO
            {
                Id = 1,
                StoreId = 4
            };

            _fsRepoMock.Setup(repo => repo.FindSingle(
    It.IsAny<Expression<Func<FlashSale, bool>>>(),
    It.IsAny<Expression<Func<FlashSale, object>>[]>())
)
                .ReturnsAsync((FlashSale)null);

            // Act
            var result = await _fsController.UpdateFlashSale(nonExistingFlashSaleId, flashSaleDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateFlashSale_Exception_ReturnsInternalServerError()
        {
            // Arrange
            int flashSaleId = 1;
            var flashSaleDTO = new FlashSaleDTO
            {
                Id = 1,
                StoreId = 4
            };

            _fsRepoMock.Setup(repo => repo.FindSingle(
     It.IsAny<Expression<Func<FlashSale, bool>>>(),
     It.IsAny<Expression<Func<FlashSale, object>>[]>())
 )
                 .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _fsController.UpdateFlashSale(flashSaleId, flashSaleDTO);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        #endregion

        #region DeleteFlashSale
        [Fact]
        public async Task DeleteFlashSale_ExistingId_ReturnsOk()
        {
            // Arrange
            int existingFlashSaleId = 1;
            var existingFlashSaleEntity = new FlashSale { Id = existingFlashSaleId };

            _fsRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<FlashSale, bool>>>()))
                .ReturnsAsync(existingFlashSaleEntity);

            _fsRepoMock.Setup(repo => repo.DeleteFlashSale(It.IsAny<int>()))
    .Returns(Task.CompletedTask);


            // Act
            var result = await _fsController.DeleteFlashSale(existingFlashSaleId);

            // Assert
            _fsRepoMock.Verify(repo => repo.DeleteFlashSale(existingFlashSaleEntity.Id), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteFlashSale_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingFlashSaleId = 2;

            _fsRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<FlashSale, bool>>>()))
                .ReturnsAsync((FlashSale)null);

            // Act
            var result = await _fsController.DeleteFlashSale(nonExistingFlashSaleId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Flash sale with ID {nonExistingFlashSaleId} not found", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task DeleteFlashSale_Exception_ReturnsBadRequest()
        {
            // Arrange
            int flashSaleId = 1;
            var existingFlashSaleEntity = new FlashSale { Id = flashSaleId };

            _fsRepoMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<FlashSale, bool>>>()))
                .ReturnsAsync(existingFlashSaleEntity);

            _fsRepoMock.Setup(repo => repo.DeleteFlashSale(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _fsController.DeleteFlashSale(flashSaleId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Simulated exception", (result as BadRequestObjectResult)?.Value);
        }
        #endregion

        #region DeleteFlashSaleDetail
        [Fact]
        public async Task DeleteFlashSaleDetail_ExistingIds_ReturnsOk()
        {
            // Arrange
            int existingFlashSaleId = 1;
            int existingFoodId = 1;

            _fsRepoMock.Setup(repo => repo.DeleteFlashSaleDetail(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _fsController.DeleteFlashSaleDetail(existingFlashSaleId, existingFoodId);

            // Assert
            _fsRepoMock.Verify(repo => repo.DeleteFlashSaleDetail(existingFlashSaleId, existingFoodId), Times.Once);
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task DeleteFlashSaleDetail_NonExistingIds_ReturnsOk()
        {
            // Arrange
            int nonExistingFlashSaleId = 2;
            int nonExistingFoodId = 2;

            _fsRepoMock.Setup(repo => repo.DeleteFlashSaleDetail(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _fsController.DeleteFlashSaleDetail(nonExistingFlashSaleId, nonExistingFoodId);

            // Assert
            _fsRepoMock.Verify(repo => repo.DeleteFlashSaleDetail(nonExistingFlashSaleId, nonExistingFoodId), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Simulated exception", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task DeleteFlashSaleDetail_Exception_ReturnsBadRequest()
        {
            // Arrange
            int flashSaleId = 1;
            int foodId = 1;

            _fsRepoMock.Setup(repo => repo.DeleteFlashSaleDetail(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _fsController.DeleteFlashSaleDetail(flashSaleId, foodId);

            // Assert
            _fsRepoMock.Verify(repo => repo.DeleteFlashSaleDetail(flashSaleId, foodId), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Simulated exception", (result as BadRequestObjectResult)?.Value);
        }
        #endregion

        #region FlashSaleDetail
        [Fact]
        public async Task FlashSaleDetail_ExistingId_ReturnsOkWithFlashSaleDTO()
        {
            // Arrange
            int existingFlashSaleId = 1;
            var existingFlashSaleEntity = new FlashSale { Id = existingFlashSaleId, /* set other properties */ };
            var expectedFlashSaleDTO = new FlashSaleDTO { /* set properties based on your mapping */ };

            _fsRepoMock.Setup(repo => repo.FindSingle(
     It.IsAny<Expression<Func<FlashSale, bool>>>(),
     It.IsAny<Expression<Func<FlashSale, object>>[]>())
 )
 .ReturnsAsync(existingFlashSaleEntity);


            _mapperMock.Setup(mapper => mapper.Map<FlashSaleDTO>(existingFlashSaleEntity))
                .Returns(expectedFlashSaleDTO);

            // Act
            var result = await _fsController.FlashSaleDetail(existingFlashSaleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedFlashSaleDTO, okResult.Value);
        }

        [Fact]
        public async Task FlashSaleDetail_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingFlashSaleId = 2;

            _fsRepoMock.Setup(repo => repo.FindSingle(
    It.IsAny<Expression<Func<FlashSale, bool>>>(),
    It.IsAny<Expression<Func<FlashSale, object>>[]>())
)
                .ReturnsAsync((FlashSale)null);

            // Act
            var result = await _fsController.FlashSaleDetail(nonExistingFlashSaleId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task FlashSaleDetail_Exception_ReturnsBadRequest()
        {
            // Arrange
            int flashSaleId = 1;

            _fsRepoMock.Setup(repo => repo.FindSingle(
     It.IsAny<Expression<Func<FlashSale, bool>>>(),
     It.IsAny<Expression<Func<FlashSale, object>>[]>())
 )
                 .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _fsController.FlashSaleDetail(flashSaleId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Simulated exception", (result as BadRequestObjectResult)?.Value);
        }
        #endregion

        #region ListFlashSaleByStore
        [Fact]
        public async Task ListFlashSaleByStore_ValidData_ReturnsOk()
        {
            // Arrange
            List<FlashSale> flashSales = new List<FlashSale>();

            var flashSaleParameters = new FlashSaleParameter { Start = DateTime.Now, PageNumber = 1, PageSize = 10 };
            var mockFlashSales = new PagedList<FlashSale>(flashSales, 1, 1, 10);
            var mockFlashSaleDTOs = new List<FlashSaleDTO> { /* mock your FlashSale DTOs data here */ };

            _fsRepoMock.Setup(repo => repo.ListFlashSaleByStore(It.IsAny<int>(), It.IsAny<FlashSaleParameter>()))
                .Returns(mockFlashSales);
            _mapperMock.Setup(mapper => mapper.Map<List<FlashSaleDTO>>(mockFlashSales))
                .Returns(mockFlashSaleDTOs);

            // Act
            var result = _fsController.ListFlashSaleByStore(1, flashSaleParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ListFlashSaleByStore_ExceptionOccurred_ReturnsInternalServerError()
        {
            // Arrange

            var flashSaleParameters = new FlashSaleParameter { Start = DateTime.Now, PageNumber = 1, PageSize = 10 };

            _fsRepoMock.Setup(repo => repo.ListFlashSaleByStore(It.IsAny<int>(), It.IsAny<FlashSaleParameter>()))
               .Throws(new Exception("Test exception"));

            // Act
            var result = _fsController.ListFlashSaleByStore(1, flashSaleParameters);

            // Assert
            var objiectCodeResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objiectCodeResult.StatusCode);
        }


        #endregion

        #region ListFlashSaleInTimeByStore 
        [Fact]
        public async Task ListFlashSaleInTimeByStore_ValidData_ReturnsOk()
        {
            // Arrange
            int storeId = 1;
            var mockFlashSales = new List<FlashSale>();
            var mockFlashSaleDTOs = new List<FlashSaleDTO>(); 

            _fsRepoMock.Setup(repo => repo.ListFoodFlashSaleInTimeByStore(storeId))
                .ReturnsAsync(mockFlashSales);
            _mapperMock.Setup(mapper => mapper.Map<List<FlashSaleDTO>>(mockFlashSales))
                .Returns(mockFlashSaleDTOs);

            // Act
            var result = await _fsController.ListFlashSaleInTimeByStore(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ListFlashSaleInTimeByStore_Exception_ReturnsBadRequest()
        {
            // Arrange
            int storeId = 1;
            _fsRepoMock.Setup(repo => repo.ListFoodFlashSaleInTimeByStore(storeId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await _fsController.ListFlashSaleInTimeByStore(storeId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Some error message", badRequestResult.Value);
        }
        #endregion
    }
}

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
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

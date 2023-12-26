using AutoMapper;
using DocumentFormat.OpenXml.Office2021.PowerPoint.Comment;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;
using Comment = FFS.Application.Entities.Comment;

namespace Test
{
    public class StoreControllerTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IStoreRepository> _storeRepositoryMock;
        private readonly Mock<IFoodRepository> _foodRepositoryMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IComboRepository> _comboRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<ILocationRepository> _locationRepositorMocky;
        private Mock<ILoggerManager> _logger;
        private StoreController _controller;
        public StoreControllerTest()
        {
            _mapperMock = new Mock<IMapper>();
            _storeRepositoryMock = new Mock<IStoreRepository>();
            _foodRepositoryMock = new Mock<IFoodRepository>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _comboRepositoryMock = new Mock<IComboRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _locationRepositorMocky = new Mock<ILocationRepository>();
            _logger = new Mock<ILoggerManager>();
            _controller = new StoreController(_mapperMock.Object, _storeRepositoryMock.Object, _foodRepositoryMock.Object, _commentRepositoryMock.Object, _comboRepositoryMock.Object, _orderRepositoryMock.Object, _locationRepositorMocky.Object, _logger.Object);
        }

        #region ListAllStore
        [Fact]
        public void ListAllStore_ReturnsOkResultWithStoresAndMetadata()
        {
            // Arrange
            var allStoreParameters = new AllStoreParameters();
            var fakeStores = new PagedList<Store>(new List<Store> { new Store { Id = 1 } }, 1, 1, 10);

            _storeRepositoryMock.Setup(repo => repo.GetAllStores(allStoreParameters))
                .Returns(fakeStores);

            var fakeStoreDTOs = new List<AllStoreDTO> { /* Fake DTO data */ };
            _mapperMock.Setup(mapper => mapper.Map<List<AllStoreDTO>>(fakeStores))
                .Returns(fakeStoreDTOs);

            // Act
            var result = _controller.ListAllStore(allStoreParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ListAllStore_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            var allStoreParameters = new AllStoreParameters();

            _storeRepositoryMock.Setup(repo => repo.GetAllStores(allStoreParameters))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = _controller.ListAllStore(allStoreParameters);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Simulated exception", badRequestResult.Value);

            _logger.Verify(logger => logger.LogError($"An error occurred while retrieving all stores: Simulated exception"), Times.Once);
        }
        #endregion

        #region GetTop10Store
        [Fact]
        public async Task GetTop10Store_ReturnsOkResultWithTop10Stores()
        {
            // Arrange
            var fakeTop10Stores = new List<Store> { };

            _storeRepositoryMock.Setup(repo => repo.GetTop10PopularStore())
                .ReturnsAsync(fakeTop10Stores);

            var fakeTop10StoreDTOs = new List<AllStoreDTO> { /* Fake DTO data */ };
            _mapperMock.Setup(mapper => mapper.Map<List<AllStoreDTO>>(fakeTop10Stores))
                .Returns(fakeTop10StoreDTOs);

            // Act
            var result = await _controller.GetTop10Store();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTop10StoreDTOs = Assert.IsType<List<AllStoreDTO>>(okResult.Value);

            Assert.Equal(fakeTop10StoreDTOs, returnedTop10StoreDTOs);

            _logger.Verify(logger => logger.LogInfo("Attempting to retrieve the top 10 popular stores..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo("Retrieved top 10 popular stores successfully."), Times.Once);
        }

        [Fact]
        public async Task GetTop10Store_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            _storeRepositoryMock.Setup(repo => repo.GetTop10PopularStore())
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetTop10Store();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Simulated exception", statusCodeResult.Value);

            _logger.Verify(logger => logger.LogError($"An error occurred while retrieving the top 10 popular stores: Simulated exception"), Times.Once);
        }

        #endregion

        #region GetStoreInformation
        [Fact]
        public async Task GetStoreInformation_ReturnsOkResultWithStoreInformation()
        {
            // Arrange
            var fakeStoreId = 1;
            var fakeStoreInformation = new StoreInforDTO { /* Fake store information data */ };

            _storeRepositoryMock.Setup(repo => repo.GetInformationStore(fakeStoreId))
                .ReturnsAsync(fakeStoreInformation);

            // Act
            var result = await _controller.GetStoreInformation(fakeStoreId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStoreInformation = Assert.IsType<StoreInforDTO>(okResult.Value);

            Assert.Equal(fakeStoreInformation, returnedStoreInformation);

            _logger.Verify(logger => logger.LogInfo($"Attempting to retrieve information for store with ID: {fakeStoreId}"), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Retrieved information for store with ID: {fakeStoreId} successfully."), Times.Once);
        }

        [Fact]
        public async Task GetStoreInformation_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var fakeStoreId = 1;
            _storeRepositoryMock.Setup(repo => repo.GetInformationStore(fakeStoreId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetStoreInformation(fakeStoreId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Simulated exception", statusCodeResult.Value);

            _logger.Verify(logger => logger.LogError($"An error occurred while retrieving store information for ID {fakeStoreId}: Simulated exception"), Times.Once);
        }
        #endregion

        #region GetStoreByUid
        [Fact]
        public async Task GetStoreByUid_ReturnsOkResultWithStore()
        {
            // Arrange
            var fakeUserId = "fakeUserId";
            var fakeStore = new Store { };

            _storeRepositoryMock.Setup(repo => repo.FindSingle(
             It.IsAny<Expression<Func<Store, bool>>>(),
             It.IsAny<Expression<Func<Store, object>>[]>()))
        .ReturnsAsync(fakeStore);

            // Act
            var result = await _controller.GetStoreByUid(fakeUserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetStoreByUid_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var fakeUserId = "fakeUserId";
            _storeRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Store, bool>>>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetStoreByUid(fakeUserId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Simulated exception", statusCodeResult.Value);
        }
        #endregion

        #region GetStore
        [Fact]
        public async Task GetStore_ReturnsOkResultWithData()
        {
            // Arrange
            var userId = "yourUserId";
            var fakeStore = new Store { };
            var fakeLocation = new Location { };

            _storeRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Store, bool>>>()))
                .ReturnsAsync(fakeStore);

            _locationRepositorMocky.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Location, bool>>>()))
                .ReturnsAsync(fakeLocation);

            // Act
            var result = await _controller.GetStore(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);


        }
        [Fact]
        public async Task GetStore_ReturnsStatusCode500WhenExceptionThrown()
        {
            // Arrange
            var userId = "yourUserId";

            _storeRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Store, bool>>>()))
                .ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _controller.GetStore(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion
        #region ExportFood
        [Fact]
        public async Task ExportFood_ReturnsFileResultWithData()
        {
            // Arrange
            var storeId = 1;
            var fakeData = new byte[] { };

            _storeRepositoryMock.Setup(repo => repo.ExportFood(storeId))
                .ReturnsAsync(fakeData);

            // Act
            var result = await _controller.ExportFood(storeId);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
            Assert.True(fileResult.FileContents.SequenceEqual(fakeData));
            Assert.Equal("ThongKe_MonAn_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task ExportFood_ReturnsStatusCode500WhenExceptionThrown()
        {
            // Arrange
            var storeId = 1;

            _storeRepositoryMock.Setup(repo => repo.ExportFood(storeId))
                .ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _controller.ExportFood(storeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region ExportInventory
        [Fact]
        public async Task ExportInventory_ReturnsFileResultWithData()
        {
            // Arrange
            var storeId = 1;
            var fakeData = new byte[] { };

            _storeRepositoryMock.Setup(repo => repo.ExportInventory(storeId))
                .ReturnsAsync(fakeData);

            // Act
            var result = await _controller.ExportInventory(storeId);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
            Assert.True(fileResult.FileContents.SequenceEqual(fakeData));
            Assert.Equal("ThongKe_Kho_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task ExportInventory_ReturnsStatusCode500WhenExceptionThrown()
        {
            // Arrange
            var storeId = 1;

            _storeRepositoryMock.Setup(repo => repo.ExportInventory(storeId))
                .ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _controller.ExportInventory(storeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region UpdateStore
        [Fact]
        public async Task UpdateStore_ReturnsOkResultWithUpdatedStoreInfo()
        {
            // Arrange
            var storeId = 1;
            var storeInfoDTO = new StoreInforDTO { };

            _storeRepositoryMock.Setup(repo => repo.UpdateStore(storeId, storeInfoDTO))
                .ReturnsAsync(storeInfoDTO);

            // Act
            var result = await _controller.UpdateStore(storeId, storeInfoDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedStoreInfo = Assert.IsType<StoreInforDTO>(okResult.Value);
            Assert.Same(storeInfoDTO, updatedStoreInfo);
        }

        [Fact]
        public async Task UpdateStore_ReturnsStatusCode500WhenExceptionThrown()
        {
            // Arrange
            var storeId = 1;
            var storeInfoDTO = new StoreInforDTO { };

            _storeRepositoryMock.Setup(repo => repo.UpdateStore(storeId, storeInfoDTO))
                .ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _controller.UpdateStore(storeId, storeInfoDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region DetailStore
        [Fact]
        public async Task DetailStore_ReturnsOkResultWithDetailedStoreInfo()
        {
            // Arrange
            var storeId = 1;
            var storeInforDTO = new StoreInforDTO { };
            var combos = new List<Combo> { };

            _storeRepositoryMock.Setup(repo => repo.GetDetailStore(storeId))
      .ReturnsAsync(storeInforDTO);

            _comboRepositoryMock.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Combo, object>>>()))
                .ReturnsAsync(combos);

            // Act
            var result = await _controller.DetailStore(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DetailStore_ReturnsStatusCode500WhenExceptionThrown()
        {
            // Arrange
            var storeId = 1;

            _storeRepositoryMock.Setup(repo => repo.GetDetailStore(storeId))
                .ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _controller.DetailStore(storeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion
        #region GetCommentByStore
        [Fact]
        public async Task GetCommentByStore_ReturnsOkResultWithComments()
        {
            // Arrange
            var storeId = 1;
            var rate = 5;
            var comments = new List<Comment> { };

            _storeRepositoryMock.Setup(repo => repo.GetCommentByStore(rate, storeId))
                .ReturnsAsync(comments);

            // Act
            var result = await _controller.GetCommentByStore(rate, storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retrievedComments = Assert.IsType<List<Comment>>(okResult.Value);
            Assert.Equal(comments, retrievedComments);
        }

        [Fact]
        public async Task GetCommentByStore_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var storeId = 1;
            var rate = 5;

            _storeRepositoryMock.Setup(repo => repo.GetCommentByStore(rate, storeId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetCommentByStore(rate, storeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region GetCommentReply
        [Fact]
        public async Task GetCommentReply_ReturnsOkResultWithCommentReplies()
        {
            // Arrange
            var commentId = 1;
            var commentReplies = new List<CommentReply> { };

            _storeRepositoryMock.Setup(repo => repo.GetCommentReply(commentId))
                .ReturnsAsync(commentReplies);

            // Act
            var result = await _controller.GetCommentReply(commentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retrievedCommentReplies = Assert.IsType<List<CommentReply>>(okResult.Value);
            Assert.Equal(commentReplies, retrievedCommentReplies);
        }

        [Fact]
        public async Task GetCommentReply_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var commentId = 1;

            _storeRepositoryMock.Setup(repo => repo.GetCommentReply(commentId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetCommentReply(commentId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _logger.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Once);
        }
        #endregion

        #region GetFoodByCategory
        [Fact]
        public async Task GetFoodByCategory_ReturnsOkResultWithFoodDTOs()
        {
            // Arrange
            var idShop = 1;
            var idCategory = 2;
            var foodDTOs = new List<FoodDTO> { };

            _storeRepositoryMock.Setup(repo => repo.GetFoodByCategory(idShop, idCategory))
                .ReturnsAsync(foodDTOs);

            // Act
            var result = await _controller.GetFoodByCategory(idShop, idCategory);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retrievedFoodDTOs = Assert.IsType<List<FoodDTO>>(okResult.Value);
            Assert.Equal(foodDTOs, retrievedFoodDTOs);
        }

        [Fact]
        public async Task GetFoodByCategory_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var idShop = 1;
            var idCategory = 2;

            _storeRepositoryMock.Setup(repo => repo.GetFoodByCategory(idShop, idCategory))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetFoodByCategory(idShop, idCategory);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion
        #region GetFoodByName
        //[Fact]
        //public void GetFoodByName_WithoutName_ReturnsOkResultWithAllFoodDTOs()
        //{
        //    // Arrange
        //    _foodRepositoryMock.Setup(repo => repo.FindAll()).Returns(new List<Food>().AsQueryable());
        //    _mapperMock.Setup(mapper => mapper.Map<List<FoodDTO>>(It.IsAny<List<Food>>()))
        //        .Returns(new List<FoodDTO>());

        //    // Act
        //    var result = _controller.GetFoodByName(null);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var retrievedFoodDTOs = Assert.IsType<List<FoodDTO>>(okResult.Value);
        //    Assert.Empty(retrievedFoodDTOs);
        //}

        [Fact]
        public void GetFoodByName_WithName_ReturnsOkResultWithMatchingFoodDTOs()
        {
            // Arrange
            var name = "Pizza";
            var foods = new List<Food> { new Food { FoodName = "Pizza Margherita" } };

            _foodRepositoryMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Food, bool>>>()))
                .Returns(foods.AsQueryable());
            _mapperMock.Setup(mapper => mapper.Map<List<FoodDTO>>(It.IsAny<List<Food>>()))
                .Returns(new List<FoodDTO> { new FoodDTO { FoodName = "Pizza Margherita" } });

            // Act
            var result = _controller.GetFoodByName(name);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retrievedFoodDTOs = Assert.IsType<List<FoodDTO>>(okResult.Value);
            Assert.Single(retrievedFoodDTOs);
            Assert.Equal("Pizza Margherita", retrievedFoodDTOs[0].FoodName);
        }

        [Fact]
        public void GetFoodByName_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            _foodRepositoryMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Food, bool>>>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.GetFoodByName("Pizza");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _logger.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Once);
        }
        #endregion

        #region RatingStore
        [Fact]
        public async Task RatingStore_WithoutParentComment_ReturnsOkResult()
        {
            // Arrange
            var storeRatingDTO = new StoreRatingDTO { StoreId = 1, Rate = 5 };
            _mapperMock.Setup(mapper => mapper.Map<Comment>(It.IsAny<StoreRatingDTO>()))
                .Returns(new Comment());

            // Act
            var result = await _controller.RatingStore(storeRatingDTO);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _commentRepositoryMock.Verify(repo => repo.RatingStore(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public async Task RatingStore_WithParentComment_ReturnsOkResultWithCommentReply()
        {
            // Arrange
            var storeRatingDTO = new StoreRatingDTO { StoreId = 1, Rate = 5, ParentCommentId = 1 };
            var comment = new Comment();
            _mapperMock.Setup(mapper => mapper.Map<Comment>(It.IsAny<StoreRatingDTO>()))
                .Returns(comment);
            _storeRepositoryMock.Setup(repo => repo.GetCommentReply(It.IsAny<int>()))
                .ReturnsAsync(new { });

            // Act
            var result = await _controller.RatingStore(storeRatingDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RatingStore_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            var storeRatingDTO = new StoreRatingDTO { StoreId = 1, Rate = 5 };
            _mapperMock.Setup(mapper => mapper.Map<Comment>(It.IsAny<StoreRatingDTO>()))
                .Returns(new Comment());
            _commentRepositoryMock.Setup(repo => repo.RatingStore(It.IsAny<Comment>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.RatingStore(storeRatingDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion

        #region OrderStatistic
        [Fact]
        public void OrderStatistic_ReturnsOkResultWithOrderStatistics()
        {
            // Arrange
            int storeId = 1;
            var fakeOrderStatistics = new List<OrderStatistic> { /* create fake order statistics */ };
            _orderRepositoryMock.Setup(repo => repo.OrderStatistic(It.IsAny<int>()))
                .Returns(fakeOrderStatistics);
            _orderRepositoryMock.Setup(repo => repo.CountTotalOrder(It.IsAny<int>()))
                .Returns(10); // replace with your expected total order count

            // Act
            var result = _controller.OrderStatistic(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void OrderStatistic_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            int storeId = 1;
            _orderRepositoryMock.Setup(repo => repo.OrderStatistic(It.IsAny<int>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.OrderStatistic(storeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _orderRepositoryMock.Verify(repo => repo.OrderStatistic(It.IsAny<int>()), Times.Once);
        }

        #endregion

        #region GetFoodDetailStatistics
        [Fact]
        public void GetFoodDetailStatistics_ReturnsOkResultWithFoodDetailStatistics()
        {
            // Arrange
            int storeId = 1;
            var fakeFoodDetailStatistics = new List<FoodDetailStatistic> { };
            _orderRepositoryMock.Setup(repo => repo.FoodDetailStatistics(It.IsAny<int>()))
                .Returns(fakeFoodDetailStatistics);

            // Act
            var result = _controller.GetFoodDetailStatistics(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var foodDetailStatistics = Assert.IsType<List<FoodDetailStatistic>>(okResult.Value);
            Assert.Equal(fakeFoodDetailStatistics, foodDetailStatistics);
            _orderRepositoryMock.Verify(repo => repo.FoodDetailStatistics(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetFoodDetailStatistics_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            int storeId = 1;
            _orderRepositoryMock.Setup(repo => repo.FoodDetailStatistics(It.IsAny<int>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.GetFoodDetailStatistics(storeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _orderRepositoryMock.Verify(repo => repo.FoodDetailStatistics(It.IsAny<int>()), Times.Once);
        }

        #endregion

        #region GetRevenuePerMonth
        [Fact]
        public void GetRevenuePerMonth_ReturnsOkResultWithRevenuePerMonths()
        {
            // Arrange
            int storeId = 1;
            int year = 2023; // Provide a valid year
            var fakeRevenuePerMonths = new List<RevenuePerMonth> { /* create fake revenue per months */ };
            _orderRepositoryMock.Setup(repo => repo.RevenuePerMonth(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(fakeRevenuePerMonths);

            // Act
            var result = _controller.GetRevenuePerMonth(storeId, year);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var revenuePerMonths = Assert.IsType<List<RevenuePerMonth>>(okResult.Value);
            Assert.Equal(fakeRevenuePerMonths, revenuePerMonths);
            _orderRepositoryMock.Verify(repo => repo.RevenuePerMonth(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetRevenuePerMonth_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            int storeId = 1;
            int year = 2023; // Provide a valid year
            _orderRepositoryMock.Setup(repo => repo.RevenuePerMonth(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.GetRevenuePerMonth(storeId, year);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _orderRepositoryMock.Verify(repo => repo.RevenuePerMonth(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        #endregion

        #region ExportOrder
        [Fact]
        public void ExportOrder_ReturnsFileResultWithData()
        {
            // Arrange
            int storeId = 1;
            var fakeData = new byte[] { /* create fake data */ };
            _orderRepositoryMock.Setup(repo => repo.ExportOrder(It.IsAny<int>()))
                .Returns(fakeData);

            // Act
            var result = _controller.ExportOrder(storeId);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
            _orderRepositoryMock.Verify(repo => repo.ExportOrder(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ExportOrder_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            int storeId = 1;
            _orderRepositoryMock.Setup(repo => repo.ExportOrder(It.IsAny<int>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.ExportOrder(storeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _orderRepositoryMock.Verify(repo => repo.ExportOrder(It.IsAny<int>()), Times.Once);
        }

        #endregion
    }
}

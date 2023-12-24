using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Wishlist;
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
    public class WishListControllerTest
    {
        private readonly Mock<IWishlistRepository> _wlRepo;
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<ILoggerManager> _logger;
        private WishlistController controller;
        public WishListControllerTest()
        {
            _wlRepo = new Mock<IWishlistRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerManager>();
            controller = new WishlistController(_wlRepo.Object, _mapper.Object, _logger.Object);
        }

        #region GetWishlistByUserId
        [Fact]
        public async Task GetWishlistByUserId_ReturnsOkResultWithWishlistDTOs()
        {
            // Arrange
            var userId = "sampleUserId";
            var wishlistItems = new List<Wishlist> { };
            var wishlistDTOs = new List<WishlistDTO> { };

            _wlRepo.Setup(repo => repo.GetListWishlist(userId))
                .ReturnsAsync(wishlistItems);

            _mapper.Setup(mapper => mapper.Map<List<WishlistDTO>>(wishlistItems))
                .Returns(wishlistDTOs);

            // Act
            var result = await controller.GetWishlistByUserId(userId);

            // Assert
            var okResult = Assert.IsType<ActionResult<List<WishlistDTO>>>(result);
        }

        [Fact]
        public async Task GetWishlistByUserId_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var userId = "sampleUserId";

            _wlRepo.Setup(repo => repo.GetListWishlist(userId))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await controller.GetWishlistByUserId(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ActionResult<List<WishlistDTO>>>(result);
        }

        #endregion

        #region AddToWishlist
        [Fact]
        public async Task AddToWishlist_ReturnsOkResultWhenFoodAddedToWishlistSuccessfully()
        {
            // Arrange
            var userId = "sampleUserId";
            var foodId = 123;

            // Act
            var result = await controller.AddToWishlist(userId, foodId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to add food with ID {foodId} to wishlist for user with ID {userId}."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Successfully added food with ID {foodId} to wishlist for user with ID {userId}."), Times.Once);
            _wlRepo.Verify(repo => repo.AddToWishlist(userId, foodId), Times.Once);
        }

        [Fact]
        public async Task AddToWishlist_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var userId = "sampleUserId";
            var foodId = 123;

            _wlRepo.Setup(repo => repo.AddToWishlist(userId, foodId))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await controller.AddToWishlist(userId, foodId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);

            _logger.Verify(logger => logger.LogError($"An error occurred while adding food with ID {foodId} to wishlist for user with ID {userId}: Simulated exception"), Times.Once);
        }
        #endregion

        #region IsInWishlist
        [Fact]
        public async Task IsInWishlist_ReturnsOkResultWithTrueWhenFoodIsInWishlist()
        {
            // Arrange
            var userId = "sampleUserId";
            var foodId = 123;

            _wlRepo.Setup(repo => repo.IsInWishlist(userId, foodId))
                .ReturnsAsync(true);

            // Act
            var result = await controller.IsInWishlist(userId, foodId);

            // Assert
            var okResult = Assert.IsType<ActionResult<bool>>(result);
        }

        [Fact]
        public async Task IsInWishlist_ReturnsOkResultWithFalseWhenFoodIsNotInWishlist()
        {
            // Arrange
            var userId = "sampleUserId";
            var foodId = 123;

            _wlRepo.Setup(repo => repo.IsInWishlist(userId, foodId))
                .ReturnsAsync(false);

            // Act
            var result = await controller.IsInWishlist(userId, foodId);

            // Assert
            var okResult = Assert.IsType<ActionResult<bool>>(result);

        }

        [Fact]
        public async Task IsInWishlist_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var userId = "sampleUserId";
            var foodId = 123;

            _wlRepo.Setup(repo => repo.IsInWishlist(userId, foodId))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await controller.IsInWishlist(userId, foodId);

            // Assert
            var statusCodeResult = Assert.IsType<ActionResult<bool>>(result);

            _logger.Verify(logger => logger.LogError($"An error occurred while checking if food with ID {foodId} is in wishlist for user with ID {userId}: Simulated exception"), Times.Once);
        }
        #endregion

        #region RemoveFromWishlist
        [Fact]
        public async Task RemoveFromWishlist_ReturnsOkResultWhenWishlistItemIsRemoved()
        {
            // Arrange
            var wishlistId = 123;

            _wlRepo.Setup(repo => repo.RemoveFromWishlist(wishlistId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.RemoveFromWishlist(wishlistId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Wishlist item removed successfully.", okResult.Value);

            _logger.Verify(logger => logger.LogInfo($"Attempting to remove wishlist item with ID {wishlistId}."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Wishlist item with ID {wishlistId} removed successfully."), Times.Once);
        }

        [Fact]
        public async Task RemoveFromWishlist_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var wishlistId = 123;

            _wlRepo.Setup(repo => repo.RemoveFromWishlist(wishlistId))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await controller.RemoveFromWishlist(wishlistId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);

            _logger.Verify(logger => logger.LogError($"An error occurred while removing wishlist item with ID {wishlistId}: Simulated exception"), Times.Once);
        }
        #endregion
        #region RemoveFromWishlistv2
        [Fact]
        public async Task RemoveFromWishlistv2_ReturnsOkResultWhenWishlistItemIsRemoved()
        {
            // Arrange
            var userId = "testUser";
            var foodId = 123;

            _wlRepo.Setup(repo => repo.RemoveFromWishlist2(userId, foodId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.RemoveFromWishlistv2(userId, foodId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Wishlist item removed successfully.", okResult.Value);

            _logger.Verify(logger => logger.LogInfo($"Attempting to remove wishlist item for user {userId} and food {foodId}."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Wishlist item for user {userId} and food {foodId} removed successfully."), Times.Once);
        }

        [Fact]
        public async Task RemoveFromWishlistv2_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var userId = "testUser";
            var foodId = 123;

            _wlRepo.Setup(repo => repo.RemoveFromWishlist2(userId, foodId))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await controller.RemoveFromWishlistv2(userId, foodId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);

            _logger.Verify(logger => logger.LogError($"An error occurred while removing wishlist item for user {userId} and food {foodId}: Simulated exception"), Times.Once);
        }
        #endregion
    }
}

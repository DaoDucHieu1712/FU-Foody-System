using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Comment;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
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
    public class CommentControllerTest
    {
        private Mock<ICommentRepository> _commentRepoMock;
        private Mock<IStoreRepository> _storeRepoMock;
        private Mock<IMapper> _mapperMock;
        private CommentController _commentController;
        private readonly Mock<ILoggerManager> _logger;
        public CommentControllerTest()
        {
            _commentRepoMock = new Mock<ICommentRepository>();
            _storeRepoMock = new Mock<IStoreRepository>();
            _mapperMock = new Mock<IMapper>();
            _logger = new Mock<ILoggerManager>();
            _commentController = new CommentController(_commentRepoMock.Object, _mapperMock.Object, _storeRepoMock.Object, _logger.Object);
        }

        #region GetAllCommentByStore
        [Fact]
        public async Task GetAllCommentByStore_StoreFound_ReturnsOkResult()
        {
            // Arrange
            int idStore = 1;
            var store = new Store { Id = idStore };
            var comments = new List<Comment> { new Comment { Id = 1, Content = "Hay"} };

            _storeRepoMock.Setup(repo => repo.FindById(idStore, null)).ReturnsAsync(store);
            _commentRepoMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(comments.AsQueryable());

            // Act
            var result = await _commentController.GetAllCommentByStore(idStore) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(comments, result.Value);
        }

        [Fact]
        public async Task GetAllCommentByStore_StoreNotFound_ReturnsBadRequest()
        {
            // Arrange
            int idStore = 1;
            _storeRepoMock.Setup(repo => repo.FindById(idStore, null)).ReturnsAsync((Store)null);

            // Act
            var result = await _commentController.GetAllCommentByStore(idStore) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Cửa hàng không tồn tại", result.Value);
        }

        [Fact]
        public async Task GetAllCommentByStore_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int idStore = 1;
            var store = new Store { Id = idStore };

            _storeRepoMock.Setup(repo => repo.FindById(idStore, null)).ReturnsAsync(store);
            _commentRepoMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Comment, bool>>>())).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _commentController.GetAllCommentByStore(idStore) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Cửa hàng không tồn tại", result.Value); // or adjust based on your exception handling
        }
        #endregion
        #region GetAllCommentByShipperId
        [Fact]
        public void GetAllCommentByShipperId_NullShipperId_ReturnsBadRequest()
        {
            // Arrange
            string idShipper = null;

            // Act
            var result = _commentController.GetAllCommentByShipperId(idShipper) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Shipper không tồn tại!", result.Value);
        }

        [Fact]
        public void GetAllCommentByShipperId_ValidShipperId_ReturnsOkResult()
        {
            // Arrange
            string idShipper = "sampleShipperId";
            var comments = new List<Comment> { new Comment { Id = 1, ShipperId = "sampleShipperId" } };

            _commentRepoMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments.AsQueryable());

            // Act
            var result = _commentController.GetAllCommentByShipperId(idShipper) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(_mapperMock.Object.Map<List<CommentDTO>>(comments), result.Value);
        }

        [Fact]
        public void GetAllCommentByShipperId_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            string idShipper = "sampleShipperId";
            _commentRepoMock.Setup(repo => repo.FindAll(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Expression<Func<Comment, object>>[]>()))
                .Throws(new Exception("Simulated exception"));
            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => _commentController.GetAllCommentByShipperId(idShipper));
            Assert.Equal("Simulated exception", ex.Message);
        }
        #endregion
    }
}

using System.Linq.Expressions;

using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Order;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using Test.Mocks;

namespace Test
{
    public class CommentControllerTest
    {
        private readonly ICommentRepository commentRepository;
        private readonly IStoreRepository storeRepository;

        private Mock<IMapper> mockMapper;
        private Mock<ApplicationDbContext> mockDbContext;
        private CommentController controller;
        public CommentControllerTest()
        {
            var mockDbComment = new Mock<DbSet<Comment>>();
           

            mockDbContext = new Mock<ApplicationDbContext>();
            mockDbContext.Setup(c => c.Comments).Returns(mockDbComment.Object);

            var iRepository = new Mock<IRepository<Comment, int>>();
            var entityRepository = new Mock<EntityRepository<Comment, int>>(mockDbContext);


            mockMapper = new Mock<IMapper>();
            commentRepository = new CommentRepository(mockDbContext.Object);
            storeRepository = new StoreRepository(mockDbContext.Object, mockMapper.Object);
            controller = new CommentController(commentRepository, mockMapper.Object, storeRepository);
        }

        [Fact]
        public async void GetCommentByStore_ReturnError_WhenIdEqual0()
        {

            // Act
            IActionResult result = await controller.GetAllCommentByStore(0);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Mật khẩu không đúng !", badRequestResult.Value);
        }

        [Fact]
        public  void GetAllCommentByShipperId_ReturnListEmpty_WhenIdShipperIsNull()
        {

            // Act
            IActionResult result = controller.GetAllCommentByShipperId(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Shipper không tồn tại!", badRequestResult.Value);
        }

        [Fact]
        public void GetAllCommentByShipperId_ReturnListEmpty_WhenIdShipperIsNotNull()
        {
            var mockComment = MockICommentRepository.GetMock();
            var mockStore = MockIStoreRepository.GetMock();

            var controller = new CommentController(mockComment.Object, mockMapper.Object, mockStore.Object);
            string id = "fe73e17c-edcc-44e0-b52a-1b9d298a0d25";
            var res = controller.GetAllCommentByShipperId(null) as ObjectResult;
            Assert.NotNull(res);
            Assert.Equal(StatusCodes.Status200OK, res.StatusCode);
        }

        [Fact]
        public async void GetAllCommentByStore_Return_WhenCommentIsNotNull()
        {
            var mockComment = MockICommentRepository.GetMock();
            var mockStore = MockIStoreRepository.GetMock();

            var controller = new CommentController(mockComment.Object, mockMapper.Object, mockStore.Object);

            var res = await controller.GetAllCommentByStore(1) as ObjectResult;
            Assert.NotNull(res);
            Assert.Equal(StatusCodes.Status200OK, res.StatusCode);
        }


    }
}

using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
            mockDbContext = new Mock<ApplicationDbContext>();
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
    }
}

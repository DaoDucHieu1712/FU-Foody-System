using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Comment;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;
using System.Linq.Expressions;
using Test.Mocks;

namespace Test
{
    public class CommentControllerTest
    {
        private readonly Mock<ICommentRepository> commentRepositoryMock;
        private readonly Mock<IStoreRepository> storeRepositoryMock;

        private Mock<IMapper> mockMapper;
        private CommentController controller;
        public CommentControllerTest()
        {
            mockMapper = new Mock<IMapper>();
            commentRepositoryMock = new Mock<ICommentRepository>();
            storeRepositoryMock = new Mock<IStoreRepository>();

            controller = new CommentController(commentRepositoryMock.Object, mockMapper.Object, storeRepositoryMock.Object);
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
        public void GetAllCommentByShipperId_ReturnListEmpty_WhenIdShipperIsNull()
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
            #region data test
            var comments = new List<Comment>
                {
                    new Comment
                    {
                        Content = "This is a comment",
                        Rate = 5,
                        UserId = "user1",
                        StoreId = 1,
                        FoodId = 101,
                        ShipperId = "shipper1",
                        NoteForShipper = "Handle with care",
                        PostId = 201,
                        ParentCommentId = null // Assuming it's a top-level comment
                    },
                };

            // Test data for CommentDTO
            var commentDTOs = new List<CommentDTO>
            {
                //new CommentDTO
                //{
                //    UserId = "user1",
                //    Username = "User1Name",
                //    Avatar = "user1_avatar.jpg",
                //    ShipperId = "shipper1",
                //    NoteForShipper = "Handle with care",
                //    CreatedAt = DateTime.UtcNow
                //},
                //new CommentDTO
                //{
                //    UserId = "user2",
                //    Username = "User2Name",
                //    Avatar = "user2_avatar.jpg",
                //    ShipperId = "shipper2",
                //    NoteForShipper = "Quick delivery, please",
                //    CreatedAt = DateTime.UtcNow
                //},
                // Add more instances as needed
            };
            #endregion

            string id = "shipper1";

            _ = commentRepositoryMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Comment, object>>>()))
                .Returns(comments.AsQueryable().Where(x => x.ShipperId == id));

            _ = mockMapper.Setup(mapper => mapper.Map<List<CommentDTO>>(It.IsAny<List<Comment>>()))
                .Returns(commentDTOs);
            var res = controller.GetAllCommentByShipperId(id) as ObjectResult;

            var okResult = Assert.IsType<OkObjectResult>(res);
            var model = Assert.IsAssignableFrom<List<CommentDTO>>(okResult.Value);
            Assert.Equal(comments.Count, model.Count); ;

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

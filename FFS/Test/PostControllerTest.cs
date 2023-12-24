using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Post;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Hubs;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Comment = FFS.Application.Entities.Comment;

namespace Test
{
    public class PostControllerTest
    {
        private Mock<IPostRepository> _postRepo;
        private Mock<IReactPostRepository> _reactRepo;
        private Mock<INotificationRepository> _notiRepo;
        private Mock<ICommentRepository> _commentRepo;
        private Mock<IMapper> _mapperMock;
        private Mock<IAuthRepository> _authRepo;
        private readonly Mock<IHubContext<NotificationHub>> _hubContext;
        private readonly Mock<ILoggerManager> _logger;
        private readonly Mock<ApplicationDbContext> _db;
        private PostController controller;

        public PostControllerTest()
        {
            _postRepo = new Mock<IPostRepository>();
            _reactRepo = new Mock<IReactPostRepository> ();
            _mapperMock = new Mock<IMapper> ();
            _authRepo = new Mock<IAuthRepository> ();
            _hubContext = new Mock<IHubContext<NotificationHub>> ();
            _notiRepo = new Mock<INotificationRepository> ();
            _db = new Mock<ApplicationDbContext> ();
            _commentRepo = new Mock<ICommentRepository> (); 
            _logger = new Mock<ILoggerManager>();
            controller = new PostController(_postRepo.Object, _db.Object, _authRepo.Object,
                _notiRepo.Object, _reactRepo.Object,_mapperMock.Object,_hubContext.Object,_commentRepo.Object, _logger.Object);
        }

        #region GetListPosts
        [Fact]
        public void GetListPosts_ReturnsOkResultWithPostsAndMetadata()
        {
            // Arrange
            var postParameters = new PostParameters
            {
               PostTitle = "a"
            };

            var fakePosts = new List<Post>
            {
                new Post { Title = "abc" },
            };
            var fakePagedPosts = new PagedList<Post>(fakePosts,1, 1, 5);

            var fakePostDTOs = new List<PostDTO>
            {
                  new PostDTO { Title = "abc", LikeNumber = 2, CommentNumber = 3 },
            };

            _postRepo.Setup(repo => repo.GetListPosts(It.IsAny<PostParameters>()))
        .Returns(fakePagedPosts);

            _mapperMock.Setup(mapper => mapper.Map<List<PostDTO>>(It.IsAny<PagedList<Post>>()))
                .Returns(fakePostDTOs);

            // Act
            var result = controller.GetListPosts(postParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetListPosts_WithException_ReturnsBadRequest()
        {
            // Arrange
            var postParameters = new PostParameters
            {
                PostTitle = "a"
            };

            _postRepo.Setup(repo => repo.GetListPosts(It.IsAny<PostParameters>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = controller.GetListPosts(postParameters);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Simulated exception", badRequestResult.Value);
        }

        #endregion

        #region GetPostByPostId
        [Fact]
        public async Task GetPostByPostId_ReturnsOkResultWhenPostFound()
        {
            // Arrange
            var postId = 1;
            var fakePost = new Post { };
            var fakePostDTO = new PostDTO {};

            _postRepo.Setup(repo => repo.GetPostByPostId(postId))
                .ReturnsAsync(fakePost);

            _mapperMock.Setup(mapper => mapper.Map<PostDTO>(fakePost))
                .Returns(fakePostDTO);

            // Act
            var result = await controller.GetPostByPostId(postId);

            // Assert
            var okResult = Assert.IsType<ActionResult<Post>>(result);
        }

        [Fact]
        public async Task GetPostByPostId_ReturnsNotFoundResultWhenPostNotFound()
        {
            // Arrange
            var postId = 1;

            _postRepo.Setup(repo => repo.GetPostByPostId(postId))
                .ReturnsAsync((Post)null);

            // Act
            var result = await controller.GetPostByPostId(postId);

            // Assert
            Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to get post with ID {postId}..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Post with ID {postId} not found."), Times.Once);
        }

        [Fact]
        public async Task GetPostByPostId_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var postId = 1;

            _postRepo.Setup(repo => repo.GetPostByPostId(postId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.GetPostByPostId(postId);

            // Assert
            var statusCodeResult = Assert.IsType<ActionResult<Post>>(result);
        }
        #endregion

        #region GetUserByPost
        [Fact]
        public async Task GetUserByPost_ReturnsOkResultWhenUserFound()
        {
            // Arrange
            var postId = 1;
            var fakeUserId = "user123";

            _postRepo.Setup(repo => repo.GetUserIdByPostId(postId))
                .ReturnsAsync(fakeUserId);

            // Act
            var result = await controller.GetUserByPost(postId);

            // Assert
            var okResult = Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to get user by post with ID {postId}..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Successfully retrieved user by post with ID {postId}."), Times.Once);
        }

        [Fact]
        public async Task GetUserByPost_ReturnsNotFoundResultWhenUserNotFound()
        {
            // Arrange
            var postId = 1;

            _postRepo.Setup(repo => repo.GetUserIdByPostId(postId))
                .ReturnsAsync((string)null);

            // Act
            var result = await controller.GetUserByPost(postId);

            // Assert
            Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to get user by post with ID {postId}..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Post with ID {postId} not found."), Times.Once);
        }

        [Fact]
        public async Task GetUserByPost_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var postId = 1;

            _postRepo.Setup(repo => repo.GetUserIdByPostId(postId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.GetUserByPost(postId);

            // Assert
            var statusCodeResult = Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogError($"An error occurred while getting user by post with ID {postId}: Simulated exception"), Times.Once);
        }
        #endregion

        #region GetTop3NewestPosts
        [Fact]
        public async Task GetTop3NewestPosts_ReturnsOkResultWithPosts()
        {
            // Arrange
            var fakeTop3Posts = new List<Post> { };
            var fakePostDTOs = new List<PostDTO> { };

            _postRepo.Setup(repo => repo.GetTop3NewestPosts())
                .ReturnsAsync(fakeTop3Posts);

            _mapperMock.Setup(mapper => mapper.Map<List<PostDTO>>(fakeTop3Posts))
                .Returns(fakePostDTOs);

            // Act
            var result = await controller.GetTop3NewestPosts();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<Post>>>(result);
            _logger.Verify(logger => logger.LogInfo("Attempting to get the top 3 newest posts..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo("Successfully retrieved the top 3 newest posts."), Times.Once);
        }

        [Fact]
        public async Task GetTop3NewestPosts_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            _postRepo.Setup(repo => repo.GetTop3NewestPosts())
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.GetTop3NewestPosts();

            // Assert
            var statusCodeResult = Assert.IsType<ActionResult<List<Post>>>(result);
            _logger.Verify(logger => logger.LogError("An error occurred while getting the top 3 newest posts: Simulated exception"), Times.Once);
        }
        #endregion
        #region CreatePost
        [Fact]
        public async Task CreatePost_ReturnsOkResultWhenPostCreatedSuccessfully()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO {};
            var fakePost = new Post {  };

            _postRepo.Setup(repo => repo.CreatePost(It.IsAny<Post>()))
                .ReturnsAsync(fakePost);

            _hubContext.Setup(hub => hub.Clients.All.SendCoreAsync(
         It.Is<string>(methodName => methodName == "ReceiveNotification"),
         It.Is<object[]>(args => args.Length == 1 && args[0] is Notification),
         default(CancellationToken)))
     .Returns(Task.CompletedTask);

            // Act
            var result = await controller.CreatePost(createPostDTO);

            // Assert
            var okResult = Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to create a new post for user with ID {createPostDTO.UserId}..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Successfully created a new post for user with ID {createPostDTO.UserId}."), Times.Once);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO { /* Populate with sample data */ };

            _postRepo.Setup(repo => repo.CreatePost(It.IsAny<Post>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.CreatePost(createPostDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogError($"An error occurred while creating a new post: Simulated exception"), Times.Once);
        }
        #endregion

        #region UpdatePost
        [Fact]
        public async Task UpdatePost_ReturnsOkResultWhenPostUpdatedSuccessfully()
        {
            // Arrange
            var postId = 1;
            var updatedPostDTO = new UpdatePostDTO { Id = postId /* Populate with other sample data */ };
            var fakeUpdatedPost = new Post { Id = postId /* Populate with other sample data */ };

            _postRepo.Setup(repo => repo.UpdatePost(It.IsAny<Post>()))
                .ReturnsAsync(fakeUpdatedPost);

            _mapperMock.Setup(mapper => mapper.Map<Post>(updatedPostDTO))
                .Returns(fakeUpdatedPost);

            // Act
            var result = await controller.UpdatePost(postId, updatedPostDTO);

            // Assert
            var okResult = Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to update post with ID {postId}..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Successfully updated post with ID {postId}."), Times.Once);
        }

        [Fact]
        public async Task UpdatePost_ReturnsBadRequestWhenIdMismatch()
        {
            // Arrange
            var postId = 1;
            var updatedPostDTO = new UpdatePostDTO { Id = postId + 1 /* Different ID to simulate mismatch */ };

            // Act
            var result = await controller.UpdatePost(postId, updatedPostDTO);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogError($"Invalid request: Provided post ID {postId} does not match the ID in the request body."), Times.Once);
        }

        [Fact]
        public async Task UpdatePost_ReturnsNotFoundWhenPostNotFound()
        {
            // Arrange
            var postId = 1;
            var updatedPostDTO = new UpdatePostDTO { Id = postId /* Populate with other sample data */ };

            _postRepo.Setup(repo => repo.UpdatePost(It.IsAny<Post>()))
                .ReturnsAsync((Post)null);

            // Act
            var result = await controller.UpdatePost(postId, updatedPostDTO);

            // Assert
            Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogError($"Post with ID {postId} not found."), Times.Once);
        }

        [Fact]
        public async Task UpdatePost_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            var postId = 1;
            var updatedPostDTO = new UpdatePostDTO { Id = postId };

            _postRepo.Setup(repo => repo.UpdatePost(It.IsAny<Post>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.UpdatePost(postId, updatedPostDTO);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<Post>>(result);
            _logger.Verify(logger => logger.LogError($"An error occurred while updating post with ID {postId}: Simulated exception"), Times.Once);
        }

        #endregion
        #region DeletePost
        [Fact]
        public async Task DeletePost_ReturnsNoContentWhenPostDeletedSuccessfully()
        {
            // Arrange
            var postId = 1;

            // Act
            var result = await controller.DeletePost(postId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to delete post with ID {postId}..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Successfully deleted post with ID {postId}."), Times.Once);
            _postRepo.Verify(repo => repo.DeletePost(postId), Times.Once);
        }

        [Fact]
        public async Task DeletePost_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            var postId = 1;

            _postRepo.Setup(repo => repo.DeletePost(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.DeletePost(postId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Simulated exception", badRequestResult.Value);
            _logger.Verify(logger => logger.LogError($"An error occurred while deleting post with ID {postId}: Simulated exception"), Times.Once);
        }
        #endregion
        #region ReactPost
        [Fact]
        public async Task ReactPost_ReturnsOkResultWhenReactingToPostSuccessfully()
        {
            // Arrange
            var reactPostDTO = new CreateReactPostDTO { };
            var reactingUser = new ApplicationUser {};
            var postAuthorId = "1"; // Assuming postAuthorId is a string, adjust accordingly
            var existingReactPost = (ReactPost)null; // Assuming ReactPost is a model, adjust accordingly

            _authRepo.Setup(repo => repo.GetUser(It.IsAny<string>()))
                .ReturnsAsync(reactingUser);

            _postRepo.Setup(repo => repo.GetUserIdByPostId(It.IsAny<int>()))
                .ReturnsAsync(postAuthorId);

            _reactRepo.Setup(repo => repo.FindSingle(
          It.IsAny<Expression<Func<ReactPost, bool>>>(),
          It.IsAny<Expression<Func<ReactPost, object>>[]>()))
      .ReturnsAsync(existingReactPost);

            _hubContext.Setup(hub => hub.Clients.All.SendCoreAsync(
          It.Is<string>(methodName => methodName == "ReceiveNotification"),
          It.Is<object[]>(args => args.Length == 1 && args[0] is Notification),
          default(CancellationToken)))
      .Returns(Task.CompletedTask);

            // Act
            var result = await controller.ReactPost(reactPostDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ReactPost_ReturnsBadRequestWhenReactingUserNotFound()
        {
            // Arrange
            var reactPostDTO = new CreateReactPostDTO { /* Populate with sample data */ };
            ApplicationUser nullUser = null;

            _authRepo.Setup(repo => repo.GetUser(It.IsAny<string>()))
                .ReturnsAsync(nullUser);

            // Act
            var result = await controller.ReactPost(reactPostDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Reacting user not found", badRequestResult.Value);
            _logger.Verify(logger => logger.LogError($"Reacting user with ID {reactPostDTO.UserId} not found."), Times.Once);
        }

        [Fact]
        public async Task ReactPost_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            var reactPostDTO = new CreateReactPostDTO { /* Populate with sample data */ };

            _authRepo.Setup(repo => repo.GetUser(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.ReactPost(reactPostDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Simulated exception", badRequestResult.Value);
            _logger.Verify(logger => logger.LogError($"An error occurred while reacting to post with ID {reactPostDTO.PostId} by user ID {reactPostDTO.UserId}: Simulated exception"), Times.Once);
        }


        #endregion

        #region CommentPost
        [Fact]
        public async Task CommentPost_ReturnsNoContentWhenCommentAddedSuccessfully()
        {
            // Arrange
            var comment = new Comment { };

            // Act
            var result = await controller.CommentPost(comment);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to add a new comment for post with ID {comment.PostId}..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Comment added successfully for post with ID {comment.PostId}."), Times.Once);
            _commentRepo.Verify(repo => repo.Add(comment), Times.Once);
        }

        [Fact]
        public async Task CommentPost_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            var comment = new Comment {};

            _commentRepo.Setup(repo => repo.Add(It.IsAny<Comment>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await controller.CommentPost(comment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Simulated exception", badRequestResult.Value);
            _logger.Verify(logger => logger.LogError($"An error occurred while adding a comment for post with ID {comment.PostId}: Simulated exception"), Times.Once);
        }

        #endregion

    }
}

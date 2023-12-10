using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using FFS.Application.Controllers;
using FFS.Application.DTOs.Chat;
using FFS.Application.Entities;
using FFS.Application.Hubs;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Moq;

namespace Test {
    public class ChatControllerTest {
        private readonly Mock<IChatRepository> mockChatRepository;
        private readonly Mock<IMessageRepository> mockMessageRepository;
        private readonly Mock<IHubContext<ChatHub>> mockHubChatContext;
        private readonly Mock<IMapper> mockMapper;

        private readonly ChatController controller;

        public ChatControllerTest()
        {
            // Initialize Mocks
            mockChatRepository = new Mock<IChatRepository>();
            mockMessageRepository = new Mock<IMessageRepository>();
            mockHubChatContext = new Mock<IHubContext<ChatHub>>();
            mockMapper = new Mock<IMapper>();

            // Initialize Controller with Mocks
            controller = new ChatController(
                mockChatRepository.Object,
                mockMessageRepository.Object,
                mockHubChatContext.Object,
                mockMapper.Object);
        }



        #region Get All by user id
        [Fact]
        public async Task GetAllByUserId_WithValidUserId_ReturnsOkResult()
        {

            // Act
            var result = await controller.GetAllByUserId("51ff7a60-bc4f-4734-be39-65bb4700055a");

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.IsType<String>(okResult.Value);
        }

        [Fact]
        public async Task GetAllByUserId_WithInvalidUserId_ReturnsNotFound()
        {
            // Arrange
        

            // Act
            var result = await controller.GetAllByUserId("invalidUserId");

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetAllByUserId_NullUserId_ReturnsBadRequest()
        {
            // Arrange
          

            // Act
            var result = await controller.GetAllByUserId(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region create chat
        [Fact]
        public async Task CreateChatBox_ChatBoxAlreadyExists_ReturnsStatusCode500()
        {
            // Arrange
            var chatRequestDTO = new ChatRequestDTO { /* add required properties for existing chat box */ };
            var existingChatBox = new Chat { /* add mock data for existing chat box */ };

            // Set up a mock hub context for testing
            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(Mock.Of<IClientProxy>());
            mockHubChatContext.Setup(context => context.Clients).Returns(mockClients.Object);

            mockChatRepository.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Chat, bool>>>()))
                .ReturnsAsync(existingChatBox);

            // Act
            var result = await controller.CreateChatBox(chatRequestDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            // Add more assertions as needed
        }

        [Fact]
        public async Task CreateChatBox_ChatBoxDoesNotExist_ReturnsOkResult()
        {
            // Arrange
            var chatRequestDTO = new ChatRequestDTO { /* add required properties for non-existing chat box */ };

            // Set up a mock hub context for testing
            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(Mock.Of<IClientProxy>());
            mockHubChatContext.Setup(context => context.Clients).Returns(mockClients.Object);

            mockChatRepository.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Chat, bool>>>()))
                .ReturnsAsync((Chat)null);

            // Act
            var result = await controller.CreateChatBox(chatRequestDTO);

            // Assert
            Assert.IsType<OkResult>(result);
            // Add more assertions as needed
        }

        #endregion
    }
}

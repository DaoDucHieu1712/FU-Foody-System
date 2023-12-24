using DocumentFormat.OpenXml.Spreadsheet;
using FFS.Application.Controllers;
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
    public class NotificationControllerTest
    {
        [Fact]
        public async Task GetNotificationsByUserId_ReturnsOkResultWithNotifications()
        {
            // Arrange
            var userId = "testUserId";
            var mockLogger = new Mock<ILoggerManager>();
            var mockNotificationRepository = new Mock<INotificationRepository>();

            var controller = new NotificationController(mockNotificationRepository.Object, mockLogger.Object);

            var notifications = new List<Notification>
            {
                new Notification{ UserId ="testUserId", Title = "Title1"}
            };

            mockNotificationRepository.Setup(repo => repo.GetNotificationsByUserId(It.IsAny<string>()))
                .ReturnsAsync(notifications);

            // Act
            var result = await controller.GetNotificationsByUserId(userId);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<Notification>>>(result);
        }

        [Fact]
        public async Task GetNotificationsByUserId_ReturnsOkResultWithEmptyList()
        {
            // Arrange
            var userId = "testUserId";
            var mockLogger = new Mock<ILoggerManager>();
            var mockNotificationRepository = new Mock<INotificationRepository>();

            var controller = new NotificationController(mockNotificationRepository.Object, mockLogger.Object);

            mockNotificationRepository.Setup(repo => repo.GetNotificationsByUserId(It.IsAny<string>()))
                .ReturnsAsync(new List<Notification>());

            // Act
            var result = await controller.GetNotificationsByUserId(userId);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<Notification>>>(result);
        }

    }
}

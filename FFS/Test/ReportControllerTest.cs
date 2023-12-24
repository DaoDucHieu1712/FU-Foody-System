using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ReportControllerTests
    {
        private Mock<IReportRepository> _reportRepo;
        private Mock<IMapper> _mapperMock;
        private Mock<ILoggerManager> _logger;
        private ReportController _controller;

        public ReportControllerTests()
        {
            _reportRepo = new Mock<IReportRepository>();
            _mapperMock = new Mock<IMapper>();
            _logger = new Mock<ILoggerManager>();
            _controller = new ReportController(_reportRepo.Object, _mapperMock.Object, _logger.Object);
        }
        [Fact]
        public async Task Report_ReturnsOkResultWhenReportCreatedSuccessfully()
        {
            // Arrange
            var reportDTO = new ReportDTO {};
            var mappedReport = new Report {};

            _mapperMock.Setup(mapper => mapper.Map<Report>(It.IsAny<ReportDTO>()))
                .Returns(mappedReport);

            // Act
            var result = await _controller.Report(reportDTO);

            // Assert
            Assert.IsType<OkResult>(result);
            _logger.Verify(logger => logger.LogInfo($"Attempting to create a new report..."), Times.Once);
            _logger.Verify(logger => logger.LogInfo($"Report created successfully."), Times.Once);
            _reportRepo.Verify(repo => repo.CreateReport(mappedReport), Times.Once);
        }

        [Fact]
        public async Task Report_ReturnsBadRequestWhenExceptionOccurs()
        {
            // Arrange
            var reportDTO = new ReportDTO { /* Populate with sample data */ };

            _mapperMock.Setup(mapper => mapper.Map<Report>(It.IsAny<ReportDTO>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = await _controller.Report(reportDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Simulated exception", badRequestResult.Value);
            _logger.Verify(logger => logger.LogError($"An error occurred while creating a report: Simulated exception"), Times.Once);
        }
    }

}

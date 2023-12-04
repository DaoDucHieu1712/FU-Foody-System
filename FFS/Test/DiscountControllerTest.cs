using Xunit;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using System.Linq.Expressions;
using FFS.Application.Repositories;
using FFS.Application.Entities.Enum;
using System.Threading.Tasks;
using System;

public class DiscountControllerTests
{
    private Mock<IDiscountRepository> _discountRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IUserDiscountRepository> _userDiscountMock;
    private DiscountController _discountController;

    public DiscountControllerTests()
    {
        _discountRepositoryMock = new Mock<IDiscountRepository>();
        _mapperMock = new Mock<IMapper>();
        _userDiscountMock = new Mock<IUserDiscountRepository>();
        _discountController = new DiscountController(_discountRepositoryMock.Object, _userDiscountMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void ListDiscountByStore_ReturnsOkResultWithDiscounts()
    {
        // Arrange
        var discountParameters = new DiscountParameters
        {
            StoreId = 2,
            //CodeName = "example",
            PageNumber = 1,
            PageSize = 10
        };

        var discounts = new List<Discount>
                {
                    new Discount
                    {
                        Id = 1,
                        StoreId = 2,
                        Code = "DISCOUNT1",
                        Description = "Sample discount 1",
                        Percent = 10,
                        ConditionPrice = 100,
                        Rank = Rank.Gold,
                        Quantity = 5,
                        Expired = DateTime.Now.AddDays(7),
                        Store = new Store(),
                        UserDiscounts = new List<UserDiscount>()
                    },
                    new Discount
                    {
                        Id = 2,
                        StoreId = 2,
                        Code = "DISCOUNT2",
                        Description = "Sample discount 2",
                        Percent = 10,
                        ConditionPrice = 100,
                        Rank = Rank.Gold,
                        Quantity = 5,
                        Expired = DateTime.Now.AddDays(7),
                        Store = new Store(),
                        UserDiscounts = new List<UserDiscount>()
                    }
                };

        var discountDtos = new List<DiscountDTO>
                {
                    new DiscountDTO
                    {
                        Id = 1,
                        StoreId = 2,
                        Code = "DISCOUNT1",
                        Description = "Sample discount 1",
                        Percent = 10,
                        ConditionPrice = 100,
                        Rank = Rank.Gold,
                        Quantity = 5,
                        Expired = DateTime.Now.AddDays(7)
                    },
                    new DiscountDTO
                    {
                        Id = 2,
                        StoreId = 2,
                        Code = "DISCOUNT2",
                        Description = "Sample discount 2",
                        Percent = 10,
                        ConditionPrice = 100,
                        Rank = Rank.Gold,
                        Quantity = 5,
                        Expired = DateTime.Now.AddDays(7)
                    }
                };

        _discountRepositoryMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Discount, object>>>()))
    .Returns(discounts.AsQueryable());

        _mapperMock.Setup(mapper => mapper.Map<List<DiscountDTO>>(It.IsAny<PagedList<Discount>>()))
            .Returns(discountDtos);

        // Act
        var result = _discountController.ListDiscoutByStore(discountParameters);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task CreateDiscount_ValidDiscount_ReturnsOkResult()
    {
        // Arrange
        var discountDTO = new DiscountDTO
        {
            Id = 1,
            StoreId = 2,
            Code = "DISCOUNT1",
            Description = "Sample discount 1",
            Percent = 10,
            ConditionPrice = 100,
            Rank = Rank.Gold,
            Quantity = 5,
            Expired = DateTime.Now.AddDays(7)
        };

        var discount = new Discount();
        _mapperMock.Setup(x => x.Map<Discount>(It.IsAny<DiscountDTO>())).Returns(discount);
        _discountRepositoryMock.Setup(x => x.Add(It.IsAny<Discount>())).Returns(Task.CompletedTask);

        // Act
        var result = await _discountController.CreateDiscount(discountDTO);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        _mapperMock.Verify(x => x.Map<Discount>(It.IsAny<DiscountDTO>()), Times.Once);
        _discountRepositoryMock.Verify(x => x.Add(It.IsAny<Discount>()), Times.Once);
    }
    [Fact]
    public async Task CreateDiscount_ExceptionThrown_ReturnsBadRequest()
    {
        // Arrange
        var discountDTO = new DiscountDTO
        {
            Id = 1,
            //StoreId = 2,
            Code = "DISCOUNT1",
            Description = "Sample discount 1",
            Percent = 10,
            ConditionPrice = 100,
            Rank = Rank.Gold,
            Quantity = 5,
            Expired = DateTime.Now.AddDays(7)
        };

        var exceptionMessage = "An error occurred.";
        _mapperMock.Setup(x => x.Map<Discount>(It.IsAny<DiscountDTO>())).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _discountController.CreateDiscount(discountDTO);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal(exceptionMessage, badRequestResult.Value);
    }
    [Fact]
    public async Task UpdateDiscount_ExistingDiscount_ReturnsOkResult()
    {
        // Arrange
        int id = 1;
        var discountDTO = new DiscountDTO
        {
            Code = "DISCOUNT1",
            Description = "Sample discount 1",
            Percent = 10,
            ConditionPrice = 100,
            Rank = Rank.Gold,
            Quantity = 5,
            Expired = DateTime.Now.AddDays(7)
        };

        var discountUpdate = new Discount(); // Create an instance of Discount
        _discountRepositoryMock.Setup(x => x.FindById(id, null)).ReturnsAsync(discountUpdate);
        _discountRepositoryMock.Setup(x => x.Update(discountUpdate)).Returns(Task.CompletedTask);

        // Act
        var result = await _discountController.UpdateDiscount(id, discountDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Sửa thành công", okResult.Value);

        Assert.Equal(id, discountDTO.Id);
        Assert.Equal(discountUpdate.StoreId, discountDTO.StoreId);

        _mapperMock.Verify(x => x.Map(discountDTO, discountUpdate), Times.Once);
        _discountRepositoryMock.Verify(x => x.Update(discountUpdate), Times.Once);
    }
    [Fact]
    public async Task UpdateDiscount_NonExistingDiscount_ReturnsNotFound()
    {
        // Arrange
        int id = 1;
        var discountDTO = new DiscountDTO
        {
            Code = "DISCOUNT1",
            Description = "Sample discount 1",
            Percent = 10,
            ConditionPrice = 100,
            Rank = Rank.Gold,
            Quantity = 5,
            Expired = DateTime.Now.AddDays(7)
        };

        _discountRepositoryMock.Setup(x => x.FindById(id, null)).ReturnsAsync((Discount)null);

        // Act
        var result = await _discountController.UpdateDiscount(id, discountDTO);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);

        _mapperMock.Verify(x => x.Map(It.IsAny<DiscountDTO>(), It.IsAny<Discount>()), Times.Never());
        _discountRepositoryMock.Verify(x => x.Update(It.IsAny<Discount>()), Times.Never());
    }

    [Fact]
    public async Task UpdateDiscount_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        int id = 1;
        var discountDTO = new DiscountDTO
        {
            Percent = 10,
            ConditionPrice = 100,
            Rank = Rank.Gold,
            Quantity = 5,
            Expired = DateTime.Now.AddDays(7)
        };

        var exceptionMessage = "An error occurred.";
        _discountRepositoryMock.Setup(x => x.FindById(id, null)).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _discountController.UpdateDiscount(id, discountDTO);

        // Assert
        var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, internalServerErrorResult.StatusCode);
        Assert.Equal(exceptionMessage, internalServerErrorResult.Value);

        _mapperMock.Verify(x => x.Map(It.IsAny<DiscountDTO>(), It.IsAny<Discount>()), Times.Never());
        _discountRepositoryMock.Verify(x => x.Update(It.IsAny<Discount>()), Times.Never());
    }

    [Fact]
    public async Task DeleteDiscount_ExistingDiscount_ReturnsOkResult()
    {
        // Arrange
        int id = 1;
        var discountDelete = new Discount(); // Create an instance of Discount
        _discountRepositoryMock.Setup(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>())).ReturnsAsync(discountDelete);
        _discountRepositoryMock.Setup(x => x.Remove(discountDelete)).Returns(Task.CompletedTask);

        // Act
        var result = await _discountController.DeleteDiscount(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Xóa thành công", okResult.Value);

        _discountRepositoryMock.Verify(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()), Times.Once);
        _discountRepositoryMock.Verify(x => x.Remove(discountDelete), Times.Once);
    }

    [Fact]
    public async Task DeleteDiscount_NonExistingDiscount_ReturnsNotFound()
    {
        // Arrange
        int id = 1;
        _discountRepositoryMock.Setup(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>())).ReturnsAsync((Discount)null);

        // Act
        var result = await _discountController.DeleteDiscount(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);

        _discountRepositoryMock.Verify(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()), Times.Once);
        _discountRepositoryMock.Verify(x => x.Remove(It.IsAny<Discount>()), Times.Never);
    }

    [Fact]
    public async Task DeleteDiscount_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        int id = 1;
        var exceptionMessage = "An error occurred.";
        _discountRepositoryMock.Setup(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>())).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _discountController.DeleteDiscount(id);

        // Assert
        var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, internalServerErrorResult.StatusCode);
        Assert.Equal(exceptionMessage, internalServerErrorResult.Value);

        _discountRepositoryMock.Verify(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()), Times.Once);
        _discountRepositoryMock.Verify(x => x.Remove(It.IsAny<Discount>()), Times.Never);
    }


}
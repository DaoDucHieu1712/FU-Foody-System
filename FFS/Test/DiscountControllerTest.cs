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
    #region ListDiscoutByStore

    /**
    * Method: ListDiscoutByStore()
    * Scenario: None
    * Expected behavior: Returns OkObjectResult
    */
    [Fact]
    public void ListDiscountByStore_ReturnsOkResultWithDiscounts()
    {
        // Arrange
        var discountParameters = new DiscountParameters
        {
            StoreId = 2,
            CodeName = "Sample",
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


    /**
    * Method: ListDiscoutByStore()
    * Scenario: Input DiscountParameters: invalid
    * Expected behavior: Returns BadRequestResult
    */
    [Fact]
    public void ListDiscoutByStore_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var discountParameters = new DiscountParameters
        {
            StoreId = -1,  //Invalid
            CodeName = "Sample",
            PageNumber = 1,
            PageSize = 10
        };

        _discountRepositoryMock.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Discount, bool>>>()))
            .Throws(new Exception("Simulated exception"));

        // Act and Assert
        var exception = Assert.Throws<Exception>(() => _discountController.ListDiscoutByStore(discountParameters));
        Assert.Equal("Simulated exception", exception.Message);

        // Ensure that mapping method is not called when an exception occurs
        _mapperMock.Verify(mapper => mapper.Map<List<DiscountDTO>>(It.IsAny<PagedList<Discount>>()), Times.Never);
    }


    #endregion

    #region CreateDiscount
    /**
    * Method: CreateDiscount()
    * Scenario: Input DiscountDTO: valid
    * Expected behavior: Returns OkObjectResult
    */

    [Fact]
    public async Task CreateDiscount_ValidDiscount_ReturnsOkResult()
    {
        // Arrange
        var discountDTO = new DiscountDTO
        {
            Id = 1,
            StoreId = 2,
            //Code = "DISCOUNT1",
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

    /**
  * Method: CreateDiscount()
  * Scenario: Input DiscountDTO: code is existed
  * Expected behavior: Returns NotFoundResult
  */
    [Fact]
    public async Task CreateDiscount_DiscountNameExist_ReturnsBadRequest()
    {
        // Arrange
        int existingDiscountId = 1; // Assuming this ID exists in the repository
        var existingDiscount = new Discount
        {
            Id = existingDiscountId,
            Code = "EXISTINGCODE",
            Description = "Sample discount 1",
        };

        var discountDTO = new DiscountDTO
        {
            Id = existingDiscountId,
            Code = "EXISTINGCODE", //code is existed
            Description = "Sample discount 2",
        };

        _discountRepositoryMock.Setup(repo => repo.FindById(existingDiscountId, null))
            .ReturnsAsync(existingDiscount);

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync(existingDiscount);

        // Act
        var result = await _discountController.UpdateDiscount(existingDiscountId, discountDTO);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal($"Mã giảm giá {existingDiscount.Code} đã tồn tại. Vui lòng chọn mã khác!", badRequestResult.Value);
    }
    /**
  * Method: CreateDiscount()
  * Scenario: Input DiscountDTO Code: code is existed
  * Expected behavior: Returns BadRequestResult
  */
    [Fact]
    public async Task CreateDiscount_ExceptionThrown_ReturnsBadRequest()
    {
        // Arrange

        //don't have StoreId data
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
    #endregion

    #region UpdateDiscount
    /**
    * Method: UpdateDiscount()
    * Scenario: Input DiscountDTO: valid
    * Expected behavior: Returns OkResult
    */
    [Fact]
    public async Task UpdateDiscount_ValidUpdate_ReturnsOkResult()
    {
        // Arrange
        int discountId = 1;
        var discountDTO = new DiscountDTO
        {
            Id = discountId,
            StoreId = 2,
            Code = "UPDATEDDISCOUNT",
            Description = "Updated discount",
            Percent = 15,
            ConditionPrice = 150,
            Rank = Rank.Silver,
            Quantity = 8,
            Expired = DateTime.Now.AddDays(14)
        };

        var existingDiscount = new Discount
        {
            Id = discountId,
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
        };

        _discountRepositoryMock.Setup(repo => repo.FindById(discountId, null))
            .ReturnsAsync(existingDiscount);

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync((Discount)null); // No discount with the same code exists

        _discountRepositoryMock.Setup(repo => repo.Update(It.IsAny<Discount>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _discountController.UpdateDiscount(discountId, discountDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    /**
   * Method: UpdateDiscount()
   * Scenario: Input DiscountDTO Id: 999
   * Expected behavior: Returns NotFoundResult
   */
    [Fact]
    public async Task UpdateDiscount_DiscountNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        int nonExistingDiscountId = 999; // Assuming this ID does not exist in the list
        var existingDiscounts = new List<Discount>
    {
        new Discount
        {
            Id = 1,
            StoreId = 2,
            Code = "DISCOUNT1",
            Description = "Discount 1",
            Percent = 10,
            ConditionPrice = 100,
            Rank = Rank.Gold,
            Quantity = 5,
            Expired = DateTime.Now.AddDays(30)
        },
        new Discount
        {
            Id = 2,
            StoreId = 2,
            Code = "DISCOUNT2",
            Description = "Discount 2",
            Percent = 20,
            ConditionPrice = 200,
            Rank = Rank.Silver,
            Quantity = 10,
            Expired = DateTime.Now.AddDays(20)
        }
        // Add more discounts as needed
    };

        var discountDTO = new DiscountDTO
        {
            Id = nonExistingDiscountId,
            StoreId = 2,
            Code = "UPDATEDDISCOUNT",
            Description = "Updated discount",
            Percent = 15,
            ConditionPrice = 150,
            Rank = Rank.Silver,
            Quantity = 8,
            Expired = DateTime.Now.AddDays(14)
        };

        _discountRepositoryMock.Setup(repo => repo.FindById(nonExistingDiscountId, null))
            .ReturnsAsync(existingDiscounts.FirstOrDefault(d => d.Id == nonExistingDiscountId));

        // Act
        var result = await _discountController.UpdateDiscount(nonExistingDiscountId, discountDTO);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    /**
   * Method: UpdateDiscount()
   * Scenario: Input DiscountDTO: code is existed
   * Expected behavior: Returns NotFoundResult
   */
    [Fact]
    public async Task UpdateDiscount_DiscountNameExist_ReturnsBadRequest()
    {
        // Arrange
        int existingDiscountId = 1; // Assuming this ID exists in the repository
        var existingDiscount = new Discount
        {
            Id = existingDiscountId,
            Code = "EXISTINGCODE",
            Description = "Sample discount 1",
        };

        var discountDTO = new DiscountDTO
        {
            Id = existingDiscountId,
            Code = "EXISTINGCODE", //code is existed
            Description = "Sample discount 2",
        };

        _discountRepositoryMock.Setup(repo => repo.FindById(existingDiscountId, null))
            .ReturnsAsync(existingDiscount);

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync(existingDiscount);

        // Act
        var result = await _discountController.UpdateDiscount(existingDiscountId, discountDTO);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal($"Mã giảm giá {existingDiscount.Code} đã tồn tại. Vui lòng chọn mã khác!", badRequestResult.Value);
    }

    /**
       * Method: UpdateDiscount()
       * Scenario: Input discountDTO: invalid
       * Expected behavior: Returns BadRequestResult
       */

    [Fact]
    public async Task UpdateDiscount_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        int id = 1;
        var discountDTO = new DiscountDTO
        {
            Description = null, //update desciption invalid
            Percent = 1,
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
    #endregion
    #region DeleteDiscoun
    /**
        * Method: DeleteDiscount()
        * Scenario: Input id: valid
        * Expected behavior: Returns OkResult
        */
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
    /**
        * Method: DeleteDiscount()
        * Scenario: Input id: 999 (not exist)
        * Expected behavior: Returns NotFoundResult
        */
    [Fact]
    public async Task DeleteDiscount_NonExistingDiscount_ReturnsNotFound()
    {
        // Arrange
        int id = 999;
        _discountRepositoryMock.Setup(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>())).ReturnsAsync((Discount)null);

        // Act
        var result = await _discountController.DeleteDiscount(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);

        _discountRepositoryMock.Verify(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()), Times.Once);
        _discountRepositoryMock.Verify(x => x.Remove(It.IsAny<Discount>()), Times.Never);
    }
    /**
       * Method: DeleteDiscount()
       * Scenario: Error occur in FindSingle()
       * Expected behavior: Returns NotFoundResult
       */
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
    #endregion
    #region CheckDiscount
    [Fact]
    public async Task CheckDiscount_ValidDiscount_ReturnsOk()
    {
        // Arrange
        string discountCode = "VALIDCODE";
        string userId = "USER123";
        decimal totalPrice = 100;
        int[] storeIds = { 1, 2 };

        var validDiscount = new Discount
        {
            Code = discountCode,
            StoreId = 1,
            Quantity = 2,
            ConditionPrice = 50,
            Expired = DateTime.Now.AddDays(30),
            Percent = 20,
        };

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync(validDiscount);



        // Act
        var result = await _discountController.CheckDiscount(discountCode, userId, totalPrice, storeIds);

        // Assert
        _discountRepositoryMock.Verify(repo => repo.Update(validDiscount), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        // Add more assertions based on the expected response JSON structure
    }

    [Fact]
    public async Task CheckDiscount_InvalidDiscountCode_ReturnsStatusCode500()
    {
        // Arrange
        string invalidDiscountCode = "INVALIDCODE";
        string userId = "USER123";
        decimal totalPrice = 100;
        int[] storeIds = { 1, 2 };

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync((Discount)null);

        // Act
        var result = await _discountController.CheckDiscount(invalidDiscountCode, userId, totalPrice, storeIds);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        // Add more assertions based on the expected response JSON structure
    }

    [Fact]
    public async Task CheckDiscount_InvalidStoreId_ReturnsStatusCode500()
    {
        // Arrange
        string discountCode = "VALIDCODE";
        string userId = "USER123";
        decimal totalPrice = 100;
        int[] storeIds = { 3, 4 }; // Invalid store IDs

        var validDiscount = new Discount
        {
            Code = discountCode,
            StoreId = 1,
            Quantity = 2,
            ConditionPrice = 50,
            Expired = DateTime.Now.AddDays(30),
            Percent = 20,
            // Add other properties as needed
        };

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync(validDiscount);

        // Act
        var result = await _discountController.CheckDiscount(discountCode, userId, totalPrice, storeIds);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        // Add more assertions based on the expected response JSON structure
    }

    [Fact]
    public async Task CheckDiscount_ExpiredDiscount_ReturnsStatusCode500()
    {
        // Arrange
        string discountCode = "EXPIREDCODE";
        string userId = "USER123";
        decimal totalPrice = 100;
        int[] storeIds = { 1, 2 };

        var expiredDiscount = new Discount
        {
            Code = discountCode,
            StoreId = 1,
            Quantity = 2,
            ConditionPrice = 50,
            Expired = DateTime.Now.AddDays(-1), // Expired discount
            Percent = 20,
            // Add other properties as needed
        };

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync(expiredDiscount);

        // Act
        var result = await _discountController.CheckDiscount(discountCode, userId, totalPrice, storeIds);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        // Add more assertions based on the expected response JSON structure
    }
    [Fact]
    public async Task CheckDiscount_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        // Arrange
        string discountCode = "EXPIREDCODE";
        string userId = "USER123";
        decimal totalPrice = 100;
        int[] storeIds = { 1, 2 };

        var expiredDiscount = new Discount
        {
            Code = discountCode,
            StoreId = 1,
            Quantity = 2,
            ConditionPrice = 50,
            Expired = DateTime.Now.AddDays(-1), // Expired discount
            Percent = 20,
            // Add other properties as needed
        };
        var exceptionMessage = "An error occurred.";
        _discountRepositoryMock.Setup(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>())).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _discountController.CheckDiscount(discountCode, userId, totalPrice, storeIds);

        // Assert
        var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, internalServerErrorResult.StatusCode);
        Assert.Equal(exceptionMessage, internalServerErrorResult.Value);

        _discountRepositoryMock.Verify(x => x.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()), Times.Once);
        _discountRepositoryMock.Verify(x => x.Remove(It.IsAny<Discount>()), Times.Never);
    }
    #endregion

    #region UseDiscount
    [Fact]
    public async Task UseDiscount_ValidDiscount_ReturnsOk()
    {
        // Arrange
        string discountCode = "VALIDCODE";
        string userId = "USER123";

        var validDiscount = new Discount
        {
            Code = discountCode,
            Quantity = 2,
            // Add other properties as needed
        };

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync(validDiscount);


        // Act
        var result = await _discountController.UseDiscount(discountCode, userId);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UseDiscount_InvalidDiscountCode_ReturnsStatusCode500()
    {
        // Arrange
        string invalidDiscountCode = "INVALIDCODE";
        string userId = "USER123";

        _discountRepositoryMock.Setup(repo => repo.FindSingle(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync((Discount)null);

        // Act
        var result = await _discountController.UseDiscount(invalidDiscountCode, userId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    #endregion
}
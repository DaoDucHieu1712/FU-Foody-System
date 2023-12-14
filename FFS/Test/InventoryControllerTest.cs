using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.Repositories;

using Moq;

namespace Test
{
    public class InventoryControllerTest
    {

        private readonly Mock<IInventoryRepository> mockInventoryRepository;
        private readonly Mock<IMapper> mockMapper;

        private readonly InventoryController controller;

        public InventoryControllerTest()
        {
            mockInventoryRepository = new Mock<IInventoryRepository>();
            mockMapper = new Mock<IMapper>();

            controller = new InventoryController(
                mockInventoryRepository.Object,
                mockMapper.Object);
        }

        #region Get inventories
        //[Fact]
        //public async Task GetInventories_ReturnsOkResultWithData()
        //{
        //    // Arrange
        //    var inventoryParameters = InventoryParameters();
        //    var inventories = PagedList<Inventory>();

        //    mockInventoryRepository.Setup(repo => repo.GetInventories(inventoryParameters))
        //        .Returns(inventories);

        //    var metadata = new
        //    {
        //        inventories.TotalCount,
        //        inventories.PageSize,
        //        inventories.CurrentPage,
        //        inventories.TotalPages,
        //        inventories.HasNext,
        //        inventories.HasPrevious
        //    };

        //    var entityInventory = List<InventoryDTO>();

        //    mockMapper.Setup(mapper => mapper.Map<List<InventoryDTO>>(inventories))
        //        .Returns(entityInventory);

        //    // Act
        //    var result =  controller.GetInventories(inventoryParameters);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var model = Assert.IsAssignableFrom<Dictionary<string, object>>(okResult.Value);
        //    Assert.Equal(entityInventory, model["entityInventory"]);
        //    // Add more assertions as nee"metadata"ed
        //}

        //[Fact]
        //public async Task GetInventories_ReturnsOkResultWithNoData()
        //{
        //    // Arrange
        //    var inventoryParameters = new InventoryParameters { /* set valid parameters */ };
        //    var emptyInventories = new PagedList<Inventory>(new List<Inventory>(), 0, 0, 0);

        //    mockInventoryRepository.Setup(repo => repo.GetInventories(inventoryParameters))
        //        .Returns(emptyInventories);

        //    var metadata = new
        //    {
        //        emptyInventories.TotalCount,
        //        emptyInventories.PageSize,
        //        emptyInventories.CurrentPage,
        //        emptyInventories.TotalPages,
        //        emptyInventories.HasNext,
        //        emptyInventories.HasPrevious
        //    };

        //    var entityInventory = new List<InventoryDTO>();

        //    mockMapper.Setup(mapper => mapper.Map<List<InventoryDTO>>(emptyInventories))
        //        .Returns(entityInventory);

        //    // Act
        //    var result =  controller.GetInventories(inventoryParameters);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var model = Assert.IsAssignableFrom<Dictionary<string, object>>(okResult.Value);
        //    Assert.Empty((List<InventoryDTO>)model["entityInventory"]);
        //    Assert.Equal(metadata, model["metadata"]);
        //    // Add more assertions as needed
        //}

        //[Fact]
        //public async Task GetInventories_InvalidParameters_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var invalidInventoryParameters = new InventoryParameters { /* set invalid parameters */ };

        //    // Act
        //    var result =  controller.GetInventories(invalidInventoryParameters);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.Equal("Your expected error message", badRequestResult.Value);
        //    // Add more assertions as needed
        //}

        //[Fact]
        //public async Task GetInventories_ExceptionThrown_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var inventoryParameters = new InventoryParameters { /* set valid parameters */ };

        //    mockInventoryRepository.Setup(repo => repo.GetInventories(inventoryParameters))
        //        .Throws(new Exception("Your expected exception message"));

        //    // Act
        //    var result =  controller.GetInventories(inventoryParameters);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //     Assert.Equal("Your expected exception message", badRequestResult.Value);
        //    // Add more assertions as needed
        //}

        //[Fact]
        //public async Task GetInventories_ValidParameters_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var inventoryParameters = new InventoryParameters { /* set valid parameters */ };

        //    mockInventoryRepository.Setup(repo => repo.GetInventories(inventoryParameters))
        //        .Returns((PagedList<Inventory>)null); // Simulate unexpected null from the repository

        //    // Act
        //    var result =  controller.GetInventories(inventoryParameters);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.Equal("Unexpected null from the repository", badRequestResult.Value);
        //    // Add more assertions as needed
        //}
        #endregion
    }

}

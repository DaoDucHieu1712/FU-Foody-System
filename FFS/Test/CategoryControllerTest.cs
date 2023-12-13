using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Category;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories.Impls;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Moq;

namespace Test
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<ILoggerManager> _logger;
        private CategoryController controller;
        public CategoryControllerTest()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerManager>();
            controller = new CategoryController(_categoryRepository.Object, _mapper.Object, _logger.Object);
        }

        #region List cate by store
        [Fact]
        public async void ListCategoryByStoreId_ValidData_ReturnsOk()
        {
            // Arrange
            List<Category> categories = new List<Category>();

            var categoryParameters = new CategoryParameters { StoreId = 1, PageNumber = 1, PageSize = 10 };
            var mockCategories = new PagedList<Category>(categories, 1, 1, 10);
            var mockCategoryDTOs = new List<CategoryDTO> { /* mock your category DTOs data here */ };

            _categoryRepository.Setup(repo => repo.GetCategoriesByStoreId(categoryParameters))
                .Returns(mockCategories);
            _mapper.Setup(mapper => mapper.Map<List<CategoryDTO>>(mockCategories))
                .Returns(mockCategoryDTOs);

            // Act
            var result = await controller.ListCategoryByStoreId(categoryParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<dataReturnCate>(okResult.Value);

            var entityCategories = Assert.IsType<List<CategoryDTO>>(responseData.entityCatetory);
            Assert.Equal(mockCategoryDTOs, entityCategories);

           
        }

        [Fact]
        public async void ListCategoryByStoreId_ExceptionOccurred_ReturnsInternalServerError()
        {
            // Arrange

            var categoryParameters = new CategoryParameters { StoreId = 4 };

            _categoryRepository.Setup(repo => repo.GetCategoriesByStoreId(categoryParameters))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await controller.ListCategoryByStoreId(categoryParameters);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region Create category
        [Fact]
        public async Task Create_NullData_ReturnsBadRequest()
        {
            // Arrange
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new CategoryController(mockCategoryRepository.Object, mockMapper.Object, _logger.Object);

            // Act
            var result = await controller.Create(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Create_InvalidData_ReturnsBadRequest()
        {
            // Arrange

            var invalidData = new CategoryRequestDTO(); // Add invalid data as needed

            // Act
            var result = await controller.Create(invalidData);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Create_ConflictDuringSave_ReturnsConflict()
        {
            // Arrange

            var categoryRequestDTO = new CategoryRequestDTO();
            categoryRequestDTO.StoreId = 4;
            categoryRequestDTO.CategoryName = "Chè";
            categoryRequestDTO.Id = 1;


            // Act
            var result = await controller.Create(categoryRequestDTO);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task Create_ValidData_ReturnsNoContent()
        {
            // Arrange

            var categoryRequestDTO = new CategoryRequestDTO();
            categoryRequestDTO.StoreId = 4;
            categoryRequestDTO.CategoryName = "new Cate";

            // Act
            var result = await controller.Create(categoryRequestDTO);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }
        #endregion

        #region Update Category
        [Fact]
        public async Task Update_NullData_ReturnsBadRequest()
        {
            var result = await controller.Update(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Update_InvalidData_ReturnsBadRequest()
        {
            // Arrange

            var invalidData = new CategoryRequestDTO(); // Invalid data with missing required properties

            // Act
            var result = await controller.Update(1, invalidData);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Update_ConflictDuringUpdate_ReturnsConflict()
        {
            // Arrange

            var categoryRequestDTO = new CategoryRequestDTO();
            categoryRequestDTO.CategoryName = "Mới nè";
            categoryRequestDTO.StoreId = 4;


            // Act
            var result = await controller.Update(1, categoryRequestDTO);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task Update_SuccessfulUpdate_ReturnsNoContent()
        {
            // Arrange

            var categoryRequestDTO = new CategoryRequestDTO();
            categoryRequestDTO.StoreId = 4;
            categoryRequestDTO.CategoryName = "new name";

            // Act
            var result = await controller.Update(1, categoryRequestDTO);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }
        #endregion

        #region delete category
        [Fact]
        public async Task Delete_NullData_ReturnsBadRequest()
        {
          

            // Act
            var result = await controller.Delete(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Delete_InvalidData_ReturnsBadRequest()
        {
            // Arrange

            // If you have specific validation logic for invalid data, you can add it here.

            // Act
            var result = await controller.Delete(0); // Assuming an invalid ID

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }


        [Fact]
        public async Task Delete_SuccessfulDeletion_ReturnsNoContent()
        {
            // Act
            var result = await controller.Delete(21);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }
        #endregion

        #region export cate
        [Fact]
        public async Task ExportCategory_IdWithData_ReturnsFileResult()
        {
            // Arrange

            // Mock data to return when ExportCategory is called with a valid id
            var mockData = new byte[] { /* your mock data here */ };
            _categoryRepository.Setup(repo => repo.ExportCategory(It.IsAny<int>()))
                .ReturnsAsync(mockData);

            // Act
            var result = await controller.ExportCategory(1);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
            Assert.NotNull(fileResult.FileContents);
        }

        [Fact]
        public async Task ExportCategory_IdWithoutData_ReturnsNotFound()
        {
            // Arrange

            // Mock data to return when ExportCategory is called with an id that doesn't have data
            _categoryRepository.Setup(repo => repo.ExportCategory(It.IsAny<int>()))
                .ReturnsAsync((byte[])null);

            // Act
            var result = await controller.ExportCategory(111);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ExportCategory_NullId_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = await controller.ExportCategory(0);

            // Assert
            Assert.IsType<FileContentResult>(result);
        }
        #endregion
    }
}

using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories.Impls;
using Moq;

namespace Test
{
    public class CategoryControllerTest
    {
        private readonly ICategoryRepository categoryRepository;
        private Mock<IMapper> mockMapper;
        private Mock<ApplicationDbContext> mockDbContext;
        private CategoryController controller;
        public CategoryControllerTest()
        {
            mockDbContext = new Mock<ApplicationDbContext>();
            mockMapper = new Mock<IMapper>();
            categoryRepository = new CategoryRepository(mockDbContext.Object, mockMapper.Object);
            controller = new CategoryController(categoryRepository, mockMapper.Object);
        }
    }
}

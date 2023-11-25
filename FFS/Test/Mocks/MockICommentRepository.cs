using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

using Moq;

namespace Test.Mocks
{
    public class MockICommentRepository
    {
        public static Mock<ICommentRepository> GetMock()
        {
            var mock = new Mock<ICommentRepository>();

            Comment comment = new Comment()
            {
                Content = "Nice experience overall.",
                Rate = 4,
                UserId = "bbd09bd3-d3c5-4be2-b234-4925c8cddf28",
                StoreId = 7,
                FoodId = 8,
                ShipperId = "fe73e17c-edcc-44e0-b52a-1b9d298a0d25",
                NoteForShipper = null,
                PostId = 9,
                ParentCommentId = null,
            };
            List<Comment> c = new List<Comment>();
            c.Add(comment);
            IQueryable<Comment> comments = c.AsQueryable();

            _ = mock.Setup(m => m.RatingFood(It.IsAny<Comment>()))
                .Callback(() => { return; });

            _ = mock.Setup(m => m.FindAll(x => x.ShipperId == It.IsAny<string>(), null))
                .Returns(() => comments);

            return mock;
        }
    }
}

using DocumentFormat.OpenXml.Bibliography;
using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories.Impls
{
    public class CommentRepository : EntityRepository<Comment, int>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task RatingStore(Comment comment)
        {
            await Add(comment);
            var store = _context.Stores.FirstOrDefault(x => x.Id == comment.StoreId);
            if (store != null)
            {
                store.RatingCount += 1;
                store.TotalRate += comment.Rate ?? 0;
                store.RateAverage = (decimal)Math.Round((double)store.TotalRate / (double)store.RatingCount, 1);
            }
            await _context.SaveChangesAsync();
        }
        public async Task RatingFood(Comment comment)
        {
            await Add(comment);
            var food = _context.Foods.FirstOrDefault(x => x.Id == comment.FoodId);
            if (food != null)
            {
                food.RatingCount += 1;
                food.TotalRate += comment.Rate ?? 0;
                food.RateAverage = (decimal)Math.Round((double)food.TotalRate / (double)food.RatingCount, 1);
            }
            await _context.SaveChangesAsync();
        }
    }
}

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

        public async Task CreateComment(Comment comment)
        {
            await Add(comment);

        }
    }
}

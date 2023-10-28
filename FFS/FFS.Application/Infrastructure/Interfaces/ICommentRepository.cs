using FFS.Application.DTOs.Store;
using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface ICommentRepository : IRepository<Comment, int>
    {
        Task CreateComment(Comment comment);
    }
}

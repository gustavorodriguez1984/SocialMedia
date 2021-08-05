using SocialMediaCore.Entities;
using System;
using System.Threading.Tasks;

namespace SocialMediaCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //En la unidad de trabajo mapeo todos los repositorios que voy a usar de mi aplicacion
        IPostRepository PostRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        void SaveChanges();
        Task SaveCHangesAsync();
    }
}

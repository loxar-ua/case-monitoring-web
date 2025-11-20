using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShkandalData.Models;

namespace ShkandalData.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IClusterRepository ClusterRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Media> MediaRepository { get; }
        IGenericRepository<Article> ArticleRepository { get; }
        Task<int> CommitAsync();
    }
}

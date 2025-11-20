using ShkandalData.Models;
using ShkandalData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalInfrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShkandalDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        private IClusterRepository? _clusterRepository;

        public UnitOfWork(ShkandalDbContext context)
        {
            _context = context;
        }

        public IClusterRepository ClusterRepository
        {
            get
            {
                if(_clusterRepository == null)
                {
                    _clusterRepository = new ClusterRepository(_context);
                }
                return _clusterRepository;
            }
        }

        public IGenericRepository<User> UserRepository => GetRepository<User>();

        public IGenericRepository<Media> MediaRepository => GetRepository<Media>();

        public IGenericRepository<Article> ArticleRepository => GetRepository<Article>();

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }
            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }
    }
}

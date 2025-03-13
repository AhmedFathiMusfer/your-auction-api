using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using your_auction_api.Data.Repository.IRepository;

namespace your_auction_api.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> Entity;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            Entity = _db.Set<T>();
        }

        public async Task CreateAsync(T obj)
        {
            await Entity.AddAsync(obj);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> Query = Entity;
            if (includeProperties != null)
            {
                foreach (var includeproperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(includeproperty);
                }
            }
            if (filter != null)
            {
                Query = Query.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;

                }
                Query = Query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            return await Query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool Tracked = true, string? includeProperties = null)
        {
            IQueryable<T> Query = Entity;
            if (includeProperties != null)
            {
                foreach (var includeproperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(includeproperty);
                }
            }
            if (Tracked != true)
            {
                Query = Query.AsNoTracking();
            }
            if (filter != null)
            {
                Query = Query.Where(filter);
            }

            return await Query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T obj)
        {
            Entity.Remove(obj);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
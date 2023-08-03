using BuenosAiresRealEstate.API.Data;
using BuenosAiresRealEstate.API.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        /*
        this.dbSet = _db.Set<T>();: In the constructor, the DbSet<T> is obtained from the 
        ApplicationDbContext by calling the Set<T>() method on the _db field. 
        The Set<T>() method is a generic method provided by ApplicationDbContext, 
        and it returns a DbSet<T> representing the database table for the entity type T. 
        The obtained DbSet<T> is then assigned to the dbSet field of the repository.
        */

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, 
            string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            // The purpose of creating the IQueryable<T> variable query in the provided code is to
            // build a flexible and composable query for retrieving entities of type T from the
            // underlying database using Entity Framework or any other LINQ provider.
            IQueryable<T> query = dbSet;
            
            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            // apply pagination if pageSize is greater than 0
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }

                // common pagination alogrithm
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
                // if pageSize = 5 and pageNumber = 1, skip 0 records and take the next 5
                // if pageSize = 5 and pageNumber = 2, skip 5 records and take the next 5
            }

            // this is made to include ApartmentComplex that is related to the ApartmentUnit in question
            if (includeProperties != null)
            {
                var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
                foreach (var property in properties)
                {
                    query = query.Include(property);
                }
            }

            // return a list of results
            return await query.ToListAsync();

        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, 
            bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // this is made to include ApartmentComplex that is related to the ApartmentUnit in question
            if (includeProperties != null)
            {
                var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var property in properties)
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}

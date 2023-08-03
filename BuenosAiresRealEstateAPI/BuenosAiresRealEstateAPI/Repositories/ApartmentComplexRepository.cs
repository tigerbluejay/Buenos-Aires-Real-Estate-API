using BuenosAiresRealEstate.API.Data;
using BuenosAiresRealEstate.API.Models.Models;
using BuenosAiresRealEstate.API.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Repositories
{
    public class ApartmentComplexRepository : Repository<ApartmentComplex>, IApartmentComplexRepository
    {
        private readonly ApplicationDbContext _db;
        public ApartmentComplexRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ApartmentComplex> UpdateAsync(ApartmentComplex entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.ApartmentComplexes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

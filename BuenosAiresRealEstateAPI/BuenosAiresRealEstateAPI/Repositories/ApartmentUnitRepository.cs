using BuenosAiresRealEstate.API.Data;
using BuenosAiresRealEstate.API.Models.Models;
using BuenosAiresRealEstate.API.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Repositories
{
    public class ApartmentUnitRepository : Repository<ApartmentUnit>, IApartmentUnitRepository
    {
        private readonly ApplicationDbContext _db;

        public ApartmentUnitRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ApartmentUnit> UpdateAsync(ApartmentUnit entity)
        {
            entity.UpdateDate = DateTime.Now;
            _db.ApartmentUnits.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

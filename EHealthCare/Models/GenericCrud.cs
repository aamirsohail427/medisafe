using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace EHealthCare.Models
{
    public class GenericCrud<TModel> : Base, IGenericRepository<TModel> where TModel : class
    {

        public async Task<TModel> CreateAsync(TModel entity)
        {
            try
            {
                _db.Set<TModel>().Add(entity);
                _db.Configuration.ValidateOnSaveEnabled = false;
                await _db.SaveChangesAsync();
                return entity;


            }
            catch (Exception ex) { throw ex; }

        }

        public async Task<List<TModel>> CreateAsync(List<TModel> entity)
        {
            try
            {
                foreach (var item in entity)
                {
                    _db.Set<TModel>().Add(item);
                    await _db.SaveChangesAsync();
                }


                return entity;
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            TModel entity = await GetSingleAsync(id);
            if (entity != null)
            {
                _db.Set<TModel>().Remove(entity);
                await _db.SaveChangesAsync();

            }
            return true;
        }

        public async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await _db.Set<TModel>().Where(predicate).ToListAsync();
        }

        public async Task<TModel> GetSingleAsync(object keyValues)
        {
            return await _db.Set<TModel>().FindAsync(keyValues);
        }
        public async Task<List<TModel>> GetAllAsync()
        {
            return await _db.Set<TModel>().ToListAsync();
        }
        public async Task<bool> UpdateEntityAsync(TModel entity, int id)
        {
            var oldEntity = await GetSingleAsync(id);
            _db.Entry(oldEntity).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
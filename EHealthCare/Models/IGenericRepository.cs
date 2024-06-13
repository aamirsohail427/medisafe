using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace EHealthCare.Models
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<TModel> CreateAsync(TModel entity);
        Task<List<TModel>> CreateAsync(List<TModel> entity);
        Task<bool> UpdateEntityAsync(TModel entity, int id);
        Task<bool> DeleteAsync(int id);
        Task<TModel> GetSingleAsync(object keyValues);
        Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate);

    }
}
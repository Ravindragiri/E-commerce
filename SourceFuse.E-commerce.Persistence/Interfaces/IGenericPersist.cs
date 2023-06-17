using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Interfaces
{
    public interface IGenericPersist<T> where T : class
    {
        public bool Add(T entity);
        T GetById(object id);
        List<T> GetAll();
        public bool Update(T entity);
        public bool Delete(T entity);
        public bool DeleteById(object id);
        public void DeleteRange(T[] entityArray);
        public Task<bool> SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SourceFuse.E_commerce.Persistence.Context;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence
{
    public class GenericPersist<T> : IGenericPersist<T> where T : class
    {
        private readonly EcommerceContext _context;
        private DbSet<T> table = null;

        public GenericPersist(EcommerceContext context)
        {
            _context = context;
            table = _context.Set<T>();

        }
        public bool Add(T entity)
        {
            _context.Add(entity);
            return true;
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }

        public List<T> GetAll()
        {
            return table.ToList();
        }

        public bool Update(T entity)
        {
            _context.Update(entity);
            return true;
        }

        public bool Delete(T entity)
        {
            _context.Remove(entity);
            return true;
        }

        public bool DeleteById(object id)
        {
            T entity = table.Find(id);
            _context.Remove(entity);
            return true;
        }

        public void DeleteRange(T[] entityArray)
        {
            _context.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

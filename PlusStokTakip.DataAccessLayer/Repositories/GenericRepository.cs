using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PlusStokTakip.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly PlusStokTakipEntities _db = new PlusStokTakipEntities();
        private readonly DbSet<T> _dbSet;

        public GenericRepository()
        {
            _dbSet = _db.Set<T>();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _db.SaveChanges();
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
            _db.SaveChanges();
        }

        public void Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
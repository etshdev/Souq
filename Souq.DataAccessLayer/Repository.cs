using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.DataAccessLayer
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SouqDB _db;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<T> data;
        public Repository(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _db = _uow.SouqDB;
            data = _db.Set<T>();
        }
        public virtual bool Delete(T entity)
        {
            data.Remove(entity);
            return SaveChange();
        }

        public virtual Task<bool> DeleteAsync(T entity)
        {
            data.Remove(entity);
            return SaveChangeAsync();
        }
        public virtual bool DeleteRange(IEnumerable<T> entities)
        {
            data.RemoveRange(entities);
            return SaveChange();
        }
        public virtual void DeleteRangeWithoutSaveChange(IEnumerable<T> entities)
        {
            data.RemoveRange(entities);
        }

        public virtual void DeleteWithoutSaveChange(T entity)
        {
            data.Remove(entity);
        }
        public virtual IQueryable<T> Find(Func<T, bool> predicate) => data.Where(predicate).AsQueryable<T>();
        public virtual IQueryable<T> GetAll() => data.AsQueryable();
        public virtual IQueryable<T> GetAllAsNoTracking() => data.AsNoTracking().AsQueryable();
        public virtual async Task<IQueryable<T>> GetAllAsync() => await Task.FromResult(data.AsQueryable());
        public virtual T GetById(object Id) => data.Find(Id);
        public virtual bool Insert(T entity)
        {
            data.Add(entity);
            return SaveChange();
        }

        public virtual Task<bool> InsertAsync(T entity)
        {
            data.AddAsync(entity);
            return SaveChangeAsync();
        }
        public virtual bool InsertRange(IEnumerable<T> entities)
        {
            data.AddRange(entities);
            return SaveChange();
        }
        public virtual void InsertRangeWithoutSaveChange(IEnumerable<T> entities) => data.AddRange(entities);
        public virtual void InsertWithoutSaveChange(T entity) => data.Add(entity);
        public virtual bool SaveChange() => _uow.SaveChanges();
        public virtual async Task<bool> SaveChangeAsync() => await _uow.SaveChangesAsync();
        public virtual bool Update(T entity)
        {
            data.Update(entity).State = EntityState.Modified;
            return SaveChange();
        }
        public virtual Task<bool> UpdateAsync(T entity)
        {
            data.Update(entity).State = EntityState.Modified;
            return SaveChangeAsync();
        }
        public virtual void UpdateWithoutSaveChange(T entity) => data.Update(entity).State = EntityState.Modified;


    }
}

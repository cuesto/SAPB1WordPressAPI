using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using SAPB1WordPressAPI.DataModel.Bases;
using SAPB1WordPressAPI.DataModel.Enums;
using SAPB1WordPressAPI.DataModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;



namespace SAPB1WordPressAPI.DataModel.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _dbContext;
        private DbSet<T> _dbSet => _dbContext.Set<T>();
        public IQueryable<T> Entities => _dbSet;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get
        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            // Solo registros Reales
            return query.Where(c => c.IsDeleted == IsDeleted.False).AsEnumerable();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            var data = _dbContext
                    .Set<T>()
                    .Where(x => x.IsDeleted != IsDeleted.False);

            if (predicate != null)
                data = data.Where(predicate);

            return data;
        }

        public virtual T GetById(object key)
        {
            return _dbSet.Find(key);
        }

        public int GetCount(Expression<Func<T, bool>> predicate)
        {
            return Get(predicate).Count();
        }

        #endregion

        #region Insert
        public virtual ValidationResult Insert(T entity, AbstractValidator<T> validator)
        {
            ValidationResult results = validator.Validate(entity);
            bool validationSucceeded = results.IsValid;
            //
            if (validationSucceeded)
            {
                entity.SetValues();
                _dbSet.Add(entity);
            }
            return results;
        }

        public virtual void Insert(T entity)
        {
            entity.SetValues();
            _dbSet.Add(entity);
        }

        public virtual void Insert(IEnumerable<T> entities)
        {
            var enumerable = entities as List<T> ?? entities.ToList();
            enumerable.ForEach(
                entity =>
                {
                    entity.SetValues();
                });
            _dbSet.AddRange(enumerable);
        }

        //public virtual ValidationResult Insert(IEnumerable<T> entities,  AbstractValidator<T> validator)
        //{
        //    var enumerable = entities as List<T> ?? entities.ToList();
        //    bool validationSucceeded = false;
        //    ValidationResult results;
        //    enumerable.ForEach(
        //       x =>
        //       {
        //           results = validator.Validate(x);
        //           validationSucceeded = results.IsValid;
        //           if (!validationSucceeded)
        //           {
        //               return results;
        //           }

        //       });




        //        enumerable.ForEach(
        //        entity =>
        //        {
        //            entity.SetValues();
        //        });
        //    _dbSet.AddRange(enumerable);
        //}

        #endregion

        #region Update

        public virtual void Update(T entityToUpdate)
        {
            entityToUpdate.SetValues();
            _dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual ValidationResult Update(T entityToUpdate, AbstractValidator<T> validator)
        {
            ValidationResult results = validator.Validate(entityToUpdate);
            bool validationSucceeded = results.IsValid;
            //
            if (validationSucceeded)
            {
                entityToUpdate.SetValues();
                _dbSet.Attach(entityToUpdate);
                _dbContext.Entry(entityToUpdate).State = EntityState.Modified;

            }
            return results;
        }

        #endregion

        #region Delete

        public virtual void Delete(object key)
        {
            T entityToDelete = _dbSet.Find(key);

            entityToDelete.IsDeleted = IsDeleted.True;
            Update(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            entityToDelete.IsDeleted = IsDeleted.True;
            Update(entityToDelete);
        }

        public virtual void Gone(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void Gone(T entities)
        {
            _dbSet.Remove(entities);
        }

        #endregion

        #region implemented


        public Task<T> GetByIDAsync(object key)
        {
            throw new NotImplementedException();
        }

        public void Audit(DbContext context, T entity, DbOperationType OpType)
        {
            throw new NotImplementedException();
        }

        public List<T> PGetProcedure(string procedureName, params (string name, object value)[] Parameters)
        {
            throw new NotImplementedException();
        }

        public List<T> PGetProcedure(string procedureName, out int procResult, params (string name, object value)[] Parameters)
        {
            throw new NotImplementedException();
        }

        public void PExecuteProcedure(string procedureName, params (string name, object value)[] Parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

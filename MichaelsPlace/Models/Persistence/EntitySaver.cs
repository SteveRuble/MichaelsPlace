using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace.Models.Persistence
{
    /// <summary>
    /// Abstraction for a class providing the service of persisting a single entity.
    /// Should be used as a dependency by components which simply need to stick things in 
    /// the database.
    /// </summary>
    public interface ISingleEntityService
    {
        /// <summary>
        /// Loads the <typeparamref name="TEntity"/> with <paramref name="id"/> from the database.
        /// This method should be thread safe. The entity will not be proxied and lazy loading will be disabled.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        TEntity Load<TEntity>(object id) where TEntity : class; 
        
        /// <summary>
        /// Saves the <paramref name="entity"/> to the database.
        /// This method should be thread safe.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Save<TEntity>(TEntity entity) where TEntity : class;
    }


    /// <summary>
    /// Provides the service of persisting a single entity.
    /// Should be used as a dependency by components which simply need to stick things in 
    /// the database.
    /// </summary>
    public class SingleEntityService : ISingleEntityService
    {
        private readonly IApplicationDbContextFactory _contextFactory;
        public SingleEntityService(IApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Loads the <typeparamref name="TEntity"/> with <paramref name="id"/> from the database.
        /// This method is thread safe.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public TEntity Load<TEntity>(object id) where TEntity : class
        {
            using (var dbContext = _contextFactory.Create())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                var entity = dbContext.Set<TEntity>().Find(id);
                dbContext.Entry(entity).State = EntityState.Detached;
                return entity;
            }
        }

        /// <summary>
        /// Saves the <paramref name="entity"/> to the database.
        /// This method is thread safe.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Save<TEntity>(TEntity entity) where TEntity : class
        {
            using (var dbContext = _contextFactory.Create())
            {
                var set = dbContext.Set<TEntity>();
                set.Add(entity);
                dbContext.SaveChanges();
            }
        }
    }
}

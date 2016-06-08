using System;
using System.Collections.Generic;
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
    public interface IEntitySaver
    {
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
    public class EntitySaver : IEntitySaver
    {
        private readonly IApplicationDbContextFactory _contextFactory;
        public EntitySaver(IApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
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

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
        void Save<TEntity>(TEntity entity) where TEntity : class;
    }

    public class EntitySaver : IEntitySaver
    {
        public void Save<TEntity>(TEntity entity) where TEntity : class
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var set = dbContext.Set<TEntity>();
                set.Add(entity);
                dbContext.SaveChanges();
            }
        }
    }
}

using JetBrains.Annotations;
using MichaelsPlace.Infrastructure;

namespace MichaelsPlace.Queries
{
    /// <summary>
    /// Query for retrieving individual entities or models by ID.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [UsedImplicitly]
    public class ByIdQuery<TEntity> : QueryBase
        where TEntity : class
    {
        /// <summary>
        /// Gets a <typeparamref name="TEntity"/> by <paramref nam="id"/>;
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Execute(object id)=> DbSets.Set<TEntity>().Find(id);

        /// <summary>
        /// Gets a <typeparamref name="TModel"/> (which must have a query-compatible mapping from <typeparamref name="TEntity"/>) by <paramref nam="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TModel Execute<TModel>(object id) => Mapper.Map<TModel>(DbSets.Set<TEntity>().Find(id));
    }
}
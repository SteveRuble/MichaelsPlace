using MichaelsPlace.Infrastructure;

namespace MichaelsPlace.Queries
{
    public class ByIdQuery<TEntity> : QueryBase
        where TEntity : class
    {
        public virtual TEntity Execute(object id)=> DbContext.Set<TEntity>().Find(id);
        public virtual TModel Execute<TModel>(object id) => Mapper.Map<TModel>(DbContext.Set<TEntity>().Find(id));
    }
}
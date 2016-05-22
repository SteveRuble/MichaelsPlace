using System.Linq;

namespace MichaelsPlace.Infrastructure
{
    public interface IProjectableQuery<out T> : IQueryable<T>
    {
        IQueryable<TModel> ProjectTo<TModel>();
    }
}
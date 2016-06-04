using AutoMapper;
using MichaelsPlace.Models.Persistence;
using Ninject;

namespace MichaelsPlace.Infrastructure
{
    /// <summary>
    /// Marker interface for queries.
    /// </summary>
    public interface IQuery
    {
        
    }

    public abstract class QueryBase : IQuery
    {
        [Inject]
        public ApplicationDbContext DbContext { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }
    }
}
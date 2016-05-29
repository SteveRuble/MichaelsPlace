using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace MichaelsPlace.Infrastructure
{
    public static class ProjectableQuery
    {
        public static IProjectableQuery<T> AsProjectable<T>(this IQueryable<T> @this, IMapper mapper)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            return new ProjectableQuery<T>(mapper, @this);
        }

        public static IQueryable<TProjected> ProjectTo<TProjected>(this IQueryable @this, IMapper mapper)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            return @this.ProjectTo<TProjected>(mapper.ConfigurationProvider);
        }
    }

    /// <summary>
    /// An <see cref="IQueryable{T}"/> which can be projected to 
    /// a model type using AutoMapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProjectableQuery<T> : IProjectableQuery<T>
    {
        private readonly IMapper _mapper;
        private readonly IQueryable<T> _queryable;
        public ProjectableQuery(IMapper mapper, IQueryable<T> queryable)
        {
            _mapper = mapper;
            _queryable = queryable;
        }

        /// <summary>
        /// Projects the items in this query to <typeparamref name="TModel"/> using AutoMapper.
        /// This will fail if there are no mappings defined.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public IQueryable<TModel> ProjectTo<TModel>() => _queryable.ProjectTo<TModel>(_mapper.ConfigurationProvider);
        public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public Expression Expression => _queryable.Expression;
        public Type ElementType => _queryable.ElementType;
        public IQueryProvider Provider => _queryable.Provider;
    }
}
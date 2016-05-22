using Ninject;
using Ninject.Syntax;

namespace MichaelsPlace.Infrastructure
{
    /// <summary>
    /// Implementation of a factory which produces queries.
    /// </summary>
    public class QueryFactory : IQueryFactory
    {
        private readonly IResolutionRoot _resolutionRoot;
        public QueryFactory(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        public TQuery Create<TQuery>() where TQuery : IQuery
        {
            return _resolutionRoot.Get<TQuery>();
        }
    }
}
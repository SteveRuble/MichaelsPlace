namespace MichaelsPlace.Infrastructure
{
    /// <summary>
    /// Factory which produces queries.
    /// </summary>
    public interface IQueryFactory
    {
        TQuery Create<TQuery>() where TQuery : IQuery;
    }
}
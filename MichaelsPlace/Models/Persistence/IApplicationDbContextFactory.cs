namespace MichaelsPlace.Models.Persistence
{
    public interface IApplicationDbContextFactory
    {
        ApplicationDbContext Create();
    }
}
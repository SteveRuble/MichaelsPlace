namespace MichaelsPlace.Models.Persistence
{
    public class Tag
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string GuidanceLabel { get; set; }
    }
}
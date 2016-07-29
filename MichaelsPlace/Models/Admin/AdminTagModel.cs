using System.Data.Entity;

namespace MichaelsPlace.Models.Admin
{
    public class AdminTagModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EntityState State { get; set; }
        public AdminTagType Type { get; set; }
    }
}
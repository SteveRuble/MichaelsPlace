using System;
using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Notification : ICreated
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual string Content { get; set; }

        [Required]
        public virtual string CreatedBy { get; set; }

        [Required]
        public virtual DateTimeOffset CreatedUtc { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Item : ICreated
    {
        private ICollection<Situation> _situations;
        public virtual int Id { get; set; }

        [Required]
        public virtual string Title { get; set; }

        [Required]
        public virtual string Content { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public virtual int Order { get; set; }

        public virtual ICollection<Situation> Situations
        {
            get { return _situations ?? (_situations = new HashSet<Situation>()); }
            set { _situations = value; }
        }

        [Required]
        public virtual string CreatedBy { get; set; }

        [Required]
        public virtual DateTimeOffset CreatedUtc { get; set; }
        
    }

    public class Article : Item
    {
    }
}
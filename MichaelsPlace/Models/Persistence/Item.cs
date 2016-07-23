using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Item : ICreated, ISoftDelete
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual string Title { get; set; }

        [Required]
        public virtual string Content { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public virtual int Order { get; set; }

        private ICollection<ContextTag> _appliesToContexts;

        public virtual ICollection<ContextTag> AppliesToContexts
        {
            get { return _appliesToContexts ?? (_appliesToContexts = new HashSet<ContextTag>()); }
            set { _appliesToContexts = value; }
        }

        private ICollection<LossTag> _appliesToLosses;

        public virtual ICollection<LossTag> AppliesToLosses
        {
            get { return _appliesToLosses ?? (_appliesToLosses = new HashSet<LossTag>()); }
            set { _appliesToLosses = value; }
        }

        private ICollection<RelationshipTag> _appliesToRelationships;

        public virtual ICollection<RelationshipTag> AppliesToRelationships
        {
            get { return _appliesToRelationships ?? (_appliesToRelationships = new HashSet<RelationshipTag>()); }
            set { _appliesToRelationships = value; }
        }

        [Required]
        public virtual string CreatedBy { get; set; }

        [Required]
        public virtual DateTimeOffset CreatedUtc { get; set; }

        public virtual bool IsDeleted { get; set; }
    }

    public class Article : Item
    {
    }
}
using System.Collections.Generic;

namespace MichaelsPlace.Models.Persistence
{
    public class Tag
    {
        public virtual int Id { get; set; }
        
        /// <summary>
        /// The internal name of the Tag.
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// The name displayed in the UI for end users.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Description/help text for end users.
        /// </summary>
        public virtual string Description { get; set; }
    }

    public class RelationshipTag : Tag
    {
        public virtual ContextTag Context { get; set; }
    }

    public class LossTag : Tag
    {
        public virtual ContextTag Context { get; set; }
    }

    public class ContextTag : Tag
    {
        private ICollection<LossTag> _losses;

        public virtual ICollection<LossTag> Losses
        {
            get { return _losses ?? (_losses = new HashSet<LossTag>()); }
            set { _losses = value; }
        }

        private ICollection<RelationshipTag> _relationships;

        public virtual ICollection<RelationshipTag> Relationships
        {
            get { return _relationships ?? (_relationships = new HashSet<RelationshipTag>()); }
            set { _relationships = value; }
        }
    }

}
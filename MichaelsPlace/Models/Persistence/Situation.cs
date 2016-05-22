using System.Collections.Generic;

namespace MichaelsPlace.Models.Persistence
{
    public class Situation
    {
        private ICollection<LossTag> _losses;

        private ICollection<MournerTag> _mourners;

        private ICollection<DemographicTag> _teamMembers;
        public virtual int Id { get; set; }

        public virtual ICollection<DemographicTag> Demographics
        {
            get { return _teamMembers ?? (_teamMembers = new HashSet<DemographicTag>()); }
            set { _teamMembers = value; }
        }

        public virtual ICollection<LossTag> Losses
        {
            get { return _losses ?? (_losses = new HashSet<LossTag>()); }
            set { _losses = value; }
        }

        public virtual ICollection<MournerTag> Mourners
        {
            get { return _mourners ?? (_mourners = new HashSet<MournerTag>()); }
            set { _mourners = value; }
        }
    }
}
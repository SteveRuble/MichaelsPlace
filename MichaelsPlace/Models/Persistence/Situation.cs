using System.Collections.Generic;
using System.Linq;

namespace MichaelsPlace.Models.Persistence
{
    public class Situation : ISoftDelete
    {
        private ICollection<LossTag> _losses;

        private ICollection<MournerTag> _mourners;

        private ICollection<DemographicTag> _teamMembers;

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Memento { get; set; }

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

        public void UpdateMemento()
        {
            Memento = string.Join("-",
                                  string.Join(".", Demographics.Select(x => x.Id)),
                                  string.Join(".", Losses.Select(x => x.Id)),
                                  string.Join(".", Mourners.Select(x => x.Id)));

        }

        public virtual bool IsDeleted { get; set; }
    }
}
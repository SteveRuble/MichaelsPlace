using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.DynamicFilters;

namespace MichaelsPlace.Utilities
{
    public class DbContextFilterControl
    {
        private bool _enabled;
        private readonly DbContext _dbContext;
        public string Name { get; }

        public bool Enabled
        {
            get { return _enabled;}
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    _dbContext.EnableFilter(Name);
                }
                else
                {
                    _dbContext.DisableFilter(Name);
                }
            }
        }

        public DbContextFilterControl(string name, DbContext dbContext, bool enabled = false)
        {
            _dbContext = dbContext;
            Name = name;
            Enabled = enabled;
        }
    }
}

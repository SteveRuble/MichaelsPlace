using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    public class PreferencesQuery : QueryBase
    {
        /// <summary>
        /// Gets preferences of the specified <typeparamref name="TPreference"/> type,
        /// including the associated user.
        /// </summary>
        /// <typeparam name="TPreference"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<TPreference> Execute<TPreference>() where TPreference : UserPreference
        {
            return DbContext.UserPreferences
                            .Include(p => p.User)
                            .OfType<TPreference>();
        }
    }
}

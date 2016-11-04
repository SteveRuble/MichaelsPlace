using System.Linq;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    /// <summary>
    /// Query which returns a full case for displaying on the case dashboard.
    /// </summary>
    public class CaseByIdQuery : QueryBase
    {
        public virtual IProjectableQuery<Case> Execute<TItem>(string caseId)
        {
            var caseFromId = from cases in DbSets.Set<Case>()
                             where cases.Id == caseId
                             select cases;

            var currentCase = caseFromId.AsProjectable(Mapper);
            return currentCase;
        }
    }
}
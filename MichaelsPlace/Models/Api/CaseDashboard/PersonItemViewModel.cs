using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api.CaseDashboard
{
    public class PersonItemViewModel
    {
        public int Id { get; set; }
        public string CaseId { get; set; }
        public CaseItemUserStatus Status { get; set; }
    }
}
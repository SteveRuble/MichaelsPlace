using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api.CaseDashboard
{
    public class PersonItemViewModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public CaseItemUserStatus Status { get; set; }
    }
}
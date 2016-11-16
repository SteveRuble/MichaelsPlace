namespace MichaelsPlace.Models.Api
{
    public class UpdateStatusModel
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string CaseId { get; set; }
    }
}
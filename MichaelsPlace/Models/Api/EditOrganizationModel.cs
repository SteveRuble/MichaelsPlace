using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api
{
    public class EditOrganizationModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Notes { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int OrganizationId { get; set; }
    }
}
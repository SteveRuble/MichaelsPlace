using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace.Models.Admin
{
    /// <summary>
    /// Represents a reference to a person, with just enough information
    /// to identify the user and display a useful UI.
    /// </summary>
    public class PersonReferenceModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

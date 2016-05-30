using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Address
    {
        public virtual int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public virtual string LineOne { get; set; }
        [Required]
        [MaxLength(200)]
        public virtual string LineTwo { get; set; }
        [Required]
        [MaxLength(100)]
        public virtual string City { get; set; }
        [Required]
        [MaxLength(2)]
        public virtual string State { get; set; }
        [Required]
        [MaxLength(12)]
        public virtual string Zip { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Data
{
    public class Participant
    { 
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage ="Name can not exceed 20 characters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public OwnSecretSanta SecretSanta { get; set; }

        public Participant()
        {
            SecretSanta = new OwnSecretSanta();
        }
    }
}

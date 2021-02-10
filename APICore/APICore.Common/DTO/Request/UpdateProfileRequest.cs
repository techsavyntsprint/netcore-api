using System;
using System.ComponentModel.DataAnnotations;

namespace APICore.Common.DTO.Request
{
    public class UpdateProfileRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [Range(0, 1)]
        public int Gender { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string Phone { get; set; }
    }
}
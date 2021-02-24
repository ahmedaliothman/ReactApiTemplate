using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.JWT {
    public class RegisterRequest {
        
        [Required]
        public string Name { get; set; }

        [Required]

        public string Email { get; set; }

        [Required]

        public string Password { get; set; }

        [Required]

        public string FirstName { get; set; }

        [Required]

        public string LastName { get; set; }

        public bool? IsActive = true;

    }
}
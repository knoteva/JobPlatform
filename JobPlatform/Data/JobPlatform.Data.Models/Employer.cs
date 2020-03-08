namespace JobPlatform.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Employer : ApplicationUser
    {
        public Employer()
        {
            this.Jobs = new HashSet<Job>();
        }

        // [Required]
        // public string UserName { get; set; }
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Description { get; set; }

        // [Required]
        // public string Email { get; set; }

        // [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        // [Required]
        // public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }

        public ICollection<Job> Jobs { get; set; }
    }
}

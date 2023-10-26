namespace TestOnlineQuestion.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        public string Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        public string HoDem { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        public string Ten { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        public bool? State { get; set; }

        public bool? Role { get; set; }
    }
}

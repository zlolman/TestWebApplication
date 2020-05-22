using System;
using System.ComponentModel.DataAnnotations;

namespace TestWebApplication.Models
{
    public class Vocation
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int employeeId { get; set; }
        [Required]
        public DateTimeOffset startDate { get; set; }
        [Required]
        public DateTimeOffset endDate { get; set; }
    }
}

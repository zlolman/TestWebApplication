using System;
using System.ComponentModel.DataAnnotations;

namespace TestWebApplication.Models
{
    public class Employee
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string position { get; set; }
    }
}

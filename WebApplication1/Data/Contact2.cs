using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class Contact2
    {
        [BindProperty]
        [Key]
        public int ID { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(1000)]
        public string Message { get; set; }
    }
}


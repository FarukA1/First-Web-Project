using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class Menu
    {
        [Key]
        public int ID { get; set; } 
        [StringLength(10)]
        public string Name { get; set; } 
        public decimal Price { get; set; } 
        [StringLength(250)]
        public string Description { get; set; } 
        public string Image { get; set; }
    }
}

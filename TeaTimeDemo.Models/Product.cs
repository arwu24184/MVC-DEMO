using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TeaTimeDemo.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("產品名稱")]
        public string Name { get; set; }

        [Required]
        [DisplayName("大小")]
        public string Size { get; set; }

        [Required]
        [Range(1, 10000)]
        [DisplayName("價格")]
        public double Price { get; set; }

        [DisplayName("備註")]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }
    }
}

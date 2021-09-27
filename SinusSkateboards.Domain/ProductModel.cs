using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinusSkateboards.Domain
{
   public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Color { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int CategoryModelId { get; set; }
        public virtual CategoryModel CategoryModel { get; set; }
    }
}

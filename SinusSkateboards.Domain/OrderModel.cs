using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinusSkateboards.Domain
{
   public class OrderModel
    {
        public int Id { get; set; }
        public int CustomerModelId { get; set; }
        public virtual CustomerModel CustomerModel { get; set; }
        public virtual List<ProductModel> Products { get; set; }
        public DateTime Date { get; set; }

    }
}

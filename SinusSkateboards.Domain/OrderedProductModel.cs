using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinusSkateboards.Domain
{
  public class OrderedProductModel
    {

        public int Id { get; set; }
        public int OrderModelId { get; set; }
        public virtual OrderModel OrderModel { get; set; }

        [ForeignKey("ProductModel")]
        public int ProductModelId { get; set; }
        public virtual ProductModel ProductModel { get; set; }
    }
}

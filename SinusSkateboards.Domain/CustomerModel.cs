using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinusSkateboards.Domain
{
 
   public class CustomerModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please input name")]
        public string Name  { get; set; }
        [Required(ErrorMessage = "Please input address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please input ZipCode")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Please input city")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please input phone number")]
        public string PhoneNumber { get; set; }
    }
}

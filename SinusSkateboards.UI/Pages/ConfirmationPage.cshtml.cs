using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SinusSkateboards.Domain;

namespace SinusSkateboards.UI.Pages
{
    public class ConfirmationPageModel : PageModel
    {

        [BindProperty]
        public List<ProductModel> CookieProducts { get; set; } = IndexModel.ProductsAddedToCart;

        [BindProperty]
        public decimal TotalPrice { get; set; }
        public void OnGet()
        {

            foreach (var product in IndexModel.ProductsAddedToCart)
            {
                TotalPrice += product.Price;
            }

        }
    }
}

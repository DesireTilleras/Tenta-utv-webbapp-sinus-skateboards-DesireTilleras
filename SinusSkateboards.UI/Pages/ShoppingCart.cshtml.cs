using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SinusSkateboards.Application;
using SinusSkateboards.Domain;

namespace SinusSkateboards.UI.Pages
{
    public class ShoppingCartModel : PageModel
    {
        [BindProperty]
        public List<ProductModel> CookieProducts { get; set; } = IndexModel.ProductsAddedToCart;



        public void OnGet()
        {


        }
        public IActionResult OnPostDelete(int id)
        {

            string stringCartItems = HttpContext.Session.GetString("AddToCart");

            List<ProductModel> productsInCart = new List<ProductModel>();

            if (!String.IsNullOrEmpty(stringCartItems))
            {
                productsInCart = JsonConvert.DeserializeObject<List<ProductModel>>(stringCartItems);
            }

            var oneProduct = productsInCart.Where(x => x.Id == id).FirstOrDefault();

            productsInCart.Remove(oneProduct);

            var productRemove = IndexModel.ProductsAddedToCart.Where(x => x.Id == id).FirstOrDefault();

            IndexModel.ProductsAddedToCart.Remove(productRemove);

            stringCartItems = JsonConvert.SerializeObject(productsInCart);
            HttpContext.Session.SetString("AddToCart", stringCartItems);

            if (IndexModel.ProductsAddedToCart.Count == 0)
            {
                return RedirectToPage("Index");
            }
            else
            {
                return RedirectToPage("ShoppingCart");
            }

        }
    }
}

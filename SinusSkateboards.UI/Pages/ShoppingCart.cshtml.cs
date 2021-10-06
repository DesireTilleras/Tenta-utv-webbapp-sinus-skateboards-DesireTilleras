using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SinusSkateboards.Domain;
using SinusSkateBoards.Data.Database;

namespace SinusSkateboards.UI.Pages
{
    public class ShoppingCartModel : PageModel
    {
        [BindProperty]
        public List<ProductModel> CookieProducts { get; set; } = IndexModel.ProductsAddedToCart;

        [BindProperty]
        public CustomerModel Customer { get; set; } = new CustomerModel();

        [BindProperty]
        public decimal TotalPrice { get; set; }


        private readonly AuthDbContext _context;

        public ShoppingCartModel(AuthDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            foreach (var product in IndexModel.ProductsAddedToCart)
            {
                TotalPrice += product.Price;
            }

        }
        public async Task<IActionResult> OnPostNewCustomer()
        {

            await _context.Customers.AddAsync(Customer);

            await _context.SaveChangesAsync();

            Customer =_context.Customers.Where(n => n.Id == Customer.Id).FirstOrDefault();

             return RedirectToPage("OrderOverview", new { id = Customer.Id });

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

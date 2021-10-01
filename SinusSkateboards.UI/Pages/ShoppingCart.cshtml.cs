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

        public OrderModel Order { get; set; } = new OrderModel();

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
        public async Task<IActionResult> OnPostCheckOut()
        {
            await _context.Customers.AddAsync(Customer);

            await _context.SaveChangesAsync();

            var customer = _context.Customers.Where(x => x.Id == Customer.Id).FirstOrDefault();

            Order.CustomerModelId = customer.Id;
            Order.Date = DateTime.Now;

            await _context.Orders.AddAsync(Order);

            await _context.SaveChangesAsync();

            var order = _context.Orders.Where(x => x.CustomerModelId == customer.Id).FirstOrDefault();

            foreach (var product in IndexModel.ProductsAddedToCart)
            {
                var newProduct = new ProductModel();
                newProduct.Title = product.Title;
                newProduct.Color = product.Color;
                newProduct.Price = product.Price;
                newProduct.Image = product.Image;
                newProduct.Description = product.Description;
                newProduct.Category = product.Category;
                newProduct.OrderModelId = order.Id;
                await _context.Products.AddAsync(newProduct);
            }


            await _context.SaveChangesAsync();


            return RedirectToPage("ConfirmationPage",  new { id = order.Id });
        }
    }
}

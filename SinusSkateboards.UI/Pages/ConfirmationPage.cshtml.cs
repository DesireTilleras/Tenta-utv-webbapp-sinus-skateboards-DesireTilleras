using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SinusSkateboards.Domain;
using SinusSkateBoards.Data.Database;

namespace SinusSkateboards.UI.Pages
{
    public class ConfirmationPageModel : PageModel
    {

        [BindProperty]
        public List<ProductModel> CookieProducts { get; set; } = IndexModel.ProductsAddedToCart;

        [BindProperty]
        public decimal TotalPrice { get; set; }

        private readonly AuthDbContext _context;

        [BindProperty]

        public OrderModel Order { get; set; } = new OrderModel();


        [BindProperty]
        public CustomerModel Customer { get; set; }

        public ConfirmationPageModel(AuthDbContext context)
        {
            _context = context;
        }
        public async Task OnGet(int id)
        {
            Customer = _context.Customers.Where(c => c.Id == id).FirstOrDefault();

            foreach (var product in IndexModel.ProductsAddedToCart)
            {
                TotalPrice += product.Price;

            }

            Order.CustomerModelId = Customer.Id;
            Order.Date = DateTime.Now;

            await _context.Orders.AddAsync(Order);

            await _context.SaveChangesAsync();

            var order = _context.Orders.Where(x => x.CustomerModelId == Customer.Id).FirstOrDefault();

            foreach (var product in IndexModel.ProductsAddedToCart)
            {
                var newProduct = new ProductModel();
                newProduct.Title = product.Title;
                newProduct.Color = product.Color;
                newProduct.Price = product.Price;
                newProduct.Image = product.Image;
                newProduct.Description = product.Description;
                newProduct.Category = product.Category;
                newProduct.ArticleNumber = product.ArticleNumber;
                newProduct.OrderModelId = order.Id;
                await _context.Products.AddAsync(newProduct);
            }


            await _context.SaveChangesAsync();

            Order = _context.Orders.Where(x => x.Id == order.Id).Include(c => c.CustomerModel).Include(p => p.Products).FirstOrDefault();



            string stringCartItems = HttpContext.Session.GetString("AddToCart");

            List<ProductModel> productsInCart = new List<ProductModel>();

            if (!String.IsNullOrEmpty(stringCartItems))
            {
                productsInCart = JsonConvert.DeserializeObject<List<ProductModel>>(stringCartItems);
            }
            productsInCart.Clear();
            IndexModel.ProductsAddedToCart.Clear();


            stringCartItems = JsonConvert.SerializeObject(productsInCart);
            HttpContext.Session.SetString("AddToCart", stringCartItems);


        }

    }
}

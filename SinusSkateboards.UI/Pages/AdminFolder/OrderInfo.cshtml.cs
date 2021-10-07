using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SinusSkateboards.Domain;
using SinusSkateBoards.Data.Database;

namespace SinusSkateboards.UI.Pages.AdminFolder
{
    public class OrderInfoModel : PageModel
    {
        [BindProperty]
        public OrderModel Order { get; set; }

        public List<OrderedProductModel> OrderedProducts { get; set; }

        [BindProperty]

        public List<ProductModel> Products { get; set; } = new List<ProductModel>();

        [BindProperty]
        public decimal TotalPrice { get; set; }

        [BindProperty]

        public int AmountOfProducts { get; set; }

        private readonly AuthDbContext _context;

        public OrderInfoModel(AuthDbContext context)
        {
            _context = context;
        }
        public void OnGet(int id)
        {

            Order = _context.Orders.Where(x => x.Id == id).Include(c => c.CustomerModel).Include(p => p.OrderedProducts).FirstOrDefault();

           OrderedProducts = Order.OrderedProducts.Where(x => x.OrderModelId == Order.Id).ToList();

            foreach (var orderedProduct in OrderedProducts)
            {
                var product = _context.Products.Where(x => x.Id == orderedProduct.ProductModelId).FirstOrDefault();
                Products.Add(product);
            }

            foreach (var product in Products)
            {
                TotalPrice += product.Price;
                AmountOfProducts += 1;
            }

        }
    }
}

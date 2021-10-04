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
    public class ProductOverviewModel : PageModel
    {
        [BindProperty]
        public ProductModel Product { get; set; }

        [BindProperty]
        public List<ProductModel> ListAlsoInColors { get; set; }
        public static List<ProductModel> ListOfAllProducts { get; set; }

        private readonly AuthDbContext _context;

        public ProductOverviewModel(AuthDbContext context)
        {
            _context = context;
        }
        public void OnGet(int id)
        {
            ListOfAllProducts = _context.Products.Select(x => x).ToList();

            Product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            ListAlsoInColors = _context.Products.Where(x => x.Category == Product.Category).ToList();

            ListAlsoInColors = ListAlsoInColors.GroupBy(x => x.Color).Select(x => x.FirstOrDefault()).ToList();

            ListAlsoInColors.Remove(Product);

        }

        public IActionResult OnPostAddToCart(int id)
        {

            Product = ListOfAllProducts.Where(x => x.Id == id).FirstOrDefault();


            string stringAddedToCart = HttpContext.Session.GetString("AddToCart");

            if (!String.IsNullOrEmpty(stringAddedToCart))
            {
                IndexModel.ProductsAddedToCart = JsonConvert.DeserializeObject<List<ProductModel>>(stringAddedToCart);
            }

            IndexModel.ProductsAddedToCart.Add(Product);

            stringAddedToCart = JsonConvert.SerializeObject(IndexModel.ProductsAddedToCart);
            HttpContext.Session.SetString("AddToCart", stringAddedToCart);

            return RedirectToPage("ProductOverview", new {id = Product.Id});
        }
    }
}

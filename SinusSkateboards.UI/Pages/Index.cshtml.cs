using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SinusSkateboards.Domain;
using SinusSkateBoards.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SinusSkateboards.UI.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<Categories> CategoryList { get; set; } = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

        [BindProperty]
        public static List<ProductModel> ListOfAllProducts { get; set; }

        public static List<ProductModel> ProductsAddedToCart { get; set; } = new List<ProductModel>();

        [BindProperty]
        public List<ProductModel> MatchedProducts { get; set; } = new List<ProductModel>();

        public ProductModel Product { get; set; }

        private readonly AuthDbContext _context;

        public IndexModel(AuthDbContext context)
        {
            _context = context;

        }

        public void OnGet()
        {
            ListOfAllProducts = _context.Products.Select(x => x).ToList();
                
            CategoryList = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
        }

        public IActionResult OnPost(string search, string category)
        {
            if (category != "Category")
            {
                MatchedProducts = ListOfAllProducts.Where(x => x.Category.ToString() == category).ToList().Distinct().ToList();

                if (search != null)
                {
                    MatchedProducts = MatchedProducts.Where(x => x.Title.ToLower().Contains(search.ToLower())
                || x.Description.ToLower().Contains(search.ToLower())
                || x.Color.ToLower().Contains(search.ToLower())
                || x.Category.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                MatchedProducts = MatchedProducts.GroupBy(x => x.Image).Select(x => x.FirstOrDefault()).ToList();

                return Page();

            }
            else
            {
                if (search != null)
                {
                    MatchedProducts = ListOfAllProducts.Where(x => x.Title.ToLower().Contains(search.ToLower())
                    || x.Description.ToLower().Contains(search.ToLower())
                    || x.Color.ToLower().Contains(search.ToLower())
                    || x.Category.ToString().ToLower().Contains(search.ToLower())).ToList();

                    MatchedProducts = MatchedProducts.GroupBy(x => x.Image).Select(x => x.FirstOrDefault()).ToList();

                    return Page();

                }

            }

            return Page();


        }
        public void OnPostAddToCart(int id)
        {

            Product = ListOfAllProducts.Where(x => x.Id == id).FirstOrDefault();


            string stringAddedToCart = HttpContext.Session.GetString("AddToCart");

            if (!String.IsNullOrEmpty(stringAddedToCart))
            {
                ProductsAddedToCart = JsonConvert.DeserializeObject<List<ProductModel>>(stringAddedToCart);
            }

            ProductsAddedToCart.Add(Product);

            stringAddedToCart = JsonConvert.SerializeObject(ProductsAddedToCart);
            HttpContext.Session.SetString("AddToCart", stringAddedToCart);

            RedirectToPage("Index");
        }
    }
}

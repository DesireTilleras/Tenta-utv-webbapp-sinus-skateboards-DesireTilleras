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
        public static List<Categories> CategoryList { get; set; } = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

        [BindProperty]

        public static List<string> ColorList { get; set; } = new List<string>();

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

            ColorList = _context.Products.Select(x => x.Color).ToList();
        }

        public IActionResult OnPost(string search, string category, string color)
        {
            if (category != "Category" && color != "Color")
            {
                MatchedProducts = ListOfAllProducts.Where(x => x.Category.ToString() == category).Where(c => c.Color == color).ToList().Distinct().ToList();

                if (search != null)
                {
                    MatchedProducts = MatchedProducts.Where(x => x.Title.ToLower().Contains(search.ToLower())
                || x.Description.ToLower().Contains(search.ToLower())
                || x.Color.ToLower().Contains(search.ToLower())
                || x.Category.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                return Page();

            }
            if (category != "Category" && color == "Color")
            {
                MatchedProducts = ListOfAllProducts.Where(x => x.Category.ToString() == category).ToList().Distinct().ToList();

                if (search != null)
                {
                    MatchedProducts = MatchedProducts.Where(x => x.Title.ToLower().Contains(search.ToLower())
                || x.Description.ToLower().Contains(search.ToLower())
                || x.Color.ToLower().Contains(search.ToLower())
                || x.Category.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                return Page();

            }
            if (category == "Category" && color != "Color")
            {
                MatchedProducts = ListOfAllProducts.Where(x => x.Color == color).ToList().Distinct().ToList();

                if (search != null)
                {
                    MatchedProducts = MatchedProducts.Where(x => x.Title.ToLower().Contains(search.ToLower())
                || x.Description.ToLower().Contains(search.ToLower())
                || x.Color.ToLower().Contains(search.ToLower())
                || x.Category.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

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


                    return Page();

                }

            }

            return Page();


        }

        public void OnPostListAll()
        {
            MatchedProducts = ListOfAllProducts;
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

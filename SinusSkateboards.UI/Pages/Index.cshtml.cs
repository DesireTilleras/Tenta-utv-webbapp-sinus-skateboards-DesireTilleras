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
using SinusSkateboards.Application;

namespace SinusSkateboards.UI.Pages
{
	public class IndexModel : PageModel
	{
		[BindProperty]
		public List<Categories> CategoryList { get; set; } = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

		[BindProperty]
        public static List<ProductModel> ListOfAllProducts { get; set; }


		[BindProperty]
		public List<ProductModel> ListProductsOnCategory { get; set; }

		public static List<ProductModel> ProductsAddedToCart { get; set; } = CartListClass.ListOfCartItems;

		public List<ProductModel> MatchedProducts { get; set; } = new List<ProductModel>();
		[BindProperty]

		public List<ProductModel> ShowMatchedProducts { get; set; }

		public ProductModel Product { get; set; }

        private readonly AuthDbContext _context;

        public IndexModel(AuthDbContext context)
        {
			_context = context;

        }

		public void OnGet()
		{
			ListOfAllProducts = _context.Products.Select(x => x).ToList();			

		}

		public async Task<IActionResult> OnPost(string search)
        {
            if (search!=null)
            {
				var listTitle = ListOfAllProducts.Where(x => x.Title.Contains(search)).ToList();
                if (listTitle != null)
                {
					foreach (var product in listTitle)
					{
						MatchedProducts.Add(product);
					}
				}
		
				var listDescription = ListOfAllProducts.Where(x => x.Description.Contains(search)).ToList();

				if (listDescription != null)
				{
					foreach (var product in listDescription)
					{
						MatchedProducts.Add(product);
					}
				}

				var listColor = ListOfAllProducts.Where(x => x.Color.Contains(search)).ToList();
				if (listColor != null)
				{
					foreach (var product in listColor)
					{
						MatchedProducts.Add(product);

					}
				}

				ShowMatchedProducts = MatchedProducts.Distinct().ToList();

				return Page();
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

		public void OnPostShowProducts(string category)
        {

			ListProductsOnCategory = ListOfAllProducts.Where(x => x.Category.ToString() == category).ToList();

        }
	}
}

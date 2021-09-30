using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        private readonly AuthDbContext _context;

        public ProductOverviewModel(AuthDbContext context)
        {
            _context = context;
        }
        public void OnGet(int id)
        {
            Product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            ListAlsoInColors = _context.Products.Where(x => x.Category == Product.Category).ToList();

            ListAlsoInColors.Remove(Product);

        }
    }
}

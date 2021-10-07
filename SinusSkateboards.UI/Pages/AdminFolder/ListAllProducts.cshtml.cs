using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SinusSkateboards.Domain;
using SinusSkateBoards.Data.Database;

namespace SinusSkateboards.UI.Pages.AdminFolder
{
    public class ListAllProductsModel : PageModel
    {
        [BindProperty]
        public List<ProductModel> ListOfAllProducts { get; set; }

        private readonly AuthDbContext _context;

        public ListAllProductsModel(AuthDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            ListOfAllProducts = _context.Products.Select(x => x).ToList();

        }
    }
}

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
    public class DeleteProductModel : PageModel
    {
        private readonly AuthDbContext _context;
        public DeleteProductModel(AuthDbContext context)
        {
            _context = context;
        }

        public static ProductModel Product;

        public void OnGet(int id)
        {
          Product =  _context.Products.Where(x => x.Id == id).FirstOrDefault();
        }

        public IActionResult OnPostDelete()
        {
            var products = _context.Products.Where(x => x.ArticleNumber == Product.ArticleNumber).ToList();

            foreach (var product in products)
            {
                _context.Products.Remove(product);
            }

            _context.SaveChanges();

            return RedirectToPage("/Index");
        }
        public IActionResult OnPostDontDelete()
        {

            return RedirectToPage("/AdminFolder/ListAllProducts");
        }
    }
}

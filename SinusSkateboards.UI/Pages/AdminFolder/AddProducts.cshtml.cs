using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SinusSkateboards.Domain;
using SinusSkateBoards.Data.Database;

namespace SinusSkateboards.UI.Pages.AdminFolder
{
    public class AddProductsModel : PageModel
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly AuthDbContext _context;
        [BindProperty]
        public ProductModel Product { get; set; } = new ProductModel();

        public List<Categories> CategoryList { get; set; } = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
        public AddProductsModel(AuthDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [BindProperty]
        public IFormFile Photo { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {

            if (Photo != null)
            {
                //Create folder

                string folder = Path.Combine(webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Delete existing photo

                if (Product.Image == null)
                {
                    Product.Image = "empty";
                }

                string oldFile = Path.Combine(folder, Product.Image);


                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                // Upload new photo

                string uniqueFileName = String.Concat(Guid.NewGuid().ToString(),"sinus","-", Product.Category,"-", Product.Title,"-", Product.Color, ".png");

                string newFile = Path.Combine(folder, uniqueFileName);

                using (var fileStream = new FileStream(newFile, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }

                // Update repo with new photopath

                Product.Image = uniqueFileName;

                _context.Products.Add(Product);
                _context.SaveChanges();

                return RedirectToPage("/Index");

            }

            return RedirectToPage("/AddProduct");



        }
    }
}

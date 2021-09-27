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
    public class EditProductModel : PageModel
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly AuthDbContext _context;


        public List<Categories> CategoryList { get; set; } = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
        public EditProductModel(AuthDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
            _context = context;
        }
        [BindProperty]
        public ProductModel Product { get; set; }

        [BindProperty]
        public string NewTitle { get; set; }
        [BindProperty]
        public decimal NewPrice { get; set; }
        [BindProperty]
        public string NewColor { get; set; }
        [BindProperty]
        public string NewDescription { get; set; }

        [BindProperty]
        public Categories NewCategory { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public void OnGet(int id)
        {
            Product = _context.Products.Where(x => x.Id == id).FirstOrDefault();


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

                string uniqueFileName = String.Concat(Guid.NewGuid().ToString(), "sinus", "-", Product.Category, "-", Product.Title, "-", Product.Color, ".png");

                string newFile = Path.Combine(folder, uniqueFileName);

                using (var fileStream = new FileStream(newFile, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }

                // Update repo with new photopath
                if (NewTitle != null)
                {
                    _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Title = NewTitle;
                }
                if (NewPrice != 0)
                {
                    _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Price = NewPrice;
                }
                if (NewColor != null)
                {
                    _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Color = NewColor;
                }
                if (NewDescription != null)
                {
                    _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Description = NewDescription;
                }
                if (NewCategory != 0)
                {
                    _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Category = NewCategory;
                }


                Product.Image = uniqueFileName;

                _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Image = uniqueFileName;

                _context.SaveChanges();

                return RedirectToPage("/AdminFolder/AddProducts");

            }


            if (NewTitle != null)
            {
                _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Title = NewTitle;
            }
            if (NewPrice != 0)
            {
                _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Price = NewPrice;
            }
            if (NewColor != null)
            {
                _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Color = NewColor;
            }
            if (NewDescription != null)
            {
                _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Description = NewDescription;
            }
            if (NewCategory != 0)
            {
                _context.Products.Where(x => x.Id == Product.Id).FirstOrDefault().Category = NewCategory;
            }


            _context.SaveChanges();

            return RedirectToPage("/AdminFolder/AddProducts");



        }
    }
}

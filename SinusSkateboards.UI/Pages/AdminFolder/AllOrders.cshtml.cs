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
    public class AllOrdersModel : PageModel
    {
        [BindProperty]
        public List<OrderModel> AllOrders { get; set; }


        private readonly AuthDbContext _context;

        public AllOrdersModel(AuthDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {

            AllOrders = _context.Orders.Include(c => c.CustomerModel).Include(p => p.OrderedProducts).ToList();          


        }
    }
}

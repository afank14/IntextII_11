using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IntexII_11.Models;
using System.Linq;

namespace IntexII_11.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }

        public IActionResult OnGet(int id)
        {
            Product = _context.Products.FirstOrDefault(p => p.product_ID == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}


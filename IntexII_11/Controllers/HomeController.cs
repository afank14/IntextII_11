using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IntexII_11.Models;
using Microsoft.EntityFrameworkCore;

namespace IntexII_11.Controllers;

public class HomeController : Controller
{
    private IAuroraRepository _repo;

    public HomeController(IAuroraRepository temp)
    {
        _repo = temp;
    }

    [HttpGet]
    public IActionResult Index(string[] category, string primaryColor, string secondaryColor)
    {
        IQueryable<Product> products = _repo.Products.AsQueryable();

        if (category != null && category.Any())
        {
            products = products.Where(p => category.Any(c => p.category.Contains(c)));
        }

        if (!string.IsNullOrEmpty(primaryColor))
        {
            products = products.Where(p => p.primary_color == primaryColor);
        }

        if (!string.IsNullOrEmpty(secondaryColor))
        {
            products = products.Where(p => p.secondary_color == secondaryColor);
        }

        return View(products.ToList());
    }

    [HttpGet]
    public IActionResult AboutPage()
    {
        return View();
    }

    [HttpGet]
    public IActionResult MyBag()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult ProductDetail(int Id)
    {
        var product = _repo.Products.FirstOrDefault(p => p.product_ID == Id);
        
        if (product == null)
        {
            return NotFound();
        }
        
        return View(product);
    }
}
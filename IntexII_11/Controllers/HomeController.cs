using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IntexII_11.Models;

namespace IntexII_11.Controllers;

public class HomeController : Controller
{
    private IAuroraRepository _repo;

    public HomeController(IAuroraRepository temp)
    {
        _repo = temp;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var products = _repo.Products.ToList();

        return View(products);
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
}
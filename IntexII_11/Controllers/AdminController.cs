using IntexII_11.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntexII_11.Controllers;

public class AdminController : Controller
{
    private IAuroraRepository _repo;

    public AdminController(IAuroraRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Orders()
    {
        var orders = _repo.Orders
            .OrderByDescending(o => o.date)
            .Take(20)
            .ToList();
        return View(orders);
    }

    [HttpGet]
    public IActionResult OrderDetail(int Id)
    {
        // Retrieve all line items with the matching order_ID
        var lineItems = _repo.LineItems.Where(li => li.order_ID == Id).ToList();

        if (lineItems == null || !lineItems.Any())
        {
            return NotFound();
        }

        // Retrieve products associated with the line items
        var productIds = lineItems.Select(li => li.product_ID).ToList();
        var products = _repo.Products.Where(p => productIds.Contains(p.product_ID)).ToList();

        return View(products);
    }
}
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
        
        var productQuantities = lineItems.Select(li => (productId: li.product_ID, quantity: li.qty)).ToList();


        // Retrieve products associated with the line items
        var productIds = lineItems.Select(li => li.product_ID).ToList();
        var products = _repo.Products.Where(p => productIds.Contains(p.product_ID)).ToList();
        
        ViewBag.qty = productQuantities.Select(pq => pq.quantity).ToList();

        return View(products);
    }

    [HttpGet]
    public IActionResult Products()
    {
        var products = _repo.Products.ToList();

        return View(products);
    }

    [HttpGet]
    public IActionResult Customers()
    {
        // only display customers with email addresses moving forward
        var customers = _repo.Customers
            .Where(c => c.email != null) // Example: Filter out records with NULL values
            .ToList();

        return View(customers);
    }

    [HttpGet]
    public IActionResult NewProduct()
    {
        return View(new Product());
    }
    
    // Post action for the NewProduct
    [HttpPost]
    public IActionResult NewProduct(Product p)
    {
        // If the ModelState is Valid add it to the database
        if (ModelState.IsValid) 
        {
            // call the AddProduct method from the repository
            _repo.AddProduct(p);

            return RedirectToAction("Products");
        }
        // If it isn't valid, stay on this screen and make them edit it til it is valid
        else
        {
            return View(p);
        }        
    }

    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        var productToEdit = _repo.Products.FirstOrDefault(p => p.product_ID == id);
        
        return View("NewProduct", productToEdit);
    }
    
    [HttpPost]
    public IActionResult EditProduct(Product updatedProduct)
    {
        // If what they edited is valid
        if (ModelState.IsValid)
        {
            // Call the Edit Product method to update the record
            _repo.EditProduct(updatedProduct);
                
            // go to the Products page
            return RedirectToAction("Products");
        }
        // If they took out a required field
        else
        {
            // make them re-update the task
            return View("NewProduct", updatedProduct);
        } 
    }
    
    // Get Action for Delete Product
    [HttpGet]
    public IActionResult DeleteProduct(int id)
    {
        // grab the record to delete based on the id passed
        var recordToDelete = _repo.Products.Single(x => x.product_ID == id);

        return View(recordToDelete);
    }
    
    // Post action for Delete Product
    [HttpPost]
    public IActionResult DeleteProduct(Product deletedProduct)
    {
        // call the DeleteStat method to delete the task
        _repo.DeleteProduct(deletedProduct);

        // redirect to index to see the products
        return RedirectToAction("Products");
    }
    
    [HttpGet]
    public IActionResult NewCustomer()
    {
        return View(new Customer());
    }
    
    // Post action for the NewProduct
    [HttpPost]
    public IActionResult NewCustomer(Customer c)
    {
        // If the ModelState is Valid add it to the database
        if (ModelState.IsValid) 
        {
            // call the AddProduct method from the repository
            _repo.AddCustomer(c);

            return RedirectToAction("Customers");
        }
        // If it isn't valid, stay on this screen and make them edit it til it is valid
        else
        {
            return View(c);
        }        
    }
    
    [HttpGet]
    public IActionResult EditCustomer(int id)
    {
        var customerToEdit = _repo.Customers.FirstOrDefault(c => c.customer_ID == id);
        
        return View("NewCustomer", customerToEdit);
    }
    
    [HttpPost]
    public IActionResult EditCustomer(Customer updatedCustomer)
    {
        // If what they edited is valid
        if (ModelState.IsValid)
        {
            // Call the Edit Product method to update the record
            _repo.EditCustomer(updatedCustomer);
                
            // go to the Products page
            return RedirectToAction("Customers");
        }
        // If they took out a required field
        else
        {
            // make them re-update the task
            return View("NewCustomer", updatedCustomer);
        } 
    }
    
    // Get Action for Delete Product
    [HttpGet]
    public IActionResult DeleteCustomer(int id)
    {
        // grab the record to delete based on the id passed
        var recordToDelete = _repo.Customers.Single(c => c.customer_ID == id);

        return View(recordToDelete);
    }
    
    // Post action for Delete Product
    [HttpPost]
    public IActionResult DeleteCustomer(Customer deletedCustomer)
    {
        // call the DeleteStat method to delete the task
        _repo.DeleteCustomer(deletedCustomer);

        // redirect to index to see the products
        return RedirectToAction("Customers");
    }
}
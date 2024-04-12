using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IntexII_11.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IntexII_11.Controllers;

public class HomeController : Controller
{
    private IAuroraRepository _repo;
    private InferenceSession _session;

    public HomeController(IAuroraRepository temp)
    {
        _repo = temp;
        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string onnxFilePath = Path.Combine(currentDirectory, "random_forest_classifier.onnx");
            _session = new InferenceSession(onnxFilePath);
        }
        catch (Exception)
        {
            Console.WriteLine();
            throw;
        }
    }


    [HttpGet]
    public IActionResult Index()
    {
        IQueryable<Product> products = _repo.Products.AsQueryable();

        // Check if the user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var customers = _repo.Customers.Where(c => c.email != null).ToList();


            // Logic for authenticated users
            var customer = customers.FirstOrDefault(c => c.email == User.Identity.Name);
            if (customer != null && customer.customer_ID == 60)
            {
                // Retrieve products based on recommendations from User60Recs table
                var recommendedProductNames = _repo.User60Recs.Select(r => r.Recommendation).ToList();

                // Retrieve products from Products table where the name matches recommendations
                products = _repo.Products.Where(p => recommendedProductNames.Contains(p.name));
            }
            else
            {
                // Logic for authenticated users that don't have personal recommendations saved
                products = _repo.Products
                    .OrderByDescending(p => p.avg_rating)
                    .Take(15);
            }
        }
        else
        {
            // Logic for non-authenticated users
            products = _repo.Products
                .OrderByDescending(p => p.avg_rating)
                .Take(15);
        }


        return View(products.ToList());
    }

    [HttpPost]
    public IActionResult Index(string[] category, string primaryColor, string secondaryColor)
    {
        IQueryable<Product> products = _repo.Products.AsQueryable();

        // Check if category filter is applied
        if (category != null && category.Any())
        {
            products = products.Where(p => category.Any(c => p.category.Contains(c)));
        }
        // Check if primary color filter is applied
        if (!string.IsNullOrEmpty(primaryColor))
        {
            products = products.Where(p => p.primary_color == primaryColor);
        }
        // Check if secondary color filter is applied
        if (!string.IsNullOrEmpty(secondaryColor))
        {
            products = products.Where(p => p.secondary_color == secondaryColor);
        }
        return View(products.ToList());
    }
    
    [HttpGet]
    public IActionResult Register(string email)
    {
        if (email != null)
        {
            // Use the email here
            ViewBag.Email = email;
        }

        return View(new Customer());
    }

    [HttpPost]
    public IActionResult Register(Customer c)
    {
        // If the ModelState is Valid add it to the database
        if (ModelState.IsValid) 
        {
            // call the AddProduct method from the repository
            _repo.AddCustomer(c);

            return RedirectToAction("Index");
        }
        // If it isn't valid, stay on this screen and make them edit it til it is valid
        else
        {
            return View(c);
        } 
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

    [Authorize(Roles = "Member")]
    public IActionResult Checkout()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ProductDetail(int Id)
    {
        var product = _repo.Products.FirstOrDefault(p => p.product_ID == Id);
        if (product == null)
        {
            return NotFound();
        }

        var recommendations = new List<Product>();
        for (var i = 1; i <= 5; i++)
        {
            // Fetch the product ID associated with the recommendation name
            var recName = (string)product.GetType().GetProperty($"rec_{i}").GetValue(product);
            if (!string.IsNullOrEmpty(recName))
            {
                var recProduct = _repo.Products.FirstOrDefault(p => p.name == recName);
                if (recProduct != null)
                {
                    recommendations.Add(recProduct);
                }
            }
        }

        // Pass both the product and recommendations to the view
        ViewBag.Recommendations = recommendations;
        return View(product);
    }

    [HttpPost]
    public IActionResult Checkout(List<Product> cart, int time, int amount, int age, string billing_country, string shipping_country, string type_of_card,
        string country_of_residence, string gender)
    {
        var country_of_transaction_India = 0;
        var country_of_transaction_Russia = 0;
        var country_of_transaction_USA = 0;
        var country_of_transaction_United_Kingdom = 0;
        var type_of_transaction_Online = 1;
        var entry_mode_PIN = 0;
        var shipping_address_United_Kingdom = 0;
        var type_of_card_Visa = 0;
        var country_of_residence_Russia = 0;
        var country_of_residence_USA = 0;
        var country_of_residence_United_Kingdom = 0;
        var gender_M = 0;

        if (billing_country.Equals("India"))
        {
            country_of_transaction_India = 1;
        }
        else if (billing_country.Equals("Russia"))
        {
            country_of_transaction_Russia = 1;
        }
        else if (billing_country.Equals("United States"))
        {
            country_of_transaction_USA = 1;
        }
        else if (billing_country.Equals("United Kingdom"))
        {
            country_of_transaction_United_Kingdom = 1;
        }

        if (shipping_country.Equals("United Kingdom"))
        {
            shipping_address_United_Kingdom = 1;
        }

        if (type_of_card.Equals("Visa"))
        {
            type_of_card_Visa = 1;
        }

        if (country_of_residence.Equals("Russia"))
        {
            country_of_residence_Russia = 1;
        }
        else if (country_of_residence.Equals("United States"))
        {
            country_of_residence_USA = 1;
        }
        else if (country_of_residence.Equals("United Kingdom"))
        {
            country_of_residence_United_Kingdom = 1;
        }

        if (gender.Equals("M"))
        {
            gender_M = 1;
        }

        try
        {

            var input = new List<float>
        {
            time, amount, age, entry_mode_PIN, type_of_transaction_Online, country_of_transaction_India,
            country_of_transaction_Russia, country_of_transaction_USA, country_of_transaction_United_Kingdom,
            shipping_address_United_Kingdom, type_of_card_Visa, country_of_residence_Russia,
            country_of_residence_USA,
            country_of_residence_United_Kingdom, gender_M
        };
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

            var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
        };

            using (var results = _session.Run(inputs)) // makes the prediction from the inputs from the form
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>()
                    .ToArray();

                if (prediction != null && prediction[0] == 1)
                {
                    ViewBag.Prediction = true;
                }
                else
                {
                    ViewBag.Prediction = false;
                }
            }
            // Generate a random customer_ID
            Random rnd = new Random();
            int customerId = rnd.Next(1, 1000); // Generates a random number between 1 and 999

            // Calculate total amount
            decimal totalAmount = 0;
            foreach (var product in cart)
            {
                totalAmount += (decimal)product.price;
            }

            // Create a new order
            var newOrder = new Order
            {
                customer_ID = customerId,
                date = DateOnly.FromDateTime(DateTime.UtcNow),
                day_of_week = DateTime.UtcNow.DayOfWeek.ToString(),
                time = DateTime.UtcNow.Hour,
                entry_mode = "PIN",
                amount = (Decimal)totalAmount, // Convert int to Decimal
                type_of_transaction = "Online",
                country_of_transaction = billing_country,
                shipping_address = shipping_country,
                type_of_card = type_of_card,
                predicted_fraud = ViewBag.Prediction // Get the prediction from ViewBag
            };

            // Add the new order to the repository
            _repo.AddOrder(newOrder);
            _repo.SaveChanges(newOrder);

            // Clear the cart after checkout
            //HttpContext.Session.Remove("cart");

            return View();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public IActionResult Privacy()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult RedirectToCoreAdmin()
    {
        // Perform any necessary operations here
        return Redirect("/coreadmin");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
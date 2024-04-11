using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IntexII_11.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.EntityFrameworkCore;

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
            _session = new InferenceSession("/Users/alexfankhauser/Documents/GitHub/IntextII_11/IntexII_11/random_forest_classifier.onnx");
        }
        catch (Exception)
        {
            Console.WriteLine();
            throw;
        }
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
    
    [HttpGet]
    public IActionResult ProductDetail(int Id)
    {
        var product = _repo.Products.FirstOrDefault(p => p.product_ID == Id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    public IActionResult Checkout(int time, int amount, int age, string billing_country, string shipping_country, string type_of_card, 
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
                    var fraud = true;
                    // Use the prediction to display fraud
                    ViewBag.Prediction = fraud;
                }
                else
                {
                    var fraud = false;
                    ViewBag.Prediction = fraud;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    
        return View();
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
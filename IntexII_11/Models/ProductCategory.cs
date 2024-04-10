using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IntexII_11.Models;

[PrimaryKey(nameof(Product), nameof(Category))]
public class ProductCategory
{

    [ForeignKey("product_ID")]
    public int? product_ID { get; set; }
    public Product? Product { get; set; }
    
    [ForeignKey("category_ID")]
    public int? category_ID { get; set; }
    public Category? Category { get; set; }
}
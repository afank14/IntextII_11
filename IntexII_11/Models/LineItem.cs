using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IntexII_11.Models;

[PrimaryKey("order_ID", "product_ID")]
public class LineItem
{
    
    [ForeignKey("order_ID")]
    public int? order_ID { get; set; }
    public Order? Order { get; set; }
    
    
    [ForeignKey("product_ID")]
    public int? product_ID { get; set; }
    public Product? Product { get; set; }
    
    [Required]
    public int qty { get; set; }
    
    public int? rating { get; set; }
    
}
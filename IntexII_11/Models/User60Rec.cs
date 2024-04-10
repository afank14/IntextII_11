using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntexII_11.Models;

public class User60Rec
{
    [Key]
    [ForeignKey("product_ID")]
    public int? product_ID { get; set; }
    public Product? Product { get; set; }
    
    public string Recommendation { get; set; }
    public decimal Rank_mean { get; set; }
}
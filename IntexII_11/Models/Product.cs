using System.ComponentModel.DataAnnotations;
namespace IntexII_11.Models;
public class Product
{
    [Key]
    [Required]
    public int product_ID { get; set; }

    [Required] 
    public string name { get; set; }

    [Required]
    public int year { get; set; }
    [Required]
    public int num_parts { get; set; }
    [Required]
    public double price { get; set; }
    [Required]
    public string img_link { get; set; }
    [Required]
    public string primary_color { get; set; }
    [Required]
    public string secondary_color { get; set; }
    [Required]
    public string description { get; set; }
    
    public float? avg_rating { get; set; }
    public string? rec_1 { get; set; }
    public string? rec_2 { get; set; }
    public string? rec_3 { get; set; }
    public string? rec_4 { get; set; }
    public string? rec_5 { get; set; }
    public string? category { get; set; }
}
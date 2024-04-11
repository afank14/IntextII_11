using System.ComponentModel.DataAnnotations;

namespace IntexII_11.Models;

public class Product
{
    [Key]
    public int product_ID { get; set; }
    public string name { get; set; }
    public int year { get; set; }
    public int num_parts { get; set; }
    public double price { get; set; }
    public string img_link { get; set; }
    public string primary_color { get; set; }
    public string secondary_color { get; set; }
    public string description { get; set; }
    public float avg_rating { get; set; }
    public string rec_1 { get; set; }
    public string rec_2 { get; set; }
    public string rec_3 { get; set; }
    public string rec_4 { get; set; }
    public string rec_5 { get; set; }
    public string category { get; set; }

}
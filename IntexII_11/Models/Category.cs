using System.ComponentModel.DataAnnotations;

namespace IntexII_11.Models;

public class Category
{
    [Key]
    public int category_ID { get; set; }
    [Required]
    public string category_name { get; set; }
}
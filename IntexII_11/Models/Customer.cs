using System.ComponentModel.DataAnnotations;

namespace IntexII_11.Models;

public class Customer
{
    [Key]
    [Required]
    public int customer_ID { get; set; }
    [Required]
    public string first_name { get; set; }
    [Required]
    public string last_name { get; set; }
    [Required]
    public DateOnly birth_date { get; set; }
    [Required]
    public string country_of_residence { get; set; }
    [Required]
    public string gender { get; set; }
    [Required]
    public int age { get; set; }
    public string? email { get; set; }
}
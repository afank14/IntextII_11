using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntexII_11.Models;

public class Order
{
    [Key]
    [Required]
    public int order_ID { get; set; }
    
    [ForeignKey("Customer")]
    public int? customer_ID {  get; set; }
    public Customer? Customer { get; set; }
    [Required]
    public DateOnly date { get; set; }
    [Required]
    public string day_of_week { get; set; }
    [Required]
    public int time { get; set; }
    [Required]
    public string entry_mode { get; set; }
    [Required]
    public Decimal amount { get; set; }
    [Required]
    public string type_of_transaction { get; set; }
    [Required]
    public string country_of_transaction { get; set; }
    [Required]
    public string shipping_address { get; set; }
    [Required]
    public string type_of_card { get; set; }
    
    public bool? fraud { get; set; }
    [Required]
    public bool predicted_fraud { get; set; }
}
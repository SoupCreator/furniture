using System.ComponentModel.DataAnnotations.Schema;

namespace myApp.Models;

[Table("Услуги в заказе")]
public class ServicesInCart
{
    [Column( "woID")]
    public int woID { get; set; }
    [Column("serviceID")]
    public int serviceID { get; set; }
    [Column("serviceQuantity")]
    public int serviceQuantity { get; set; }
}
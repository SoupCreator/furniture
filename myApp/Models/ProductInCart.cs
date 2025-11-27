using System.ComponentModel.DataAnnotations.Schema;

namespace myApp.Models;

[Table("Товары в заказе")]
public class ProductInCart
{
    [Column( "woID")]
    public int woID { get; set; }
    [Column("productID")]
    public int productID { get; set; }
    [Column("productQuantity")]
    public int productQuantity { get; set; }
}
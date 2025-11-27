using System.ComponentModel.DataAnnotations.Schema;

namespace myApp.Models;

[Table("Товары")]
public class Product
{
    [Column("productID")]
    public int ProductId { get; set; }
    [Column("productName")]
    public string ProductName { get; set; }
    [Column("productPrice")]
    public double ProductPrice { get; set; }
    [Column("productParams")]
    public string ProductParams { get; set; }
    [Column("fabricName")]
    public string FabricName { get; set; }
    [Column("productInStock")]
    public bool ProductInStock { get; set; }
    [Column("productImage")]
    public string ProductImage { get; set; }
}

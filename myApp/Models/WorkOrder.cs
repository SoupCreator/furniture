using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace myApp.Models;

[Table("Заказ-наряды")]
public class WorkOrder
{
    [Column("woID")]
    public int WorkOrderId { get; set; }
    [Column("woPrice")]
    public double Price { get; set; }
    [Column("woDate")]
    public DateTime Date { get; set; }
    [Column("clientID")]
    public int ClientId { get; set; }
    [Column("sellerID")]
    public int SellerId { get; set; }
    
}
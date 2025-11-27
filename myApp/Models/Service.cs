using System.ComponentModel.DataAnnotations.Schema;

namespace myApp.Models;

[Table("Услуги")]
public class Service
{
    [Column("serviceId")]
    public int ServiceId { get; set; }
    [Column("serviceName")]
    public string ServiceName { get; set; }
    [Column("servicePrice")]
    public double ServicePrice { get; set; }
}
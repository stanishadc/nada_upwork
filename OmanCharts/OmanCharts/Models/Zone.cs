using System.ComponentModel.DataAnnotations;

namespace OmanCharts.Models
{
    public class Zone
    {
        [Key]
        public Guid ZoneId { get; set; }
        public string? ZoneName { get; set; }
    }
}

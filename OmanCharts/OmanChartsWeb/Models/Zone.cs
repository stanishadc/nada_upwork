using System.ComponentModel.DataAnnotations;

namespace OmanChartsWeb.Models
{
    public class Zone
    {
        public Guid ZoneId { get; set; }
        public string? ZoneName { get; set; }
    }
}

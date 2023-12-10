using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmanChartsWeb.Models
{
    public class Zone
    {
        public Guid ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}

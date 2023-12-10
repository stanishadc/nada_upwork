using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmanCharts.Models
{
    public class Zone
    {
        [Key]
        public Guid ZoneId { get; set; }
        [Required, Column(TypeName = "varchar(100)")]
        public string? ZoneName { get; set; }
        [Required, Column(TypeName = "varchar(25)")]
        public string? Latitude { get; set; }
        [Required, Column(TypeName = "varchar(25)")]
        public string? Longitude { get; set; }
    }
    public class ZoneLabour
    {
        public Guid? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public double? TotalLabour { get; set; }
    }
}

namespace OmanChartsWeb.Models
{
    public class Statistic
    {
        public Guid StatisticId { get; set; }
        public Guid? ZoneId { get; set; }
        public double? OmanizationRate { get; set; }
        public double? TotalLabour { get; set; }
        public double? Investments { get; set; }
        public double? TotalInvestors { get; set; }
        public double? TotalProjects { get; set; }
        public string? Year { get; set; }
        public string? ProjectCategory { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

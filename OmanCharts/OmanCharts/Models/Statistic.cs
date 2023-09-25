﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmanCharts.Models
{
    public class Statistic
    {
        [Key]
        public Guid StatisticId { get; set; }
        public Guid? ZoneId { get; set; }
        public double? OmanizationRate { get; set; }
        public double? TotalLabour { get; set; }
        public double? Investments { get; set; }
        public double? TotalInvestors { get; set; }
        public double? TotalProjects { get; set; }

        [Required, Column(TypeName = "varchar(4)")]
        public string? Year { get; set; }

        [Required, Column(TypeName = "varchar(4)")]
        public string? ProjectCategory { get; set; }

        public DateTime LastUpdated { get; set; }

    }
}

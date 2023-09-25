using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OmanCharts.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? ProjectNumber { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? ProjectName { get; set; }

        public Guid? ZoneId { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? ProjectCategory { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? Year { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? FinanceEntity { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? Contractor { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? Consultant { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? Progress { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string? ProjectPeriod { get; set; }

        public DateTime StartDateContract { get; set; }
        public DateTime EndDateContract { get; set; }
        public DateTime CompletionDate { get; set; }
        public DateTime timeExtension { get; set; }

        public double? Cost { get; set; }
        public double? IdentifiedCost { get; set; }
        public double? Expenses { get; set; }
        public double? RemainingCost { get; set; }
        public double? ChangeRequestCost { get; set; }

        [Required, Column(TypeName = "varchar(45)")]
        public string? RequiredAction { get; set; }

        public string? Challenges { get; set; }

    }
}

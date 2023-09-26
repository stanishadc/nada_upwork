using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OmanChartsWeb.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string? ProjectNumber { get; set; }
        public string? ProjectName { get; set; }
        public Guid? ZoneId { get; set; }
        public Guid? UserId { get; set; }
        public string? ZoneName { get; set; }
        public string? ProjectCategory { get; set; }
        public string? Year { get; set; }
        public string? FinanceEntity { get; set; }
        public string? Contractor { get; set; }
        public string? Consultant { get; set; }
        public string? Progress { get; set; }
        public string? ProjectPeriod { get; set; }
        public DateTime StartDateContract { get; set; }
        public DateTime EndDateContract { get; set; }
        public DateTime CompletionDate { get; set; }
        public DateTime TimeExtension { get; set; }
        public double? Cost { get; set; }
        public double? IdentifiedCost { get; set; }
        public double? Expenses { get; set; }
        public double? RemainingCost { get; set; }
        public double? ChangeRequestCost { get; set; }
        public string? RequiredAction { get; set; }
        public string? Challenges { get; set; }
        public List<SelectListItem> ListofZones { get; set; }
    }
}

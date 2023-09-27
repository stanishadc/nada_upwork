using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmanCharts.Models;
using System.Net;

namespace OmanCharts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public StatisticController(RepositoryContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var data = await (from p in _context.tblStatistics
                              join z in _context.Zones on p.ZoneId equals z.ZoneId
                              select new
                              {
                                  p.StatisticId,
                                  p.ZoneId,
                                  p.Year,
                                  p.TotalLabour,
                                  p.OmanizationRate,
                                  p.TotalProjects,
                                  p.Investments,
                                  p.TotalInvestors,
                                  p.ProjectCategory,
                                  p.LastUpdated,
                                  z.ZoneName,
                              }).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpGet]
        [Route("GetById/{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var data = await (from p in _context.tblStatistics
                              join z in _context.Zones on p.ZoneId equals z.ZoneId
                              select new
                              {
                                  p.StatisticId,
                                  p.ZoneId,
                                  p.Year,
                                  p.TotalLabour,
                                  p.OmanizationRate,
                                  p.TotalProjects,
                                  p.Investments,
                                  p.TotalInvestors,
                                  p.ProjectCategory,
                                  p.LastUpdated,
                                  z.ZoneName,
                              }).Where(z => z.StatisticId == Id).FirstOrDefaultAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpGet]
        [Route("GetByZone")]
        public async Task<IActionResult> GetByZone(Guid ZoneId, Guid UserId)
        {
            var data = await (from p in _context.tblStatistics
                              join z in _context.Zones on p.ZoneId equals z.ZoneId
                              select new
                              {
                                  p.StatisticId,
                                  p.Year,
                                  p.TotalLabour,
                                  p.OmanizationRate,
                                  p.TotalProjects,
                                  p.Investments,
                                  p.TotalInvestors,
                                  p.ProjectCategory,
                                  p.LastUpdated,
                                  p.UserId,
                                  z.ZoneId,
                                  z.ZoneName,
                              }).Where(z => z.ZoneId == ZoneId && z.UserId == UserId).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpGet]
        [Route("GetUserDashboard")]
        public async Task<IActionResult> GetUserDashboard(Guid ZoneId, Guid UserId)
        {
            var data = await _context.tblStatistics.Where(s => s.ZoneId == ZoneId && s.UserId == UserId).ToListAsync();
            if (data.Count > 0)
            {
                var lastrecord = data.LastOrDefault();
                double totalInvestments = (double)(from num in data select num.Investments).Sum();
                double totalProjects = (double)(from num in data select num.TotalProjects).Sum();
                double totalLabour = (double)(from num in data select num.TotalLabour).Sum();
                double totalInvestors = (double)(from num in data select num.TotalInvestors).Sum();
                return Ok(new { StatusCode = HttpStatusCode.OK, TotalInvestments = totalInvestments, TotalProjects = totalProjects, TotalLabour = totalLabour, TotalInvestors = totalInvestors, LastUpdated = lastrecord.LastUpdated.ToString("MMMM dd, yyyy"), OmanizationRate = lastrecord.OmanizationRate });
            }
            return Ok(new { StatusCode = HttpStatusCode.OK, TotalInvestments = 0, TotalProjects = 0, TotalLabour = 0, TotalInvestors = 0, LastUpdated=DateTime.UtcNow.ToString("MMMM dd, yyyy"), OmanizationRate = 0 });
        }
        [HttpGet]
        [Route("GetAdminDashboard")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var data = await _context.tblStatistics.ToListAsync();
            if (data.Count > 0)
            {
                var lastrecord = data.LastOrDefault();
                double totalInvestments = (double)(from num in data select num.Investments).Sum();
                double totalProjects = (double)(from num in data select num.TotalProjects).Sum();
                double totalLabour = (double)(from num in data select num.TotalLabour).Sum();
                double totalInvestors = (double)(from num in data select num.TotalInvestors).Sum();
                return Ok(new { StatusCode = HttpStatusCode.OK, TotalInvestments = totalInvestments, TotalProjects = totalProjects, TotalLabour = totalLabour, TotalInvestors = totalInvestors, LastUpdated = lastrecord.LastUpdated.ToString("MMMM dd, yyyy"), OmanizationRate=lastrecord.OmanizationRate });
            }
            return Ok(new
            {
                StatusCode = HttpStatusCode.OK,
                TotalInvestments = 0,
                TotalProjects = 0,
                TotalLabour = 0,
                TotalInvestors = 0,
                LastUpdated = DateTime.UtcNow.ToString("MMMM dd, yyyy"),
                OmanizationRate=0
            });
        }
        [HttpGet]
        [Route("GetDashboard")]
        public async Task<IActionResult> GetDashboard(Guid? UserId, Guid? ZoneId)
        {
            List<Statistic> data = new List<Statistic>();
            if(UserId == null || ZoneId == null)
            {
                data= await _context.tblStatistics.ToListAsync();
            }
            else
            {
                data = await _context.tblStatistics.Where(s => s.UserId == UserId && s.ZoneId == ZoneId).ToListAsync();
            }
            if (data.Count > 0)
            {
                //Counts

                var lastrecord = data.LastOrDefault();
                double totalInvestments = (double)(from num in data select num.Investments).Sum();
                double totalProjects = (double)(from num in data select num.TotalProjects).Sum();
                double totalLabour = (double)(from num in data select num.TotalLabour).Sum();
                double totalInvestors = (double)(from num in data select num.TotalInvestors).Sum();


                //Charts
                double?[] ProjectSeries = new double?[data.Count];
                double?[] LabourSeries = new double?[data.Count];
                string[] Labels = new string[data.Count];
                List<string> labelsData = new List<string>();
                List<double?> seriesProjectsData = new List<double?>();
                List<double?> seriesLabourData = new List<double?>();
                for (int i = 0; i < data.Count; i++)
                {
                    labelsData.Add(data[i].ProjectCategory);
                    seriesProjectsData.Add(data[i].TotalProjects);
                    seriesLabourData.Add(data[i].TotalLabour);
                }
                ProjectSeries = seriesProjectsData.ToArray();
                LabourSeries = seriesLabourData.ToArray();
                Labels = labelsData.ToArray();
                return Ok(new { StatusCode = HttpStatusCode.OK, ProjectSeries = ProjectSeries, Labels = Labels, LabourSeries= LabourSeries, TotalInvestments = totalInvestments, TotalProjects = totalProjects, TotalLabour = totalLabour, TotalInvestors = totalInvestors, LastUpdated = lastrecord.LastUpdated.ToString("MMMM dd, yyyy"), OmanizationRate = lastrecord.OmanizationRate });
            }
            return Ok(new { StatusCode = HttpStatusCode.OK, ProjectSeries = 0, Labels = 0, LabourSeries = 0,
                TotalInvestments = 0,
                TotalProjects = 0,
                TotalLabour = 0,
                TotalInvestors = 0,
                LastUpdated = DateTime.UtcNow.ToString("MMMM dd, yyyy"),
                OmanizationRate = 0
            });
        }
        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert(Statistic model)
        {
            try
            {
                model.StatisticId = Guid.NewGuid();
                model.LastUpdated = DateTime.UtcNow;
                _context.Add(model);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Record Created Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Statistic model)
        {
            try
            {
                var data = await _context.tblStatistics.Where(z => z.StatisticId == model.StatisticId).AsNoTracking().FirstOrDefaultAsync();
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Record not exists!" });
                }
                else
                {
                    data = model;
                    data.LastUpdated = DateTime.UtcNow;
                    _context.Entry(data).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Record Updated Successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var data = await _context.tblStatistics.FindAsync(id);
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Record not exists!" });
                }
                else
                {
                    _context.tblStatistics.Remove(data);
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Message = "Record Deleted!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}

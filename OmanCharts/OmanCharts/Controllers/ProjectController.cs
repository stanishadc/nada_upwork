using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmanCharts.Models;
using System.Net;

namespace OmanCharts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public ProjectController(RepositoryContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var data = await (from p in _context.Projects
                              join z in _context.Zones on p.ZoneId equals z.ZoneId
                              select new
                              {
                                  p.ProjectId,
                                  p.ProjectNumber,
                                  p.ProjectName,
                                  p.ProjectPeriod,
                                  p.Year,
                                  p.StartDateContract,
                                  p.EndDateContract,
                                  p.Challenges,
                                  p.ChangeRequestCost,
                                  p.CompletionDate,
                                  p.Consultant,
                                  p.Contractor,
                                  p.Cost,
                                  p.Expenses,
                                  p.FinanceEntity,
                                  p.IdentifiedCost,
                                  p.Progress,
                                  p.ProjectCategory,
                                  p.RemainingCost,
                                  p.RequiredAction,
                                  p.TimeExtension,
                                  z.ZoneId,
                                  z.ZoneName
                              }).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpGet]
        [Route("GetById/{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var data = await (from p in _context.Projects
                              join z in _context.Zones on p.ZoneId equals z.ZoneId
                              select new
                              {
                                  p.ProjectId,
                                  p.ProjectName,
                                  p.ProjectNumber,
                                  p.ProjectPeriod,
                                  p.Year,
                                  p.StartDateContract,
                                  p.EndDateContract,
                                  p.Challenges,
                                  p.ChangeRequestCost,
                                  p.CompletionDate,
                                  p.Consultant,
                                  p.Contractor,
                                  p.Cost,
                                  p.Expenses,
                                  p.FinanceEntity,
                                  p.IdentifiedCost,
                                  p.Progress,
                                  p.ProjectCategory,
                                  p.RemainingCost,
                                  p.RequiredAction,
                                  p.TimeExtension,
                                  z.ZoneId,
                                  z.ZoneName
                              }).Where(z => z.ProjectId == Id).FirstOrDefaultAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpGet]
        [Route("GetByZone")]
        public async Task<IActionResult> GetByZone(Guid ZoneId,Guid UserId)
        {
            var data = await (from p in _context.Projects
                              join z in _context.Zones on p.ZoneId equals z.ZoneId
                              select new
                              {
                                  p.ProjectId,
                                  p.ProjectNumber,
                                  p.ProjectName,
                                  p.ProjectPeriod,
                                  p.Year,
                                  p.StartDateContract,
                                  p.EndDateContract,
                                  p.Challenges,
                                  p.ChangeRequestCost,
                                  p.CompletionDate,
                                  p.Consultant,
                                  p.Contractor,
                                  p.Cost,
                                  p.Expenses,
                                  p.FinanceEntity,
                                  p.IdentifiedCost,
                                  p.Progress,
                                  p.ProjectCategory,
                                  p.RemainingCost,
                                  p.RequiredAction,
                                  p.TimeExtension,
                                  p.UserId,
                                  z.ZoneId,
                                  z.ZoneName
                              }).Where(z => z.ZoneId == ZoneId && z.UserId == UserId).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert(Project model)
        {
            try
            {
                model.ProjectId = Guid.NewGuid();
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
        public async Task<IActionResult> Update(Project model)
        {
            try
            {
                var data = await _context.Projects.Where(z => z.ProjectId == model.ProjectId).AsNoTracking().FirstOrDefaultAsync();
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Record not exists!" });
                }
                else
                {
                    data = model;
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
                var data = await _context.Projects.FindAsync(id);
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Record not exists!" });
                }
                else
                {
                    _context.Projects.Remove(data);
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Message = "Record Deleted!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetDashboard")]
        public IActionResult GetDashboard()
        {
            int[] Series = new int[4];
            string[] Labels = new string[4];
            List<string> strings = new List<string>();
            strings.Add("google1");
            strings.Add("google2");
            strings.Add("google3");
            strings.Add("google4");
            Labels = strings.ToArray();
            double totalInvestments = (double)(from num in _context.Projects select num.Cost).Sum();
            double totalProjects = (double)(from num in _context.Projects select num.Cost).Sum();
            return Ok(new { StatusCode = HttpStatusCode.OK, TotalInvestments = totalInvestments, TotalProjects = totalProjects, series = Series, labels = Labels });
        }
    }
}

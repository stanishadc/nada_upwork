using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmanCharts.Models;

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
            var data = await (from p in _context.Statistics
                              join z in _context.Zones on p.ZoneId equals z.ZoneId
                              select new
                              {
                                  p,
                                  z.ZoneName
                              }).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpGet]
        [Route("GetByZone")]
        public async Task<IActionResult> GetByZone(Guid ZoneId)
        {
            var data = await (from p in _context.Statistics
                              join z in _context.Zones on p.ZoneId equals z.ZoneId
                              select new
                              {
                                  p,
                                  z.ZoneId,
                                  z.ZoneName
                              }).Where(z => z.ZoneId == ZoneId).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert(Statistic model)
        {
            try
            {
                model.StatisticId = Guid.NewGuid();
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
                var data = await _context.Statistics.Where(z => z.StatisticId == model.StatisticId).FirstOrDefaultAsync();
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Record not exists!" });
                }
                else
                {
                    data = model;
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
                var data = await _context.Statistics.FindAsync(id);
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Record not exists!" });
                }
                else
                {
                    _context.Statistics.Remove(data);
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

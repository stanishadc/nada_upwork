using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmanCharts.Models;

namespace OmanCharts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public ZoneController(RepositoryContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var data = await (from bs in _context.Zones
                              select new
                              {
                                  bs.ZoneId,
                                  bs.ZoneName
                              }).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert(Zone model)
        {
            try
            {
                model.ZoneId = Guid.NewGuid();
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
        public async Task<IActionResult> Update(Zone model)
        {
            try
            {
                var data = await _context.Zones.Where(z => z.ZoneId == model.ZoneId).FirstOrDefaultAsync();
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Record not exists!" });
                }
                else
                {
                    data.ZoneName = model.ZoneName;
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
                var data = await _context.Zones.FindAsync(id);
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Record not exists!" });
                }
                else
                {
                    _context.Zones.Remove(data);
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Record Deleted!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}


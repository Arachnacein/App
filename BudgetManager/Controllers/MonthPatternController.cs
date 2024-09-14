using BudgetManager.Dto.MonthPattern;
using BudgetManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthPatternController : ControllerBase
    {
        private readonly IMonthPatternService _monthPatternService;

        public MonthPatternController(IMonthPatternService monthPatternService)
        {
            _monthPatternService = monthPatternService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var monthPattern = await _monthPatternService.Get(id);
                return Ok(monthPattern);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var monthPatterns = await _monthPatternService.GetAll();
                return Ok(monthPatterns);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddMonthPatternDto dto)
        {
            try
            {
                var monthpattern = await _monthPatternService.Add(dto);
                return Created($"api/monthpatterns/{monthpattern.Id}", monthpattern);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateMonthPatternDto dto)
        {
            try
            {
                await _monthPatternService.Update(dto);
                return NoContent();
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _monthPatternService.Delete(id);
                return NoContent();
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}
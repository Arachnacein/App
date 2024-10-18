using BudgetManager.Dto.MonthPattern;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Features.MonthPatterns.Queries;
using BudgetManager.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthPatternController : ControllerBase
    {
        private readonly IMonthPatternService _monthPatternService;
        private readonly IMediator _mediator;

        public MonthPatternController(IMonthPatternService monthPatternService, IMediator mediator)
        {
            _monthPatternService = monthPatternService;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var monthPattern = await _monthPatternService.RetrieveMonthPattern(id);
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
                var query = new RetrieveMonthPatternsQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
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
                var monthpattern = await _monthPatternService.AddMonthPattern(dto);
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
                await _monthPatternService.UpdateMonthPattern(dto);
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
                await _monthPatternService.DeleteMonthPattern(id);
                return NoContent();
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("GetMonthPattern")]
        public async Task<IActionResult> GetMonthPattern([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var pattern = await _monthPatternService.RetrieveMonthPattern(month, year);
                return Ok(pattern); 
            }            
            catch(PatternNotFoundException e)
            {
                return Conflict(e.Message);
            }              
            catch(MonthPatternAlreadyExistsException e)
            {
                return Conflict(e.Message);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);

            }
        }

        [HttpGet("GetAllWithPattern")]
        public async Task<IActionResult> GetAllWithPattern()
        {
            try
            {
                var monthPatterns = await _monthPatternService.RetrievePatterns();
                return Ok(monthPatterns);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}
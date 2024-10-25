using BudgetManager.Dto.MonthPattern;
using BudgetManager.Features.MonthPatterns.Commands;
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
            var query = new RetrieveMonthPatternQuery(id);
            var reponse = await _mediator.Send(query);
            return Ok(reponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new RetrieveMonthPatternsQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddMonthPatternDto dto)
        {
            var command = new SaveMonthPatternCommand(dto.Date, dto.PatternId);
            var response = await _mediator.Send(command);
            return Created($"api/monthpatterns/{response.Id}", response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMonthPatternDto dto)
        {
            var command = new UpdateMonthPatternCommand(dto.Id, dto.Date, dto.PatternId);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteMonthPatternCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("GetMonthPattern")]
        public async Task<IActionResult> GetMonthPattern([FromQuery] int month, [FromQuery] int year)
        {
            var query = new RetrieveMonthPatternByMonthAndByYearQuery(month, year);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAllWithPattern")]
        public async Task<IActionResult> GetAllWithPattern()
        {
            var query = new GetAllWithPatternQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
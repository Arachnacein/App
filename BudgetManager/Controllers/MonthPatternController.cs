using BudgetManager.Dto.MonthPattern;
using BudgetManager.Features.MonthPatterns.Commands;
using BudgetManager.Features.MonthPatterns.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,user")]
    public class MonthPatternController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MonthPatternController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/user/{userId}")]
        public async Task<IActionResult> Get(int id, Guid userId)
        {
            var query = new RetrieveMonthPatternQuery(id, userId);
            var reponse = await _mediator.Send(query);
            return Ok(reponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid userId)
        {
            var query = new RetrieveMonthPatternsQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddMonthPatternDto dto)
        {
            var command = new SaveMonthPatternCommand(dto.UserId, dto.Date, dto.PatternId);
            var response = await _mediator.Send(command);
            return Created($"api/monthpatterns/{response.Id}", response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMonthPatternDto dto)
        {
            var command = new UpdateMonthPatternCommand(dto.Id, dto.UserId, dto.Date, dto.PatternId);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}/user/{userId}")]
        public async Task<IActionResult> Delete(int id, Guid userId)
        {
            var command = new DeleteMonthPatternCommand(id, userId);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("GetMonthPattern")]
        public async Task<IActionResult> GetMonthPattern([FromQuery] int month, [FromQuery] int year, [FromQuery] Guid userId)
        {
            var query = new RetrieveMonthPatternByMonthAndByYearQuery(userId, month, year);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAllWithPattern")]
        public async Task<IActionResult> GetAllWithPattern([FromQuery] Guid userId)
        {
            var query = new GetAllWithPatternQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
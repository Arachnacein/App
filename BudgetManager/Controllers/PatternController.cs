using BudgetManager.Dto.Pattern;
using BudgetManager.Features.Patterns.Commands;
using BudgetManager.Features.Patterns.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UserPolicy")]
    public class PatternController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatternController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid userId)
        {
            var query = new RetrievePatternsQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}/user/{userId}")]
        public async Task<IActionResult> GetById(int id, Guid userId)
        {
            var query = new RetrievePatternQuery(id, userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPatternDto dto)
        {
            var command = new SavePatternCommand(dto.UserId, dto.Name, dto.Value_Saves, dto.Value_Fees, dto.Value_Entertainment);
            var response = await _mediator.Send(command);
            return Created($"api/patterns/{response.Id}", response);
        }

        [HttpDelete("{id}/user/{userId}")]
        public async Task<IActionResult> Delete(int id, Guid userId)
        {
            var command = new DeletePatternCommand(id, userId);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
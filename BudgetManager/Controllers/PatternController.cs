using BudgetManager.Dto.Pattern;
using BudgetManager.Features.Patterns.Commands;
using BudgetManager.Features.Patterns.Queries;
using BudgetManager.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatternController : ControllerBase
    {
        private readonly IPatternService _patternService;
        private readonly IMediator _mediator;

        public PatternController(IPatternService patternService, IMediator mediator)
        {
            _patternService = patternService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new RetrievePatternsQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new RetrievePatternQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPatternDto dto)
        {
            var command = new SavePatternCommand(dto.Name, dto.Value_Saves, dto.Value_Fees, dto.Value_Entertainment);
            var response = await _mediator.Send(command);
            return Created($"api/patterns/{response.Id}", response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeletePatternCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
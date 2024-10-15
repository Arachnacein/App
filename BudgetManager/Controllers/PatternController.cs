using BudgetManager.Dto.Pattern;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.PatternExceptions;
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
            try
            {
                var query = new RetrievePatternsQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var query = new RetrievePatternQuery(id);
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddPatternDto dto)
        {
            try
            {
                var pattern = await _patternService.AddPattern(dto);
                return Created($"api/patterns/{pattern.Id}", pattern);
            }
            catch(ArgumentNullException e)
            {
                return Conflict(e.Message);
            }
            catch(BadStringLengthException e)
            {
                return Conflict(e.Message);
            }           
            catch(BadValueException e)
            {
                return Conflict(e.Message);
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
                await _patternService.DeletePattern(id);
                return NoContent();
            }
            catch(PatternNotFoundException e)
            {
                return Conflict(e.Message);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}

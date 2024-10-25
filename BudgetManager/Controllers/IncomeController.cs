using BudgetManager.Dto.Income;
using BudgetManager.Features.Incomes.Commands;
using BudgetManager.Features.Incomes.Queries;
using BudgetManager.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IMediator _mediator;

        public IncomeController(IIncomeService incomeService, IMediator mediator)
        {
            _incomeService = incomeService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new RetrieveIncomesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new RetrieveIncomeQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddIncomeDto dto)
        {
            var command = new SaveIncomeCommand(dto.Name, dto.Amount, dto.Date);
            var result = await _mediator.Send(command);

            return Created($"api/incomes/{result.Id}", result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateIncomeDto dto)
        {
            var command = new UpdateIncomeCommand(dto.Id, dto.Name, dto.Amount, dto.Date);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteIncomeCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("GetIncome")]
        public async Task<IActionResult> GetIncome([FromQuery] int month, [FromQuery] int year)
        {
            var query = new RetrieveMonthIncomeQuery(month, year);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
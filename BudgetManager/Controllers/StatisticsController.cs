using BudgetManager.Features.Statistics.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetTotalExpenses")]
        public async Task<IActionResult> GetTotalExpenses()
        {
            var query = new RetrieveTotalExpensesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetTotalSaves")]
        public async Task<IActionResult> GetTotalSaves()
        {
            var query = new RetrieveTotalSavesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("GetAverageExpenses")]
        public async Task<IActionResult> GetAverageExpenses()
        {
            var query = new RetrieveAverageExpenses();
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
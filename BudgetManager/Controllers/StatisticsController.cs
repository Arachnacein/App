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
            var query = new RetrieveAverageExpensesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }  
        
        [HttpGet("GetAverageSaves")]
        public async Task<IActionResult> GetAverageSaves()
        {
            var query = new RetrieveAverageSavesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }        
        
        [HttpGet("GetThreeMonthsSaves")]
        public async Task<IActionResult> GetThreeMonthsSaves()
        {
            var query = new RetrieveThreeMonthsSavesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }        
        
        [HttpGet("GetThreeMonthsExpenses")]
        public async Task<IActionResult> GetThreeMonthsExpenses()
        {
            var query = new RetrieveThreeMonthsExpensesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }        
        
        [HttpGet("GetCategoriesDistribution")]
        public async Task<IActionResult> GetCategoriesDistribution()
        {
            var query = new RetrieveCategoriesDistributionQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet("GetMonthlyCategoriesDistribution")]
        public async Task<IActionResult> GetMonthlyCategoriesDistribution()
        {
            var query = new RetrieveMonthlyCategoriesDistributionQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
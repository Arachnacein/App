using BudgetManager.Features.Statistics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UserPolicy")]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetTotalExpenses")]
        public async Task<IActionResult> GetTotalExpenses([FromQuery] Guid userId)
        {
            var query = new RetrieveTotalExpensesQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetTotalSaves")]
        public async Task<IActionResult> GetTotalSaves([FromQuery] Guid userId)
        {
            var query = new RetrieveTotalSavesQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetAverageExpenses")]
        public async Task<IActionResult> GetAverageExpenses([FromQuery] Guid userId)
        {
            var query = new RetrieveAverageExpensesQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }  
        
        [HttpGet("GetAverageSaves")]
        public async Task<IActionResult> GetAverageSaves([FromQuery] Guid userId)
        {
            var query = new RetrieveAverageSavesQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }        
        
        [HttpGet("GetThreeMonthsSaves")]
        public async Task<IActionResult> GetThreeMonthsSaves([FromQuery] Guid userId)
        {
            var query = new RetrieveThreeMonthsSavesQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }        
        
        [HttpGet("GetThreeMonthsExpenses")]
        public async Task<IActionResult> GetThreeMonthsExpenses([FromQuery] Guid userId)
        {
            var query = new RetrieveThreeMonthsExpensesQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }        
        
        [HttpGet("GetCategoriesDistribution")]
        public async Task<IActionResult> GetCategoriesDistribution([FromQuery] Guid userId)
        {
            var query = new RetrieveCategoriesDistributionQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet("GetMonthlyCategoriesDistribution")]
        public async Task<IActionResult> GetMonthlyCategoriesDistribution([FromQuery] Guid userId)
        {
            var query = new RetrieveMonthlyCategoriesDistributionQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
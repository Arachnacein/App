namespace BudgetManager.Controllers;

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
    public async Task<IActionResult> GetMonthlyCategoriesDistribution([FromQuery] Guid userId, [FromQuery] int? year = null)
    {
        var query = new RetrieveMonthlyCategoriesDistributionQuery(userId, year);
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetSavingsRate")]
    public async Task<IActionResult> GetSavingsRate([FromQuery] Guid userId)
    {
        var query = new RetrieveSavingsRateQuery(userId);
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetTransactionCountByCategory")]
    public async Task<IActionResult> GetTransactionCountByCategory([FromQuery] Guid userId)
    {
        var query = new RetrieveTransactionCountByCategoryQuery(userId);
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetAvailableYears")]
    public async Task<IActionResult> GetAvailableYears([FromQuery] Guid userId)
    {
        var query = new RetrieveAvailableYearsQuery(userId);
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetFilteredStatistics")]
    public async Task<IActionResult> GetFilteredStatistics([FromQuery] Guid userId, [FromQuery] int? year)
    {
        var query = new RetrieveFilteredStatisticsQuery(userId, year);
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}
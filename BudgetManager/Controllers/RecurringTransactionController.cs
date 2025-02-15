using BudgetManager.Dto.RecurringTransaction;
using BudgetManager.Features.RecurringTransactions.Commands;
using BudgetManager.Features.RecurringTransactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecurringTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RecurringTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/user/{userId}")]
        public async Task<IActionResult> GetById(int id, Guid userId)
        {
            var query = new RetrieveRecurringTransactionQuery(id, userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }        

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var query = new RetrieveRecurringTransactionsQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddRecurringTransactionDto dto)
        {
            var command = new SaveRecurringTransactionCommand(dto.UserId, dto.Name, dto.Description, dto.Amount, 
                                                              dto.TransactionType, dto.StartDate, dto.EndDate, 
                                                              dto.Approved, dto.Frequency, dto.Interval, dto.WeeklyDays, 
                                                              dto.MonthlyDay, dto.YearlyMonth, dto.YearlyDay, dto.MaxOccurrences);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRecurringTransactionDto dto)
        {
            var command = new UpdateRecurringTransactionCommand(dto.Id, dto.UserId, dto.Name, dto.Description, dto.Amount, 
                                                                dto.TransactionType, dto.StartDate, dto.EndDate, dto.Approved, 
                                                                dto.Frequency, dto.Interval, dto.WeeklyDays,
                                                                dto.MonthlyDay, dto.YearlyMonth, dto.YearlyDay, dto.MaxOccurrences);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}/user/{userId}")]
        public async Task<IActionResult> Delete(int id, Guid userId)
        {
            var command = new DeleteRecurringTransactionCommand(id, userId);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
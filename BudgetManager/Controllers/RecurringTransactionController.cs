using BudgetManager.Dto.RecurringTransaction;
using BudgetManager.Features.RecurringTransactions.Commands;
using BudgetManager.Features.RecurringTransactions.Queries;
using BudgetManager.Models;
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
            var schedule = new RecurringTransactionSchedule
            {
                Frequency = dto.Schedule.Frequency,
                Interval = dto.Schedule.Interval,
                WeeklyDays = dto.Schedule.WeeklyDays,
                MonthlyDay = dto.Schedule.MonthlyDay,
                YearlyMonth = dto.Schedule.YearlyMonth,
                YearlyDay = dto.Schedule.YearlyDay,
                MaxOccurrences = dto.Schedule.MaxOccurrences,
                RecurringTransactionId = dto.Schedule.RecurringTransactionId
            };
            var command = new SaveRecurringTransactionCommand(dto.UserId, dto.Name, dto.Description, dto.Amount, dto.TransactionType, dto.StartDate, dto.EndDate, dto.Approved, dto.ScheduleId, schedule);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRecurringTransactionDto dto)
        {
            var schedule = new RecurringTransactionSchedule
            {
                Frequency = dto.Schedule.Frequency,
                Interval = dto.Schedule.Interval,
                WeeklyDays = dto.Schedule.WeeklyDays,
                MonthlyDay = dto.Schedule.MonthlyDay,
                YearlyMonth = dto.Schedule.YearlyMonth,
                YearlyDay = dto.Schedule.YearlyDay,
                MaxOccurrences = dto.Schedule.MaxOccurrences,
                RecurringTransactionId = dto.Schedule.RecurringTransactionId
            };
            var command = new UpdateRecurringTransactionCommand(dto.Id, dto.UserId, dto.Name, dto.Description, dto.Amount, dto.TransactionType, dto.StartDate, dto.EndDate, dto.Approved, dto.ScheduleId, schedule);
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
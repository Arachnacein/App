using BudgetManager.Features.RecurringTransactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
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
    }
}
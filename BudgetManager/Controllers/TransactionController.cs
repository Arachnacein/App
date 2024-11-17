using BudgetManager.Dto.Transaction;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.TransactionExceptions;
using BudgetManager.Features.Transactions.Commands;
using BudgetManager.Features.Transactions.Queries;
using BudgetManager.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMediator _mediator;

        public TransactionController(ITransactionService transactionService, IMediator mediator)
        {
            _transactionService = transactionService;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new RetrieveTransactionQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new RetrieveTransactionsQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTransactionDto dto)
        {
            var command = new SaveTransactionCommand(dto.Name, dto.Description, dto.Date, dto.Price, dto.Category);
            var response = await _mediator.Send(command);        
            return Created($"api/transactions/{response.Id}", response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTransactionDto dto)
        {
            var command = new UpdateTransactionCommand(dto.Id, dto.Name, dto.Description, dto.Date, dto.Price, dto.Category);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, Guid userId)
        {
            var command = new DeleteTransactionCommand(id, userId);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateTransactionCategoryDto dto)
        {
            var command = new UpdateCategoryCommand(dto.Id, dto.Category);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
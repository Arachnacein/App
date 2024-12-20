using BudgetManager.Dto.Transaction;
using BudgetManager.Features.Transactions.Commands;
using BudgetManager.Features.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UserPolicy")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/user/{userId}")]
        public async Task<IActionResult> GetById(int id, Guid userId)
        {
            var query = new RetrieveTransactionQuery(id, userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid userId)
        {
            var query = new RetrieveTransactionsQuery(userId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTransactionDto dto)
        {
            var command = new SaveTransactionCommand(dto.Name, dto.Description, dto.Date, dto.Price, dto.Category, dto.UserId);
            var response = await _mediator.Send(command);        
            return Created($"api/transactions/{response.Id}", response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTransactionDto dto)
        {
            var command = new UpdateTransactionCommand(dto.Id, dto.UserId, dto.Name, dto.Description, dto.Date, dto.Price, dto.Category);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}/user/{userId}")]
        public async Task<IActionResult> Delete(int id, Guid userId)
        {
            var command = new DeleteTransactionCommand(id, userId);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateTransactionCategoryDto dto)
        {
            var command = new UpdateCategoryCommand(dto.Id, dto.UserId, dto.Category);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
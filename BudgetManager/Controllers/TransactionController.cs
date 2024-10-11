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
            try
            {
                var transaction = await _transactionService.RetrieveTransaction(id);
                return Ok(transaction);
            }

            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var query = new RetrieveTransactionsQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTransactionDto dto)
        {
            try
            {
                var command = new SaveTransactionCommand(dto.Name, dto.Description, dto.Date, dto.Price, dto.Category);
                var response = await _mediator.Send(command);        
                return Created($"api/transactions/{response.Id}", response);
            }
            catch(NullPointerException e)
            {
                return Conflict(e.Message);
            }            
            catch(BadStringLengthException e)
            {
                return Conflict(e.Message);
            }            
            catch(BadValueException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTransactionDto dto)
        {
            try
            {
                await _transactionService.UpdateTransaction(dto);
                return NoContent();
            }
            catch (NullPointerException e)
            {
                return Conflict(e.Message);
            }
            catch (BadStringLengthException e)
            {
                return Conflict(e.Message);
            }
            catch (BadValueException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _transactionService.DeleteTransaction(id);
                return NoContent();
            }
            catch(NullPointerException e)
            {
                return Conflict(e.Message);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(UpdateTransactionCategoryDto dto)
        {
            try
            {
                await _transactionService.UpdateCategory(dto);
                return NoContent();
            }
            catch (TransactionNotFoundException e)
            {
                return Conflict(e.Message);
            }
            catch (BadValueException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}

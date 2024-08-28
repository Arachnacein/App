using BudgetManager.Dto;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.TransactionExceptions;
using BudgetManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public BudgetController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var transaction = _transactionService.RetrieveTransaction(id);
                return Ok(transaction);
            }

            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var transactions = _transactionService.RetrieveTransactions();
                return Ok(transactions);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(AddTransactionDto dto)
        {
            try
            {
                Console.WriteLine("------------------------------------------------------");
                var transaction = _transactionService.AddTransaction(dto);
                return Created($"api/transactions/{transaction.Id}", transaction);
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
        public IActionResult Update(UpdateTransactionDto dto)
        {
            try
            {
                _transactionService.UpdateTransaction(dto);
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
        public IActionResult Delete(int id)
        {
            try
            {
                _transactionService.DeleteTransaction(id);
                return NoContent();
            }
            catch(TransactionNotFoundException e)
            {
                return Conflict(e.Message);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}

﻿using BudgetManager.Dto.Transaction;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.TransactionExceptions;
using BudgetManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
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
                var transactions = await _transactionService.RetrieveTransactions();
                return Ok(transactions);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddTransactionDto dto)
        {
            try
            {
                var transaction = await _transactionService.AddTransaction(dto);
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

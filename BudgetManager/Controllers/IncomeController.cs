using BudgetManager.Dto.Income;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.IncomeExceptions;
using BudgetManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var incomes = await _incomeService.RetrieveIncomes();
                return Ok(incomes);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var incomes = await _incomeService.RetrieveIncomes();
                return Ok(incomes);
            }
            catch (IncomeNotFoundException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddIncomeDto dto)
        {
            try
            {
                var income = await _incomeService.AddIncome(dto);
                return Created($"api/incomes/{income.Id}", income);
            }
            catch(BadStringLengthException e)
            {
                return Conflict(e.Message);
            }         
            catch(BadValueException e)
            {
                return Conflict(e.Message);
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

        [HttpPut]
        public async Task<IActionResult> Update(UpdateIncomeDto dto)
        {
            try
            {
                await _incomeService.UpdateIncome(dto);
                return NoContent();
            }
            catch (BadStringLengthException e)
            {
                return Conflict(e.Message);
            }
            catch (BadValueException e)
            {
                return Conflict(e.Message);
            }
            catch (NullPointerException e)
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
                await _incomeService.DeleteIncome(id);
                return NoContent();
            }
            catch(IncomeNotFoundException e)
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
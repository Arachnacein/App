using BudgetManager.Dto;
using BudgetManager.Dto.Income;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.IncomeExceptions;
using BudgetManager.Features.Incomes.Queries;
using BudgetManager.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IMediator _mediator;

        public IncomeController(IIncomeService incomeService, IMediator mediator)
        {
            _incomeService = incomeService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var query = new RetrieveIncomesQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
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
                var query = new RetrieveIncomeQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
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

        [HttpGet("GetIncome")]
        public async Task<IActionResult> GetIncome([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var incomes = await _incomeService.RetrieveIncomes(month, year);
                return Ok(incomes);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}
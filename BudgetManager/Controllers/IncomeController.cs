using BudgetManager.Dto.Income;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.IncomeExceptions;
using BudgetManager.Features.Incomes.Commands;
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
        public async Task<IActionResult> Create([FromBody] AddIncomeDto dto)
        {
            try
            {
                var command = new SaveIncomeCommand(dto.Name, dto.Amount, dto.Date);
                var result = await _mediator.Send(command);

                return Created($"api/incomes/{result.Id}", result);
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
        public async Task<IActionResult> Update([FromBody] UpdateIncomeDto dto)
        {
            try
            {
                var command = new UpdateIncomeCommand(dto.Id, dto.Name, dto.Amount, dto.Date);
                await _mediator.Send(command);
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
                var command = new DeleteIncomeCommand(id);
                await _mediator.Send(command);
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
                var query = new RetrieveMonthIncomeQuery(month, year);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}
using BudgetManager.Dto.Income;
using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Incomes.Commands
{
    public record SaveIncomeCommand : IRequest<IncomeDto>
    {
        public string Name { get; init; }
        public double Amount { get; init; }
        public DateTime Date { get; init; }
        public SaveIncomeCommand(string name, double amount, DateTime date)
        {
            Name = name;
            Amount = amount;
            Date = date;
        }
    }
    public class SaveIncomeCommandHandler : IRequestHandler<SaveIncomeCommand, IncomeDto>
    {
        private readonly IIncomeService _incomeService;
        private readonly IIncomeMapper _incomeMapper;

        public SaveIncomeCommandHandler(IIncomeService incomeService, IIncomeMapper incomeMapper)
        {
            _incomeService = incomeService;
            _incomeMapper = incomeMapper;
        }

        public async Task<IncomeDto> Handle(SaveIncomeCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            return await _incomeService.AddIncome(_incomeMapper.Map(request));
        }
    }
}
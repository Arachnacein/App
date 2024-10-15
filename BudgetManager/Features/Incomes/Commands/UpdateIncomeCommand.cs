using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Incomes.Commands
{
    public record UpdateIncomeCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public UpdateIncomeCommand(int id, string name, double amount, DateTime date)
        {
            Id = id;
            Name = name;
            Amount = amount;
            Date = date;
        }
    }
    public class UpdateIncomeCommandHandler : IRequestHandler<UpdateIncomeCommand>
    {
        private readonly IIncomeService _incomeService;
        private readonly IIncomeMapper _incomeMapper;
        public UpdateIncomeCommandHandler(IIncomeService incomeService, IIncomeMapper incomeMapper)
        {
            _incomeService = incomeService;
            _incomeMapper = incomeMapper;
        }

        public async Task Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _incomeService.UpdateIncome(_incomeMapper.Map(request));
        }
    }
}
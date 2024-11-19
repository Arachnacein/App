using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Incomes.Commands
{
    public record UpdateIncomeCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; init; }
        public double Amount { get; init; }
        public DateTime Date { get; init; }
        public UpdateIncomeCommand(int id, Guid userId, string name, double amount, DateTime date)
        {
            Id = id;
            UserId = userId;
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
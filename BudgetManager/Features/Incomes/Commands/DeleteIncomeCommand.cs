﻿using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Incomes.Commands
{
    public record DeleteIncomeCommand : IRequest
    {
        public int Id { get; init; }
        public DeleteIncomeCommand(int id)
        {
            Id = id;
        }
    }
    public class DeleteIncomeCommandHandler : IRequestHandler<DeleteIncomeCommand>
    {
        private readonly IIncomeService _incomeService;
        public DeleteIncomeCommandHandler(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        public async Task Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            await _incomeService.DeleteIncome(request.Id);
        }
    }
}
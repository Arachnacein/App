﻿using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Transactions.Commands
{
    public class UpdateTransactionCommand : IRequest    
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public DateTime Date { get; init; }
        public double Price { get; init; }
        public TransactionCategoryEnum Category { get; set; }

        public UpdateTransactionCommand(int id, string name, string? description, DateTime date, double price, TransactionCategoryEnum category)
        {
            Id = id;
            Name = name;
            Description = description;
            Date = date;
            Price = price;
            Category = category;
        }
    }

    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionMapper _transactionMapper;

        public UpdateTransactionCommandHandler(ITransactionService transactionService, ITransactionMapper transactionMapper)
        {
            _transactionService = transactionService;
            _transactionMapper = transactionMapper;
        }
        public async Task Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _transactionService.UpdateTransaction(_transactionMapper.Map(request));
        }
    }
}
﻿using BudgetManager.Dto.Transaction;
using BudgetManager.Features.Transactions.Commands;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public interface ITransactionMapper
    {
        TransactionDto Map(Transaction source);
        AddTransactionDto Map(SaveTransactionCommand command);
        UpdateTransactionDto Map(UpdateTransactionCommand command);
        UpdateTransactionCategoryDto Map(UpdateCategoryCommand command);
        ConfirmTransactionDto Map(ConfirmTransactionCommand command);
        Transaction Map(TransactionDto source);
        Transaction Map(AddTransactionDto source);
        Transaction Map(UpdateTransactionDto source);
        ICollection<TransactionDto> MapElements(ICollection<Transaction> source);
        ICollection<Transaction> MapElements(ICollection<TransactionDto> source);
    }
    public class TransactionMapper : ITransactionMapper
    {
        public TransactionDto Map(Transaction source)
        {
            var destination = new TransactionDto();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Date = source.Date;
            destination.Price = source.Price;
            destination.Category = source.Category;
            destination.IsApproved = source.IsApproved;
            destination.IsRecurring = source.IsRecurring;

            return destination;
        }

        public AddTransactionDto Map(SaveTransactionCommand command)
        {
            var destination = new AddTransactionDto();
            destination.UserId = command.UserId;
            destination.Name = command.Name;
            destination.Description = command.Description;
            destination.Date = command.Date;
            destination.Price = command.Price;
            destination.Category = command.Category;
            destination.IsApproved = command.IsApproved;
            destination.IsRecurring = command.IsRecurring;

            return destination;
        }

        public UpdateTransactionDto Map(UpdateTransactionCommand command)
        {
            var destination = new UpdateTransactionDto();
            destination.Id = command.Id;
            destination.UserId = command.UserId;
            destination.Name = command.Name;
            destination.Description = command.Description;
            destination.Date = command.Date;
            destination.Price = command.Price;
            destination.Category = command.Category;
            destination.IsApproved = command.IsApproved;
            destination.IsRecurring = command.IsRecurring;

            return destination;
        }
        public UpdateTransactionCategoryDto Map(UpdateCategoryCommand command)
        {
            var destination = new UpdateTransactionCategoryDto();
            destination.Id = command.Id;
            destination.UserId = command.UserId;
            destination.Category = command.Category;

            return destination;
        }
        public ConfirmTransactionDto Map(ConfirmTransactionCommand command)
        {
            var destination = new ConfirmTransactionDto();
            destination.Id = command.Id;
            destination.UserId = command.UserId;

            return destination;
        }
        public Transaction Map(TransactionDto source)
        {
            var destination = new Transaction();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Date = source.Date;
            destination.Price = source.Price;
            destination.Category = source.Category;
            destination.IsApproved = source.IsApproved;
            destination.IsRecurring = source.IsRecurring;

            return destination;
        }

        public Transaction Map(AddTransactionDto source)
        {
            var destination = new Transaction();
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Date = source.Date;
            destination.Price = source.Price;
            destination.Category = source.Category;
            destination.IsApproved = source.IsApproved;
            destination.IsRecurring = source.IsRecurring;

            return destination;
        }

        public Transaction Map(UpdateTransactionDto source)
        {
            var destination = new Transaction();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Date = source.Date;
            destination.Price = source.Price;
            destination.Category = source.Category;
            destination.IsApproved = source.IsApproved;
            destination.IsRecurring = source.IsRecurring;

            return destination;
        }        

        public ICollection<TransactionDto> MapElements(ICollection<Transaction> source)
        {
            List<TransactionDto> destination = new List<TransactionDto>();

            foreach (var transaction in source)
                destination.Add(Map(transaction));

            return destination;
        }

        public ICollection<Transaction> MapElements(ICollection<TransactionDto> source)
        {
            List<Transaction> destination = new List<Transaction>();

            foreach (var transaction in source)
                destination.Add(Map(transaction));

            return destination;
        }
    }
}
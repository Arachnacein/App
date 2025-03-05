using BudgetManager.Dto.RecurringTransaction;
using BudgetManager.Features.RecurringTransactions.Commands;
using BudgetManager.Features.Transactions.Commands;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public interface IRecurringTransactionMapper
    {
        RecurringTransaction Map(RecurringTransactionDto source);
        RecurringTransactionDto Map(RecurringTransaction source);
        RecurringTransaction Map(AddRecurringTransactionDto source);
        RecurringTransaction Map(UpdateRecurringTransactionDto source);
        AddRecurringTransactionDto Map(SaveRecurringTransactionCommand source);
        UpdateRecurringTransactionDto Map(UpdateRecurringTransactionCommand source);
        ICollection<RecurringTransactionDto> MapElements(ICollection<RecurringTransaction> source);
        ICollection<RecurringTransaction> MapElements(ICollection<RecurringTransactionDto> source);
    }
    public class RecurringTransactionMapper : IRecurringTransactionMapper
    {
        public RecurringTransaction Map(RecurringTransactionDto source)
        {
            var destination = new RecurringTransaction();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Amount = source.Amount;
            destination.TransactionType = source.TransactionType;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.Frequency = source.Frequency;
            destination.Interval = source.Interval;
            destination.WeeklyDays = source.WeeklyDays;
            destination.MonthlyDay = source.MonthlyDay;
            destination.YearlyMonth = source.YearlyMonth;
            destination.YearlyDay = source.YearlyDay;
            destination.MaxOccurrences = source.MaxOccurrences;

            return destination;
        }

        public RecurringTransactionDto Map(RecurringTransaction source)
        {
            var destination = new RecurringTransactionDto();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Amount = source.Amount;
            destination.TransactionType = source.TransactionType;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.Frequency = source.Frequency;
            destination.Interval = source.Interval;
            destination.WeeklyDays = source.WeeklyDays;
            destination.MonthlyDay = source.MonthlyDay;
            destination.YearlyMonth = source.YearlyMonth;
            destination.YearlyDay = source.YearlyDay;
            destination.MaxOccurrences = source.MaxOccurrences;

            return destination;
        }

        public RecurringTransaction Map(AddRecurringTransactionDto source)
        {
            var destination = new RecurringTransaction();
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Amount = source.Amount;
            destination.TransactionType = source.TransactionType;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.Frequency = source.Frequency;
            destination.Interval = source.Interval;
            destination.WeeklyDays = source.WeeklyDays;
            destination.MonthlyDay = source.MonthlyDay;
            destination.YearlyMonth = source.YearlyMonth;
            destination.YearlyDay = source.YearlyDay;
            destination.MaxOccurrences = source.MaxOccurrences; 
            
            return destination;
        }

        public RecurringTransaction Map(UpdateRecurringTransactionDto source)
        {
            var destination = new RecurringTransaction();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Amount = source.Amount;
            destination.TransactionType = source.TransactionType;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.Frequency = source.Frequency;
            destination.Interval = source.Interval;
            destination.WeeklyDays = source.WeeklyDays;
            destination.MonthlyDay = source.MonthlyDay;
            destination.YearlyMonth = source.YearlyMonth;
            destination.YearlyDay = source.YearlyDay;
            destination.MaxOccurrences = source.MaxOccurrences;

            return destination;
        }

        public AddRecurringTransactionDto Map(SaveRecurringTransactionCommand source)
        {
            var destination = new AddRecurringTransactionDto();
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Amount = source.Amount;
            destination.TransactionType = source.TransactionType;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.Frequency = source.Frequency;
            destination.Interval = source.Interval;
            destination.WeeklyDays = source.WeeklyDays;
            destination.MonthlyDay = source.MonthlyDay;
            destination.YearlyMonth = source.YearlyMonth;
            destination.YearlyDay = source.YearlyDay;
            destination.MaxOccurrences = source.MaxOccurrences;

            return destination;
        }

        public UpdateRecurringTransactionDto Map(UpdateRecurringTransactionCommand source)
        {
            var destination = new UpdateRecurringTransactionDto();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Amount = source.Amount;
            destination.TransactionType = source.TransactionType;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.Frequency = source.Frequency;
            destination.Interval = source.Interval;
            destination.WeeklyDays = source.WeeklyDays;
            destination.MonthlyDay = source.MonthlyDay;
            destination.YearlyMonth = source.YearlyMonth;
            destination.YearlyDay = source.YearlyDay;
            destination.MaxOccurrences = source.MaxOccurrences;

            return destination;
        }

        public ICollection<RecurringTransactionDto> MapElements(ICollection<RecurringTransaction> source)
        {
            var destination = new List<RecurringTransactionDto>();
            foreach (var item in source)
                destination.Add(Map(item));
            return destination;
        }

        public ICollection<RecurringTransaction> MapElements(ICollection<RecurringTransactionDto> source)
        {
            var destination = new List<RecurringTransaction>();
            foreach (var item in source)
                destination.Add(Map(item));
            return destination;
        }
    }
}
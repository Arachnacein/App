using BudgetManager.Dto.RecurringTransaction;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public interface IRecurringTransactionMapper
    {
        RecurringTransaction Map(RecurringTransactionDto source);
        RecurringTransactionDto Map(RecurringTransaction source);
        RecurringTransaction Map(AddRecurringTransactionDto source);
        RecurringTransaction Map(UpdateRecurringTransactionDto source);
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
            destination.Approved = source.Approved;
            destination.ScheduleId = source.ScheduleId;

            destination.Schedule.Id = source.Schedule.Id;
            destination.Schedule.Frequency = source.Schedule.Frequency;
            destination.Schedule.Interval = source.Schedule.Interval;
            destination.Schedule.WeeklyDays = source.Schedule.WeeklyDays;
            destination.Schedule.MonthlyDay = source.Schedule.MonthlyDay;
            destination.Schedule.YearlyMonth = source.Schedule.YearlyMonth;
            destination.Schedule.YearlyDay = source.Schedule.YearlyDay;
            destination.Schedule.MaxOccurrences = source.Schedule.MaxOccurrences;
            destination.Schedule.RecurringTransactionId = source.Schedule.RecurringTransactionId;
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
            destination.Approved = source.Approved;
            destination.ScheduleId = source.ScheduleId;

            destination.Schedule.Id = source.Schedule.Id;
            destination.Schedule.Frequency = source.Schedule.Frequency;
            destination.Schedule.Interval = source.Schedule.Interval;
            destination.Schedule.WeeklyDays = source.Schedule.WeeklyDays;
            destination.Schedule.MonthlyDay = source.Schedule.MonthlyDay;
            destination.Schedule.YearlyMonth = source.Schedule.YearlyMonth;
            destination.Schedule.YearlyDay = source.Schedule.YearlyDay;
            destination.Schedule.MaxOccurrences = source.Schedule.MaxOccurrences;
            destination.Schedule.RecurringTransactionId = source.Schedule.RecurringTransactionId;
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
            destination.Approved = source.Approved;
            destination.ScheduleId = source.ScheduleId;

            destination.Schedule.Id = source.Schedule.Id;
            destination.Schedule.Frequency = source.Schedule.Frequency;
            destination.Schedule.Interval = source.Schedule.Interval;
            destination.Schedule.WeeklyDays = source.Schedule.WeeklyDays;
            destination.Schedule.MonthlyDay = source.Schedule.MonthlyDay;
            destination.Schedule.YearlyMonth = source.Schedule.YearlyMonth;
            destination.Schedule.YearlyDay = source.Schedule.YearlyDay;
            destination.Schedule.MaxOccurrences = source.Schedule.MaxOccurrences;
            destination.Schedule.RecurringTransactionId = source.Schedule.RecurringTransactionId;
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
            destination.Approved = source.Approved;
            destination.ScheduleId = source.ScheduleId;

            destination.Schedule.Id = source.Schedule.Id;
            destination.Schedule.Frequency = source.Schedule.Frequency;
            destination.Schedule.Interval = source.Schedule.Interval;
            destination.Schedule.WeeklyDays = source.Schedule.WeeklyDays;
            destination.Schedule.MonthlyDay = source.Schedule.MonthlyDay;
            destination.Schedule.YearlyMonth = source.Schedule.YearlyMonth;
            destination.Schedule.YearlyDay = source.Schedule.YearlyDay;
            destination.Schedule.MaxOccurrences = source.Schedule.MaxOccurrences;
            destination.Schedule.RecurringTransactionId = source.Schedule.RecurringTransactionId;
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
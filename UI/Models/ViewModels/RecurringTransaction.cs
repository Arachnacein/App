﻿namespace UI.Models.ViewModels
{
    public class RecurringTransaction
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Approved { get; set; }

        public int ScheduleId { get; set; }
        public RecurringTransactionSchedule Schedule { get; set; }
    }
    public class RecurringTransactionSchedule
    {
        public int Id { get; set; }
        public FrequencyEnum Frequency { get; set; }
        public int Interval { get; set; }
        public List<DayOfWeek>? WeeklyDays { get; set; }
        public int? MonthlyDay { get; set; }
        public int? YearlyMonth { get; set; }
        public int? YearlyDay { get; set; }
        public int? MaxOccurrences { get; set; }

        public int RecurringTransactionId { get; set; }
        public RecurringTransaction RecurringTransaction { get; set; }
    }
}
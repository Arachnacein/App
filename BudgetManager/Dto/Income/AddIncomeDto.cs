﻿namespace BudgetManager.Dto.Income
{
    public class AddIncomeDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
﻿namespace BudgetManager.Dto.Pattern
{
    public class AddPatternDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Value_Saves { get; set; }
        public double Value_Fees { get; set; }
        public double Value_Entertainment { get; set; }
    }
}
﻿using BudgetManager.Dto.MonthPattern;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public class MonthPatternMapper : IMonthPatternMapper
    {
        public MonthPattern Map(MonthPatternDto source)
        {
            var destination = new MonthPattern();
            destination.Id = source.Id;
            destination.PatternId = source.PatternId;
            destination.Date = source.Date;

            return destination;
        }

        public MonthPattern Map(AddMonthPatternDto source)
        {
            var destination = new MonthPattern();
            destination.PatternId = source.PatternId;
            destination.Date = source.Date;

            return destination;
        }

        public MonthPattern Map(UpdateMonthPatternDto source)
        {
            var destination = new MonthPattern();
            destination.Id = source.Id;
            destination.PatternId = source.PatternId;
            destination.Date = source.Date;

            return destination;
        }

        public MonthPatternDto Map(MonthPattern source)
        {
            var destination = new MonthPatternDto();
            destination.Id = source.Id;
            destination.PatternId = source.PatternId;
            destination.Date = source.Date;

            return destination;
        }

        public ICollection<MonthPatternDto> MapElements(ICollection<MonthPattern> source)
        {
            List<MonthPatternDto> destination = new List<MonthPatternDto>();
            foreach (var item in source)
                destination.Add(Map(item));

            return destination;
        }

        public ICollection<MonthPattern> MapElements(ICollection<MonthPatternDto> source)
        {
            List<MonthPattern> destination = new List<MonthPattern>();
            foreach (var item in source)
                destination.Add(Map(item));

            return destination;
        }
    }
    public interface IMonthPatternMapper
    {
        MonthPattern Map(MonthPatternDto source);
        MonthPattern Map(AddMonthPatternDto source);
        MonthPattern Map(UpdateMonthPatternDto source);
        MonthPatternDto Map(MonthPattern source);
        ICollection<MonthPatternDto> MapElements(ICollection<MonthPattern> source);
        ICollection<MonthPattern> MapElements(ICollection<MonthPatternDto> source);
    }
}
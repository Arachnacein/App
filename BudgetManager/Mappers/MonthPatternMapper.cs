using BudgetManager.Dto.MonthPattern;
using BudgetManager.Features.MonthPatterns.Commands;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public interface IMonthPatternMapper
    {
        MonthPattern Map(MonthPatternDto source);
        MonthPattern Map(AddMonthPatternDto source);
        MonthPattern Map(UpdateMonthPatternDto source);
        MonthPatternDto Map(MonthPattern source);
        AddMonthPatternDto Map(SaveMonthPatternCommand command);
        UpdateMonthPatternDto Map(UpdateMonthPatternCommand command);
        ICollection<MonthPatternDto> MapElements(ICollection<MonthPattern> source);
        ICollection<MonthPattern> MapElements(ICollection<MonthPatternDto> source);
    }
    public class MonthPatternMapper : IMonthPatternMapper
    {

        public MonthPattern Map(MonthPatternDto source)
        {
            var destination = new MonthPattern();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.PatternId = source.PatternId;
            destination.Date = source.Date;

            return destination;
        }

        public MonthPattern Map(AddMonthPatternDto source)
        {
            var destination = new MonthPattern();
            destination.UserId = source.UserId;
            destination.PatternId = source.PatternId;
            destination.Date = source.Date;

            return destination;
        }

        public MonthPattern Map(UpdateMonthPatternDto source)
        {
            var destination = new MonthPattern();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.PatternId = source.PatternId;
            destination.Date = source.Date;

            return destination;
        }

        public MonthPatternDto Map(MonthPattern source)
        {
            var destination = new MonthPatternDto();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.PatternId = source.PatternId;
            destination.Date = source.Date;

            return destination;
        }

        public AddMonthPatternDto Map(SaveMonthPatternCommand command)
        {
            var destination = new AddMonthPatternDto();
            destination.UserId = command.UserId;
            destination.PatternId = command.PatternId;
            destination.Date = command.Date;

            return destination;
        }

        public UpdateMonthPatternDto Map(UpdateMonthPatternCommand command)
        {
            var destination = new UpdateMonthPatternDto();
            destination.Id = command.Id;
            destination.UserId = command.UserId;
            destination.PatternId = command.PatternId;
            destination.Date = command.Date;

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
}
using BudgetManager.Dto.Pattern;
using BudgetManager.Features.Patterns.Commands;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public interface IPatternMapper
    {
        PatternDto Map(Pattern source);
        Pattern Map(PatternDto source);
        Pattern Map(AddPatternDto source);
        AddPatternDto Map(SavePatternCommand command);
        ICollection<PatternDto> MapElements(ICollection<Pattern> source);
        ICollection<Pattern> MapElements(ICollection<PatternDto> source);
    }
    public class PatternMapper : IPatternMapper
    {
        public PatternDto Map(Pattern source)
        {
            var destination = new PatternDto();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Value_Fees = source.Value_Fees;
            destination.Value_Saves = source.Value_Saves;
            destination.Value_Entertainment = source.Value_Entertainment;

            return destination;
        }

        public Pattern Map(PatternDto source)
        {
            var destination = new Pattern();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Value_Fees = source.Value_Fees;
            destination.Value_Saves = source.Value_Saves;
            destination.Value_Entertainment = source.Value_Entertainment;

            return destination;
        }

        public Pattern Map(AddPatternDto source)
        {
            var destination = new Pattern();
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Value_Fees = source.Value_Fees;
            destination.Value_Saves = source.Value_Saves;
            destination.Value_Entertainment = source.Value_Entertainment;

            return destination;
        }

        public AddPatternDto Map(SavePatternCommand command)
        {
            var destination = new AddPatternDto();
            destination.UserId = command.UserId;
            destination.Name = command.Name;
            destination.Value_Fees = command.Value_Fees;
            destination.Value_Saves = command.Value_Saves;
            destination.Value_Entertainment = command.Value_Entertainment;

            return destination;
        }

        public ICollection<PatternDto> MapElements(ICollection<Pattern> source)
        {
            List<PatternDto> destination = new List<PatternDto>();
            foreach (var item in source)
                destination.Add(Map(item));

            return destination;
        }

        public ICollection<Pattern> MapElements(ICollection<PatternDto> source)
        {
            List<Pattern> destination = new List<Pattern>();
            foreach (var item in source)
                destination.Add(Map(item));

            return destination;
        }
    }
}
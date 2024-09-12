using BudgetManager.Dto.Pattern;
using BudgetManager.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BudgetManager.Mappers
{
    public class PatternMapper : IPatternMapper
    {
        public PatternDto Map(Pattern source)
        {
            var destination = new PatternDto();
            destination.Id = source.Id;
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
            destination.Name = source.Name;
            destination.Value_Fees = source.Value_Fees;
            destination.Value_Saves = source.Value_Saves;
            destination.Value_Entertainment = source.Value_Entertainment;

            return destination;
        }

        public Pattern Map(AddPatternDto source)
        {
            var destination = new Pattern();
            destination.Name = source.Name;
            destination.Value_Fees = source.Value_Fees;
            destination.Value_Saves = source.Value_Saves;
            destination.Value_Entertainment = source.Value_Entertainment;

            return destination;
        }
        public ICollection<PatternDto> MapeElements(ICollection<Pattern> source)
        {
            List<PatternDto> destination = new List<PatternDto>();
            foreach (var item in source)
                destination.Add(Map(item));

            return destination;
        }

        public ICollection<Pattern> MapeElements(ICollection<PatternDto> source)
        {
            List<Pattern> destination = new List<Pattern>();
            foreach (var item in source)
                destination.Add(Map(item));

            return destination;
        }
    }
    public interface IPatternMapper
    {
        PatternDto Map(Pattern source);
        Pattern Map(PatternDto source);
        Pattern Map(AddPatternDto source);
        ICollection<PatternDto> MapeElements(ICollection<Pattern> source);
        ICollection<Pattern> MapeElements(ICollection<PatternDto> source);
    }
}

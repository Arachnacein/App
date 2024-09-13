using BudgetManager.Dto.Income;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public class IncomeMapper : IIncomeMapper
    {
        public Income Map(IncomeDto source)
        {
            Income destination = new Income();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Amount = source.Amount; 
            destination.Date = source.Date;

            return destination;
        }

        public Income Map(AddIncomeDto source)
        {
            Income destination = new Income();
            destination.Name = source.Name;
            destination.Amount = source.Amount;
            destination.Date = source.Date;

            return destination;
        }

        public Income Map(UpdateIncomeDto source)
        {
            Income destination = new Income();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Amount = source.Amount;
            destination.Date = source.Date;

            return destination;
        }

        public IncomeDto Map(Income source)
        {
            var destination = new IncomeDto();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Amount = source.Amount;
            destination.Date = source.Date;

            return destination;
        }

        public ICollection<IncomeDto> MapElements(ICollection<Income> source)
        {
            List<IncomeDto> destination = new List<IncomeDto>();
            foreach (var item in source)
                destination.Add(Map(item));

            return destination;
        }

        public ICollection<Income> MapElements(ICollection<IncomeDto> source)
        {
            List<Income> destination = new List<Income>();
            foreach (var item in source)
                destination.Add(Map(item));

            return destination;
        }
    }
    public interface IIncomeMapper
    {
        Income Map(IncomeDto source);
        Income Map(AddIncomeDto source);
        Income Map(UpdateIncomeDto source);
        IncomeDto Map(Income source);
        ICollection<IncomeDto> MapElements(ICollection<Income> source);
        ICollection<Income> MapElements(ICollection<IncomeDto> source);
    }
}
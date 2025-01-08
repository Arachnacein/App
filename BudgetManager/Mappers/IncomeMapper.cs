using BudgetManager.Dto.Income;
using BudgetManager.Features.Incomes.Commands;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public interface IIncomeMapper
    {
        Income Map(IncomeDto source);
        Income Map(AddIncomeDto source);
        Income Map(UpdateIncomeDto source);
        AddIncomeDto Map(SaveIncomeCommand command);
        UpdateIncomeDto Map(UpdateIncomeCommand command);
        IncomeDto Map(Income source);
        ICollection<IncomeDto> MapElements(ICollection<Income> source);
        ICollection<Income> MapElements(ICollection<IncomeDto> source);
    }
    public class IncomeMapper : IIncomeMapper
    {
        public Income Map(IncomeDto source)
        {
            Income destination = new Income();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Amount = source.Amount; 
            destination.Date = source.Date;

            return destination;
        }

        public Income Map(AddIncomeDto source)
        {
            Income destination = new Income();
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Amount = source.Amount;
            destination.Date = source.Date;

            return destination;
        }

        public Income Map(UpdateIncomeDto source)
        {
            Income destination = new Income();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Amount = source.Amount;
            destination.Date = source.Date;

            return destination;
        }

        public IncomeDto Map(Income source)
        {
            var destination = new IncomeDto();
            destination.Id = source.Id;
            destination.UserId = source.UserId;
            destination.Name = source.Name;
            destination.Amount = source.Amount;
            destination.Date = source.Date;

            return destination;
        }
        public AddIncomeDto Map(SaveIncomeCommand command)
        {
            var destination = new AddIncomeDto();
            destination.UserId = command.UserId;
            destination.Name = command.Name;
            destination.Amount = command.Amount;
            destination.Date = command.Date;

            return destination;
        }   
        public UpdateIncomeDto Map(UpdateIncomeCommand command)
        {
            var destination = new UpdateIncomeDto();
            destination.Id = command.Id;
            destination.UserId = command.UserId;
            destination.Name = command.Name;
            destination.Amount = command.Amount;
            destination.Date = command.Date;

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
}
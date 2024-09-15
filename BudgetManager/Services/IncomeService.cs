using BudgetManager.Dto;
using BudgetManager.Dto.Income;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.IncomeExceptions;
using BudgetManager.Mappers;
using BudgetManager.Repositories;

namespace BudgetManager.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IIncomeMapper _incomeMapper;

        public IncomeService(IIncomeRepository incomeRepository, IIncomeMapper incomeMapper)
        {
            _incomeRepository = incomeRepository;
            _incomeMapper = incomeMapper;
        }

        public async Task<IEnumerable<IncomeDto>> RetrieveIncomes()
        {
            var incomes = await _incomeRepository.GetAll();
            return _incomeMapper.MapElements(incomes.ToList());
        }

        public async Task<IncomeDto> RetrieveIncome(int id)
        {
            var income = await _incomeRepository.Get(id);
            if (income == null)
                throw new IncomeNotFoundException($"Income not found. Id:{id}.");
            return _incomeMapper.Map(income);
        }

        public async Task<IncomeDto> AddIncome(AddIncomeDto income)
        {
            if (income == null)
                throw new NullPointerException($"Object is null.");
            if (income.Name.Length < 5 || income.Name.Length > 50)
                throw new BadStringLengthException($"Name should be between 5 and 50 characters. Now is: {income.Name}.");
            if (income.Amount < 0d)
                throw new BadValueException($"Amount should be more than 0. Now is: {income.Amount}.");

            var mappedIncome = _incomeMapper.Map(income);
            await _incomeRepository.Add(mappedIncome);
            return _incomeMapper.Map(mappedIncome);
        }
        public async Task UpdateIncome(UpdateIncomeDto income)
        {
            if (income == null)
                throw new NullPointerException($"Object is null.");
            if (income.Name.Length < 5 || income.Name.Length > 50)
                throw new BadStringLengthException($"Name should be between 5 and 50 characters. Now is: {income.Name}.");
            if (income.Amount < 0d)
                throw new BadValueException($"Amount should be more than 0. Now is: {income.Amount}.");

            var mappedIncome = _incomeMapper.Map(income);
            await _incomeRepository.Update(mappedIncome);
        }

        public async Task DeleteIncome(int id)
        {
            var income = await _incomeRepository.Get(id);
            if (income == null)
                throw new IncomeNotFoundException($"Income not found. Id:{id}.");
            await _incomeRepository.Delete(income);
        }

        public async Task<IEnumerable<IncomeDto>> RetrieveIncomes(int month, int year)
        {
            var model = new MonthYearModel { Month = month, Year = year };
            var incomes = await _incomeRepository.Get(model);
            return _incomeMapper.MapElements(incomes.ToList());
        }
    }
}
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
        public async Task<IEnumerable<IncomeDto>> RetrieveIncomesAsync(Guid userId)
        {
            var incomes = await _incomeRepository.GetAllAsync(userId);
            return _incomeMapper.MapElements(incomes.ToList());
        }
        public async Task<IncomeDto> RetrieveIncomeAsync(int id, Guid userId)
        {
            var income = await _incomeRepository.GetAsync(id, userId);
            if (income == null)
                throw new IncomeNotFoundException($"Income not found. Id:{id}, userId:{userId}.");
            return _incomeMapper.Map(income);
        }
        public async Task<IncomeDto> AddIncomeAsync(AddIncomeDto income)
        {
            if (income == null)
                throw new NullPointerException($"Object is null.");
            if (income.Name.Length < 3 || income.Name.Length > 50)
                throw new BadStringLengthException($"Name should be between 3 and 50 characters. Now is: {income.Name}.");
            if (income.Amount < 1d)
                throw new BadValueException($"Amount should be more than 0. Now is: {income.Amount}.");

            var mappedIncome = _incomeMapper.Map(income);
            await _incomeRepository.AddAsync(mappedIncome);
            return _incomeMapper.Map(mappedIncome);
        }
        public async Task UpdateIncomeAsync(UpdateIncomeDto income)
        {
            if (income == null)
                throw new NullPointerException($"Object is null.");
            if (income.Name.Length < 3 || income.Name.Length > 50)
                throw new BadStringLengthException($"Name should be between 3 and 50 characters. Now is: {income.Name}.");
            if (income.Amount < 1d)
                throw new BadValueException($"Amount should be more than 0. Now is: {income.Amount}.");

            var mappedIncome = _incomeMapper.Map(income);
            await _incomeRepository.UpdateAsync(mappedIncome);
        }
        public async Task DeleteIncomeAsync(int id, Guid userId)
        {
            var income = await _incomeRepository.GetAsync(id, userId);
            if (income == null)
                throw new IncomeNotFoundException($"Income not found. Id:{id}.");
            await _incomeRepository.DeleteAsync(income);
        }
        public async Task<IEnumerable<IncomeDto>> RetrieveIncomesAsync(int month, int year, Guid userId)
        {
            var model = new MonthYearModel { Month = month, Year = year };
            var incomes = await _incomeRepository.GetAsync(model, userId);
            return _incomeMapper.MapElements(incomes.ToList());
        }
    }
}
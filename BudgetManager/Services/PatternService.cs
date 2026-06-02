using BudgetManager.Dto.Pattern;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Repositories;

namespace BudgetManager.Services
{
    public class PatternService : IPatternService
    {
        private readonly IPatternRepository _patternRepository;
        private readonly IMonthPatternRepository _monthPatternRepository;
        private readonly IPatternMapper _patternMapper;
        public PatternService(IPatternRepository patternRepository, IMonthPatternRepository monthPatternRepository, IPatternMapper patternMapper)
        {
            _patternRepository = patternRepository;
            _monthPatternRepository = monthPatternRepository;
            _patternMapper = patternMapper;
        }
      
        public async Task<IEnumerable<PatternDto>> RetrievePatternsAsync(Guid userId)
        {
            var patterns =  await _patternRepository.GetAllAsync(userId);
            return _patternMapper.MapElements(patterns.ToList());
        }

        public async Task<PatternDto> RetrievePatternAsync(int id, Guid userId)
        {
            var pattern = await _patternRepository.GetAsync(id, userId);
            if (pattern == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{id}");
            return _patternMapper.Map(pattern);
        }

        public async Task<PatternDto> AddPatternAsync(AddPatternDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException($"Object is null.");
            if (dto.Name.Length <= 3)
                throw new BadStringLengthException($"Name have incorrect length. Should be more than 3 characters.");
            if (dto.Name.Length >= 50)
                throw new BadStringLengthException($"Name have incorrect length. Should be less than 50 characters.");
            if (dto.Value_Fees < 0d || dto.Value_Fees > 100d)
                throw new BadValueException($"Fees Value should be more than 0 and less than 100. ({dto.Value_Fees})");            
            if (dto.Value_Saves < 0d || dto.Value_Saves > 100d)
                throw new BadValueException($"Saves Value should be more than 0 and less than 100. ({dto.Value_Saves})");
            if (dto.Value_Entertainment < 0d || dto.Value_Entertainment > 100d)
                throw new BadValueException($"Entertainment Value should be more than 0 and less than 100. ({dto.Value_Entertainment})");
            var sum = dto.Value_Entertainment + dto.Value_Fees + dto.Value_Saves;
            if (sum != 100d)
                throw new BadValueException($"Value_Fees + Value_Saves + Value_Entertainment Should be 100%. Current is {sum}.");

            if (await _patternRepository.ExistsWithNameAsync(dto.Name, dto.UserId))
                throw new PatternAlreadyExistsException($"Pattern with name '{dto.Name}' already exists.");
            if (await _patternRepository.ExistsWithValuesAsync(dto.Value_Saves, dto.Value_Fees, dto.Value_Entertainment, dto.UserId))
                throw new PatternAlreadyExistsException($"Pattern with the same values already exists.");

            Pattern mappedPattern = _patternMapper.Map(dto);
            await _patternRepository.AddAsync(mappedPattern);
            return _patternMapper.Map(mappedPattern);
        }

        public async Task DeletePatternAsync(int id, Guid userId)
        {
            var pattern = await _patternRepository.GetAsync(id, userId);
            if (pattern == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{id}");

            var usageCount = await _monthPatternRepository.CountByPatternIdAsync(id, userId);
            if (usageCount > 0)
                throw new PatternInUseException($"Pattern is in use in {usageCount} month(s). Id:{id}");

            await _patternRepository.DeleteAsync(id, userId);
        }
    }
}

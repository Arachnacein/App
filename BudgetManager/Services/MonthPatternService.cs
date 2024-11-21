using BudgetManager.Dto;
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Dto.Pattern;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Mappers;
using BudgetManager.Repositories;

namespace BudgetManager.Services
{
    public class MonthPatternService : IMonthPatternService
    {
        private readonly IMonthPatternRepository _monthPatternRepository;
        private readonly IPatternRepository _patternRepository;
        private readonly IMonthPatternMapper _monthPatternMapper;
        private readonly IPatternMapper _patternMapper;

        public MonthPatternService(IMonthPatternRepository repository, IMonthPatternMapper mapper, IPatternRepository patternRepository, IPatternMapper patternMapper)
        {
            _monthPatternRepository = repository;
            _monthPatternMapper = mapper;
            _patternRepository = patternRepository;
            _patternMapper = patternMapper;
        }

        public async Task<MonthPatternDto> RetrieveMonthPatternAsync(int id, Guid userId)
        {
            var monthPattern = await _monthPatternRepository.GetAsync(id, userId);
            if (monthPattern == null)
                throw new Exception($"Pattern not found exception. Id: {id}.");
            return _monthPatternMapper.Map(monthPattern);
        }

        public async Task<IEnumerable<MonthPatternDto>> RetrieveMonthPatternsAsync(Guid userId)
        {
            var monthPatterns = await _monthPatternRepository.GetAllAsync(userId);
            return _monthPatternMapper.MapElements(monthPatterns.ToList());
        }

        public async Task<MonthPatternDto> AddMonthPatternAsync(AddMonthPatternDto dto)
        {
            var checkPatternExists = await _patternRepository.GetAsync(dto.PatternId, dto.UserId);
            if (checkPatternExists == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{dto.PatternId}.");

            var exists = await _monthPatternRepository.CheckExistsAsync(new MonthYearModel { Month =  dto.Date.Month, Year = dto.Date.Year}, dto.UserId);
            if (exists != 0)
                throw new MonthPatternAlreadyExistsException($"Pattern for Month:{dto.Date.Month} and Year:{dto.Date.Year} already exists.");

            var mappedMonthPattern = _monthPatternMapper.Map(dto);
            await _monthPatternRepository.AddAsync(mappedMonthPattern);
            return _monthPatternMapper.Map(mappedMonthPattern);
        }

        public async Task UpdateMonthPatternAsync(UpdateMonthPatternDto dto)
        {
            var monthPattern = await _monthPatternRepository.GetAsync(dto.Id, dto.UserId);
            if (monthPattern == null)
                throw new MonthPatternNotFoundException($"Pattern not found exception. Id: {dto.Id}.");
            var mappedMonthPattern = _monthPatternMapper.Map(dto);
            await _monthPatternRepository.UpdateAsync(mappedMonthPattern);
        }

        public async Task DeleteMonthPatternAsync(int id, Guid userId)
        {
            var monthPattern = await _monthPatternRepository.GetAsync(id, userId);
            if (monthPattern == null)
                throw new MonthPatternNotFoundException($"MonthPattern not found exception. Id: {id}.");
            await _monthPatternRepository.DeleteAsync(monthPattern);
        }

        public async Task<PatternDto> RetrieveMonthPatternAsync(int month, int year, Guid userId)
        {
            var model = new MonthYearModel { Month = month, Year = year };
            var monthPattern = await _monthPatternRepository.GetAsync(model, userId);
            if (monthPattern == null)
                return new PatternDto { Id = -1 };

            var pattern = await _patternRepository.GetAsync(monthPattern.PatternId, userId);
            if(pattern == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{pattern.Id}.");

            return _patternMapper.Map(pattern);
        }

        public async Task<IEnumerable<FullMonthPatternDto>> RetrievePatternsAsync(Guid userId)
        {
            var monthpattern = await _monthPatternRepository.GetAllAsync(userId);
            monthpattern = monthpattern.ToList();//avoid reading two entities in the same time
            var patterns = await _patternRepository.GetAllAsync(userId);

            var result = monthpattern.Join(patterns,
                                            monthpattern => monthpattern.PatternId,
                                            pattern => pattern.Id,
                                            (monthpattern, pattern) => new FullMonthPatternDto
                                            {
                                                Id = monthpattern.Id,
                                                UserId = userId,
                                                Date = monthpattern.Date,
                                                Pattern = new PatternDto
                                                {
                                                    Id = pattern.Id,
                                                    UserId = userId,
                                                    Name = pattern.Name,
                                                    Value_Saves = pattern.Value_Saves,
                                                    Value_Fees = pattern.Value_Fees,
                                                    Value_Entertainment = pattern.Value_Entertainment
                                                }
                                            }
                                           ).OrderByDescending(x => x.Date)
                                            .ToList();
            return result;
        }
    }
}
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.MonthPatterns.Commands
{
    public record SaveMonthPatternCommand : IRequest<MonthPatternDto>
    {
        public DateTime Date { get; init; }
        public int PatternId { get; init; }
        public SaveMonthPatternCommand(DateTime date, int patternId)
        {
            Date = date;
            PatternId = patternId;
        }
    }
    public class SaveMonthPatternCommandHandler : IRequestHandler<SaveMonthPatternCommand, MonthPatternDto>
    {
        private readonly IMonthPatternService _monthPatternService;
        private readonly IMonthPatternMapper _monthPatternMapper;
        public SaveMonthPatternCommandHandler(IMonthPatternService monthPatternService, IMonthPatternMapper monthPatternMapper)
        {
            _monthPatternService = monthPatternService;
            _monthPatternMapper = monthPatternMapper;
        }

        public async Task<MonthPatternDto> Handle(SaveMonthPatternCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            return await _monthPatternService.AddMonthPattern(_monthPatternMapper.Map(request));
        }
    }
}
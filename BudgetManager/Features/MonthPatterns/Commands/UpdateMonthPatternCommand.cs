using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.MonthPatterns.Commands
{
    public record UpdateMonthPatternCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime Date { get; init; }
        public int PatternId { get; init; }
        public UpdateMonthPatternCommand(int id, Guid userId, DateTime date, int patternId)
        {
            UserId = userId;
            Id = id;
            Date = date;
            PatternId = patternId;
        }
    }
    public class UpdateMonthPatternCommandHandler : IRequestHandler<UpdateMonthPatternCommand>
    {
        private readonly IMonthPatternService _monthPatternService;
        private readonly IMonthPatternMapper _monthPatternMapper;
        public UpdateMonthPatternCommandHandler(IMonthPatternService monthPatternService, IMonthPatternMapper monthPatternMapper)
        {
            _monthPatternService = monthPatternService;
            _monthPatternMapper = monthPatternMapper;
        }

        public async Task Handle(UpdateMonthPatternCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            await _monthPatternService.UpdateMonthPatternAsync(_monthPatternMapper.Map(request));
        }
    }
}
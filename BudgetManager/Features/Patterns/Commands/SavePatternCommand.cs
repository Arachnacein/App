using BudgetManager.Dto.Pattern;
using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Patterns.Commands
{
    public record SavePatternCommand : IRequest<PatternDto>
    {
        public Guid UserId { get; init; }
        public string Name { get; init; }
        public double Value_Saves { get; init; }
        public double Value_Fees { get; init; }
        public double Value_Entertainment { get; init; }
        public SavePatternCommand(Guid userId, string name, double value_Saves, double value_Fees, double value_Entertainment)
        {
            UserId = userId;
            Name = name;
            Value_Saves = value_Saves;
            Value_Fees = value_Fees;
            Value_Entertainment = value_Entertainment;
        }
    }

    public class SavePatternCommandHandler : IRequestHandler<SavePatternCommand, PatternDto>
    {
        private readonly IPatternService _patternService;
        private readonly IPatternMapper _patternMapper;
        public SavePatternCommandHandler(IPatternService patternService, IPatternMapper patternMapper)
        {
            _patternService = patternService;
            _patternMapper = patternMapper;
        }

        public async Task<PatternDto> Handle(SavePatternCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            return await _patternService.AddPatternAsync(_patternMapper.Map(request));
        }
    }
}

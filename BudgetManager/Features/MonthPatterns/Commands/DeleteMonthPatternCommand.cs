using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.MonthPatterns.Commands
{
    public record DeleteMonthPatternCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public DeleteMonthPatternCommand(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class DeleteMonthPatternCommandHandler : IRequestHandler<DeleteMonthPatternCommand>
    {
        private readonly IMonthPatternService _monthPatternService;
        public DeleteMonthPatternCommandHandler(IMonthPatternService monthPatternService)
        {
            _monthPatternService = monthPatternService;
        }

        public async Task Handle(DeleteMonthPatternCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));
            await _monthPatternService.DeleteMonthPattern(request.Id, request.UserId);
        }
    }
}
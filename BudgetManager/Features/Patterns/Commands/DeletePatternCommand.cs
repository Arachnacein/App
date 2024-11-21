using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Patterns.Commands
{
    public record DeletePatternCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public DeletePatternCommand(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class DeletePatternCommandHandler : IRequestHandler<DeletePatternCommand>
    {
        private readonly IPatternService _patternService;
        public DeletePatternCommandHandler(IPatternService patternService)
        {
            _patternService = patternService;
        }

        public async Task Handle(DeletePatternCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            await _patternService.DeletePatternAsync(request.Id, request.UserId);
        }
    }
}
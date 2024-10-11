using BudgetManager.Dto.Transaction;
using MediatR;

namespace BudgetManager.Features.Transactions.Queries
{
    public class RetrieveTransactionsQuery : IRequest<List<TransactionDto>>
    {

    }
}

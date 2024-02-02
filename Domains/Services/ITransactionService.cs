using MonTraApi.Common;
using MonTraApi.Domains.DTOs;

namespace MonTraApi.Domains.Services;

public interface ITransactionService
{
    Task<ResultDTO<List<CategoryDTO>?>> GetCategories(string userId, int limit, int offset);
    Task<ResultDTO<bool?>> InsertCategory(CreateCategoryRequest request);
    Task<ResultDTO<bool?>> CreateNewTransaction(CreateNewTransactionRequest request);
    Task<ResultDTO<List<TransactionDTO>?>> GetTransactions(string userId, int limit, int offset, OrderByType orderBy, CategoryType? categoryType = null, string? categoryId = null);

}

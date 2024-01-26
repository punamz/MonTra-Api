using MonTraApi.Domains.DTOs;

namespace MonTraApi.Domains.Services;

public interface ITransactionService
{
    Task<ResultDTO<List<CategoryDTO>?>> GetCategories(int limit, int offset);
    Task<ResultDTO<bool?>> InsertCategory(CreateCategoryRequest request);

}

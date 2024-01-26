using MonTraApi.Domains.DTOs;
using MonTraApi.Domains.Services;

namespace MonTraApi.Routers;

public static class TransactionRouter
{
    public static void TransactionMap(this IEndpointRouteBuilder builder)
    {
        string groupName = "transaction";
        string tag = "Transaction";

        builder.MapGet($"/{groupName}/getCategories", async (ITransactionService transactionService, int limit, int offset) => await transactionService.GetCategories(limit: limit, offset: offset)).WithTags(tag);
        builder.MapPost($"/{groupName}/createCategory", async (ITransactionService transactionService, CreateCategoryRequest request) => await transactionService.InsertCategory(request: request)).WithTags(tag);
    }

}

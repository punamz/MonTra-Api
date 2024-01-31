using MonTraApi.Common;
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

        builder.MapPost($"/{groupName}/createTransaction", async (HttpContext ctx, ITransactionService transactionService, CreateNewTransactionRequest request) =>
        {
            string? userIdToken = ctx.User.Claims.Where(c => c.Type == ConstantValue.JWTUserIdKey).Select(c => c.Value).SingleOrDefault();

            if (userIdToken == null || userIdToken != request.UserId)
                return Results.Unauthorized();

            var value = await transactionService.CreateNewTransaction(request: request);
            return Results.Ok(value: value);
        }).WithTags(tag).RequireAuthorization();

        builder.MapGet($"/{groupName}/getTransactions", async (HttpContext ctx, ITransactionService transactionService, string userId, int limit, int offset, OrderByType? orderBy, CategoryType? categoryType, string? categoryId) =>
        {
            string? userIdToken = ctx.User.Claims.Where(c => c.Type == ConstantValue.JWTUserIdKey).Select(c => c.Value).SingleOrDefault();
            if (userIdToken == null || userIdToken != userId)
                return Results.Unauthorized();

            var result = await transactionService.GetTransactions(userId: userId,
                                                            limit: limit,
                                                            offset: offset,
                                                            orderBy: orderBy ?? OrderByType.Newest,
                                                            categoryType: categoryType,
                                                            categoryId: categoryId);
            return Results.Ok(result);
        }).WithTags(tag).RequireAuthorization();
    }

}

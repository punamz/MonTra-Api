using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.DTOs;
using MonTraApi.Domains.Entities;
using MonTraApi.Domains.Services;
using MonTraApi.Infrastructures.Commands;
using MonTraApi.Infrastructures.Queries;
using System.Diagnostics;

namespace MonTraApi.Infrastructures.Repositories;

public class TransactionRepository : ITransactionService
{
    private readonly IMongoDatabase _database;
    private readonly IMapper _mapper;

    public TransactionRepository(IMongoDatabase database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<ResultDTO<List<CategoryDTO>?>> GetCategories(string userId, int limit, int offset)
    {
        try
        {
            // check valid param
            if (limit < 0 || offset < 0)
                return Helper.GetResponse<List<CategoryDTO>?>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0002, message: ConstantValue.Err0002Message);

            List<CategoryEntity> categories = await _database.CategoryColection().GetCategory(userId, limit, offset);
            if (categories.IsNullOrEmpty())
                return Helper.GetResponse<List<CategoryDTO>?>(statusCode: StatusCodeValue.NoData);
            else
                return Helper.GetResponse<List<CategoryDTO>?>(data: _mapper.Map<List<CategoryEntity>, List<CategoryDTO>>(categories));
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Helper.GetResponse<List<CategoryDTO>?>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0001, message: $"Error: {e.Message}");
        }
    }

    public async Task<ResultDTO<bool?>> InsertCategory(CreateCategoryRequest request)
    {
        try
        {
            string newCategorytId = ObjectId.GenerateNewId().ToString();
            CategoryEntity category = new() { Id = newCategorytId, Category = request.Category, Type = request.Type, Color = request.Color, Icon = request.Icon, UserId = request.UserId };
            await _database.CategoryColection().CreateNewCategory(category);
            return Helper.GetResponse<bool?>(data: true);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Helper.GetResponse<bool?>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0001, message: $"Error: {e.Message}");
        }
    }

    public async Task<ResultDTO<bool?>> CreateNewTransaction(CreateNewTransactionRequest request)
    {
        try
        {
            string newTransactionId = ObjectId.GenerateNewId().ToString();
            TransactionEntity transaction = new()
            {
                Id = newTransactionId,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
                Amount = request.Amount,
                Description = request.Note,
                TransactionAt = request.TransactionAt,
            };
            await _database.TransactionColection().CreateNewTransaction(transaction);
            return Helper.GetResponse<bool?>(data: true);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Helper.GetResponse<bool?>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0001, message: $"Error: {e.Message}");
        }
    }

    public async Task<ResultDTO<List<TransactionDTO>?>> GetTransactions(string userId, int limit, int offset, OrderByType orderBy, CategoryType? categoryType = null, string? categoryId = null)
    {
        try
        {
            List<TransactionAggregate> result = await _database.GetTransactionAggregate(
                userId: userId,
                limit: limit,
                offset: offset,
                OrderBy: orderBy,
                categoryType: categoryType,
                categoryId: categoryId);

            if (result.IsNullOrEmpty())
                return Helper.GetResponse<List<TransactionDTO>?>(statusCode: StatusCodeValue.NoData);

            return Helper.GetResponse<List<TransactionDTO>?>(data: _mapper.Map<List<TransactionAggregate>, List<TransactionDTO>>(result));
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Helper.GetResponse<List<TransactionDTO>?>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0001, message: $"Error: {e.Message}");
        }
    }
}

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

    public async Task<ResultDTO<List<TransactionDTO>?>> GetTransactions(string userId, int limit, int offset, OrderByType orderBy, CategoryType? categoryType = null, List<string>? categoriesId = null)
    {
        try
        {
            List<TransactionAggregate> result = await _database.GetTransactionAggregate(
                userId: userId,
                limit: limit,
                offset: offset,
                OrderBy: orderBy,
                categoryType: categoryType,
                categoriesId: categoriesId);

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

    public async Task<ResultDTO<GetFrequencyResponse?>> GetFrequency(string userId, int timeZone, FrequencyType frequencyType, CategoryType categoryType)
    {
        try
        {
            DateTime startTime = CalStartTimeFrequency(type: frequencyType, timeZone: timeZone);
            int frequencyLength = frequencyType switch
            {
                FrequencyType.Today => 24,
                FrequencyType.Week => 7,
                FrequencyType.Month => DateTime.DaysInMonth(startTime.Year, startTime.Month),
                FrequencyType.Year => 12,
                _ => 0
            };
            GetFrequencyResponse result = new()
            {
                Frequency = Enumerable.Repeat(0, frequencyLength).ToList()
            };

            List<TransactionFrequencyEntity> transactionFrequencies = await _database.GetTransactionFrequency(userId: userId, categoryType: categoryType, startTime: startTime);
            if (transactionFrequencies == null)
                return Helper.GetResponse<GetFrequencyResponse?>(statusCode: StatusCodeValue.NoData);

            foreach (var transactionFrequency in transactionFrequencies)
            {
                transactionFrequency.TransactionAt.AddHours(timeZone);
                switch (frequencyType)
                {
                    case FrequencyType.Today:
                        result.Frequency[transactionFrequency.TransactionAt.Hour] += transactionFrequency.Amount;
                        break;
                    case FrequencyType.Week:
                        result.Frequency[((int)transactionFrequency.TransactionAt.DayOfWeek + 6) % 7] += transactionFrequency.Amount;
                        break;
                    case FrequencyType.Month:
                        result.Frequency[transactionFrequency.TransactionAt.Day - 1] += transactionFrequency.Amount;
                        break;
                    case FrequencyType.Year:
                        result.Frequency[transactionFrequency.TransactionAt.Month - 1] += transactionFrequency.Amount;
                        break;
                    default:
                        break;
                }
            }
            return Helper.GetResponse<GetFrequencyResponse?>(data: result);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Helper.GetResponse<GetFrequencyResponse?>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0001, message: $"Error: {e.Message}");
        }
    }

    private static DateTime CalStartTimeFrequency(FrequencyType type, int timeZone)
    {
        switch (type)
        {
            case FrequencyType.Today:
                {
                    var today = DateTime.Now;
                    return today.AddHours(-timeZone);
                }
            case FrequencyType.Week:
                {
                    var today = DateTime.Now.AddHours(-timeZone);
                    int delta = DayOfWeek.Monday - today.DayOfWeek;
                    if (delta > 0) delta -= 7;
                    return today.AddDays(delta);
                }
            case FrequencyType.Month:
                {
                    var today = DateTime.Now.AddHours(-timeZone);
                    return new DateTime(today.Year, today.Month, 1, 0, 0, 0, 0);
                }
            case FrequencyType.Year:
                {
                    var today = DateTime.Now.AddHours(-timeZone);
                    return new DateTime(today.Year, 1, 1, 0, 0, 0, 0);
                }
            default:
                {
                    var result = DateTime.Now;
                    return result.AddHours(-timeZone);
                }

        }

    }
}

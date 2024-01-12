using AutoMapper;
using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.Services;
using MonTraApi.Infrastructures.Repositories;
using MonTraApi.Routers;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var Username = Environment.GetEnvironmentVariable(ConstantValue.UserNameEnvKey);
var Password = Environment.GetEnvironmentVariable(ConstantValue.PasswordEnvKey);
var DatabaseUrl = Environment.GetEnvironmentVariable(ConstantValue.BaseUrlEnvKey);
var DatabaseName = Environment.GetEnvironmentVariable(ConstantValue.DatabaseNameEnvKey);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
    mc.AllowNullCollections = true;
    mc.AllowNullDestinationValues = true;
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var a = $"mongodb+srv://{Username}:{Password}@{DatabaseUrl}/?retryWrites=true&w=majority";
builder.Services.AddTransient(x => new MongoClient($"mongodb+srv://{Username}:{Password}@{DatabaseUrl}/?retryWrites=true&w=majority").GetDatabase(DatabaseName));
builder.Services.AddScoped<IAuthenticationService, AuthenticationRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => DateTime.Now);
app.AuthenticationMap();


app.Run();


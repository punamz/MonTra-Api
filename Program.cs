using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.Services;
using MonTraApi.Infrastructures.Repositories;
using MonTraApi.Routers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region log

#endregion


#region mapper

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
    mc.AllowNullCollections = true;
    mc.AllowNullDestinationValues = true;
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

#endregion

#region database

var Username = Environment.GetEnvironmentVariable(ConstantValue.UserNameEnvKey);
var Password = Environment.GetEnvironmentVariable(ConstantValue.PasswordEnvKey);
var DatabaseUrl = Environment.GetEnvironmentVariable(ConstantValue.BaseUrlEnvKey);
var DatabaseName = Environment.GetEnvironmentVariable(ConstantValue.DatabaseNameEnvKey);
var connectionString = $"mongodb+srv://{Username}:{Password}@{DatabaseUrl}/?retryWrites=true&w=majority";
builder.Services.AddTransient(x => new MongoClient(connectionString).GetDatabase(DatabaseName));

#endregion


#region repository

builder.Services.AddScoped<IAuthenticationService, AuthenticationRepository>();
builder.Services.AddScoped<ITransactionService, TransactionRepository>();

#endregion

#region setup JWT bearer

var JWTKey = Environment.GetEnvironmentVariable(ConstantValue.JWTKey) ?? "";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey)),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
    };
});
builder.Services.AddAuthorization();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => DateTime.Now);
app.AuthenticationMap();
app.TransactionMap();


app.Run();


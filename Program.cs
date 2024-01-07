var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/login", (LoginParam param) =>
{
if (param.username == "admin" && param.password == "123456")
return true;
else return false;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/", () => DateTime.Now);


app.Run();


class LoginParam
{
    public string? username { get; set; }
    public string? password { get; set; }
}
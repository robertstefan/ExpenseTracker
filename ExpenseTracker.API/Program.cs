using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Services;
using ExpenseTracker.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITransactionRepository, ITransactionRepository>(sp =>
    new TransactionsRespository(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddScoped<TransactionService>();

builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>(sp =>
    new CategoriesRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped<IRaportRepository, RaportRepository>(sp =>
    new RaportRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddScoped<RaportService>();

builder.Services.AddScoped<IUserRepository, UserRepository>(sp =>
    new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddScoped<UserService>();

builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();

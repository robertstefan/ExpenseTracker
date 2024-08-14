using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Services;
using ExpenseTracker.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITransactionsRepository, ITransactionsRepository>(sp =>
    new TransactionsRespository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICategoriesRepository, ICategoriesRepository>(sp =>
    new CategoriesRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<TransactionsService>();
builder.Services.AddScoped<CategoriesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

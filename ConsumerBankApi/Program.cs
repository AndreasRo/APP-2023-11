using ConsumerBank.Services;
using ConsumerBank.Services.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<ILoanerService, LoanerService>();
builder.Services.AddTransient<IDatabase, Database>();
builder.Services.AddSingleton(_ => 
        new DbOptions(builder.Configuration.GetValue<string>(nameof(DbOptions.Database)), builder.Configuration.GetValue<string>(nameof(DbOptions.Username)), builder.Configuration.GetValue<string>(nameof(DbOptions.Password)))
);
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

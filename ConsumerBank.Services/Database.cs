using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ConsumerBank.Services.DbObjects;
using System.Data.SqlClient;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ConsumerBank.Services.Options;

namespace ConsumerBank.Services;

public interface IDatabase
{
    Task<int> SavePerson(PersonEntity person);
    Task SaveAddress(AddressEntity address);
    Task UpdateLoan(int personId, decimal amount);
}

public class Database : DbContext, IDatabase
{
    private readonly DbOptions _dbOptions;

    public Database(DbOptions dbOptions)
    {
        _dbOptions = dbOptions;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = new SqlConnection(
            $"Server=tcp:{_dbOptions.Database},1433;Initial Catalog=kurs-itv-inttest-db;Persist Security Info=False;User ID={_dbOptions.Username};Password={_dbOptions.Password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        optionsBuilder.UseSqlServer(connection);
    }

    public DbSet<PersonEntity> Persons { get; set; }
    public DbSet<AddressEntity> Address { get; set; }
    public DbSet<LoanEntity> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PersonEntity>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<LoanEntity>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<AddressEntity>()
            .HasKey(p => p.Id);
    }

    public async Task<int> SavePerson(PersonEntity person)
    {
        Persons.Add(person);
        await SaveChangesAsync();
        return person.Id;
    }

    public async Task SaveAddress(AddressEntity address)
    {
        Address.Add(address);
        await SaveChangesAsync();
    }

    public async Task UpdateLoan(int personId, decimal amount)
    {
        var loan = await Loans.FirstOrDefaultAsync(loan => loan.PersonId == personId);
        if (loan == null)
        {
            loan = new()
            {
                PersonId = personId,
                Amount = amount
            };
            await Loans.AddAsync(loan);
        }
        else
        {
            loan.Amount = amount;
        }
        await SaveChangesAsync();
    }
}
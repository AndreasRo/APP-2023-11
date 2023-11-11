using System.Threading.Tasks;
using ConsumerBank.Services.Contracts;
using ConsumerBank.Services.DbObjects;
using ConsumerBank.Services.Options;

namespace ConsumerBank.Services
{
    public class LoanerService : ILoanerService
    {
        private readonly IDatabase _database;

        public LoanerService(IDatabase database)
        {
            _database = database;
        }

        public async Task<bool> Apply(LoanRequest request)
        {
            var accepted = CreditProvider.Evaluate(request);
            var personId = await _database.SavePerson(new PersonEntity
            {
                FirstName = request.Person.FirstName,
                LastName = request.Person.LastName,
                SocialSecurityNumber = request.Person.SocialSecurityNumber
            });

            await _database.SaveAddress(new AddressEntity
            {
                City = request.Person.CustomerAddress.City,
                Country = request.Person.CustomerAddress.Country,
                Street = request.Person.CustomerAddress.Street,
                Zip = request.Person.CustomerAddress.Zip,
                PersonId = personId
            });

            await _database.UpdateLoan(personId, request.Amount);

            return accepted;
        }
    }

    public interface ILoanerService
    {
        Task<bool> Apply(LoanRequest request);
    }
}
using ConsumerBank.Services.DbObjects;
using ConsumerBank.Services.Options;
using Microsoft.EntityFrameworkCore;

namespace ConsumerBank.Services.IntegrationTests
{
    // Disse testene vil feile hvis/n�r testressursene i Azure blir slettet....
    public class DatabaseTests : IDisposable
    {
        private readonly Database _database;
        private readonly List<int> _personIdsToDelete = new();

        public DatabaseTests()
        {
            var options = new DbOptions("inttest-itv-inttest-sqlsrv.database.windows.net", "abc","abc");
            _database = new Database(options);
        }

        [Fact]
        public async Task SavePerson_ShouldSucceed()
        {
            // Arrange
            var person = new PersonEntity
            {
                FirstName = "abcd",
                LastName = "abcd",
                SocialSecurityNumber = "1234"
            };

            // Act
            var personId = await _database.SavePerson(person);
            _personIdsToDelete.Add(personId);

            // Assert
            var actualPerson = await _database.Persons.FindAsync(personId);
            Assert.NotNull(actualPerson);
        }

        [Fact]
        public async Task SaveAddress_ShouldSucceed()
        {
            // Arrange
            var personId = await InsertPersonInDb();
            var address = new AddressEntity
            {
                Street = "abcd",
                City = "abcd",
                Country = "abcd",
                PersonId = personId,
                Zip = "abcd"
            };

            // Act
            await _database.SaveAddress(address);

            // Assert
            var actualAddress = await _database.Address.FindAsync(address.Id);
            Assert.NotNull(actualAddress);
        }

        [Fact]
        public async Task UpdateLoan_ShouldSucceed()
        {
            // Arrange
            var personId = await InsertPersonInDb();

            // Act
            await _database.UpdateLoan(personId, 12345);

            // Assert
            var actualLoan = await _database.Loans.FirstOrDefaultAsync(loan => loan.PersonId == personId);
            Assert.NotNull(actualLoan);
            Assert.Equal(12345, actualLoan.Amount);
        }



        private async Task<int> InsertPersonInDb()
        {
            var person = new PersonEntity
            {
                FirstName = "abcd",
                LastName = "abcd",
                SocialSecurityNumber = "1234"
            };
            await _database.Persons.AddAsync(person);
            await _database.SaveChangesAsync();
            _personIdsToDelete.Add(person.Id);
            return person.Id;
        }

        public void Dispose()
        {
            // Dispose is always called after a completed test, regardless of the result. So this is a good place to clean up data after tests.
            foreach (var personId in _personIdsToDelete)
            {
                try
                {
                    var personToDelete = _database.Persons.Find(personId);
                    if (personToDelete != null)
                    {
                        _database.Persons.Remove(personToDelete);
                    }

                    _database.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Log and Mute this exception - we don't want unhandled exceptions in Dispose.
                }
            }
        }
    }
}
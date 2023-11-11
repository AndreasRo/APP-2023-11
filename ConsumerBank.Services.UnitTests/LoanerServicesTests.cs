using ConsumerBank.Services.Contracts;
using ConsumerBank.Services.DbObjects;
using Moq;
using Xunit;

namespace ConsumerBank.Services.UnitTests
{
    public class LoanerServicesTests
    {
        private readonly Mock<IDatabase> _databaseServiceMock;
        private readonly LoanerService _service;

        public LoanerServicesTests()
        {
            _databaseServiceMock = new Mock<IDatabase>();
            _service = new LoanerService(_databaseServiceMock.Object);
        }

        // Bruker [Theory] i steden for [Fact] fordi Theory lar deg angi input-parametere med InlineData-attributter.
        // Tester for b�de true og false for � verifisere at det er resultatet fra creditProvider som returneres.
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Apply_ValidRequest_ShouldReturnCreditEvaluationResult(bool evaluationResult)
        {
            // Arrange
            var request = new LoanRequest
            {
                Person = new Person { FirstName = "Andreas" }
            };

            // Act
            var actualResult = await _service.Apply(request);

            // Assert
            Assert.Equal(evaluationResult, actualResult);
        }

        // Merk at alle tester er delt i 3:
        // Arrange - inneholder all kode for oppsett.
        // Act - trigger koden som skal testes. Typisk bare en linje kode her.
        // Assert - her er alle Assert og Verify-statements. I denne delen avgj�res det om testen har feilet eller ikke.

        [Fact]
        public async Task Apply_ValidPersonData_PersonIsStoredInDb()
        {
            // Arrange
            var request = new LoanRequest
            {
                Person = new Person { FirstName = "Andreas" }
            };

            // Act
            await _service.Apply(request);

            // Assert
            AssertPersonDataWasSaved(firstName: request.Person.FirstName);
        }

        [Fact]
        public async Task Apply_ValidPersonData_AddressIsStoredInDb()
        {
            // Arrange
            var request = new LoanRequest
            {
                Person = new Person
                {
                    CustomerAddress = new Address { Street = "Storgata 42" }
                },
                Amount = 100
            };

            // Act
            await _service.Apply(request);

            // Assert
            AssertAddressWasSaved(street: request.Person.CustomerAddress.Street);
        }

        [Fact]
        public async Task Apply_ValidPersonData_LoanWasUpdated()
        {
            // Arrange
            SetupPersonIdReturned(42);

            var request = new LoanRequest
            {
                Person = new Person(),
                Amount = 100
            };

            // Act
            await _service.Apply(request);

            // Assert
            AssertLoanWasUpdated(42, request.Amount);
        }

        private void AssertLoanWasUpdated(int personId, int requestAmount)
        {
            _databaseServiceMock.Verify(x => x.UpdateLoan(personId, requestAmount));
        }

        private void SetupPersonIdReturned(int personId)
        {
            _databaseServiceMock
                .Setup(x => x.SavePerson(It.IsNotNull<PersonEntity>()))
                .ReturnsAsync(42);
        }

        private void AssertAddressWasSaved(string street)
        {
            // Kan legge til sjekk p� flere adresseparametere her om �nskelig.
            _databaseServiceMock.Verify(x => x.SaveAddress(It.Is<AddressEntity>(address => address.Street == street)));
        }

        private void AssertPersonDataWasSaved(string firstName)
        {
            _databaseServiceMock
                .Verify(x => x.SavePerson(
                    It.Is<PersonEntity>(personEntity => personEntity.FirstName == firstName)));
        }
    }
}
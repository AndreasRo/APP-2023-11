using System.Text;
using ConsumerBank.Services.Contracts;
using Newtonsoft.Json;

namespace ConsumerBankApi.RegressionTests
{
    public class LoansControllerTests
    {
        // BaseAddress vil normalt være en konfigurerbar setting som forandrer seg ettersom hvilket miljø som testes. Men for eksempelets skyld kaller vi her localhost
        private const string BaseAddress = "http://localhost:5269/";

        // Denne testen rydder ikke opp etter seg - en regresjonstest vil typisk ha egne test subjects som benyttes hver gang.
        // Denne testen vil feile hvis/når testressursene i Azure blir slettet....
        [Theory]
        [InlineData(50000, true)]
        [InlineData(150000, false)]
        public async Task Apply_RegressionTest_ShouldNotFail(int loanAmount, bool expectedResult)
        {
            // Arrange
            var request = new LoanRequest
            {
                Amount = loanAmount,
                Person = new Person
                {
                    FirstName = "RegressionTestFirstName",
                    CustomerAddress = new Address
                    {
                        City = "abcd",
                        Country = "abcd",
                        Street = "abcd",
                        Zip = "abcd"
                    },
                    LastName = "RegressionTestLastName",
                    SocialSecurityNumber = "abcd",
                }
            };

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseAddress);

            // Act
            using var response = await httpClient.PostAsync("loans/apply", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.True(bool.TryParse(resultString, out var result) && result == expectedResult);
        }
    }
}
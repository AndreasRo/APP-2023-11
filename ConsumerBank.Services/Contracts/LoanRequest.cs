namespace ConsumerBank.Services.Contracts
{
    public class LoanRequest
    {
        public int Amount { get; set; }
        public Person Person { get; set; } = new Person();
    }
}
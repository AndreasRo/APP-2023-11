namespace ConsumerBank.Services.DbObjects
{
    public class LoanEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public decimal Amount { get; set; }
    }
}
namespace ConsumerBank.Services.DbObjects
{
    public class AddressEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string Zip { get; set; } = "";
        public string Country { get; set; } = "";
    }
}
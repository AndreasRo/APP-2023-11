namespace ConsumerBank.Services.Contracts
{
    public class Person
    {
        public string SocialSecurityNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public Address CustomerAddress { get; set; } = new Address();

    }
}
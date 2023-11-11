
namespace ConsumerBank.Services.DbObjects
{
    public class PersonEntity
    {
        public int Id { get; set; }
        public string SocialSecurityNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
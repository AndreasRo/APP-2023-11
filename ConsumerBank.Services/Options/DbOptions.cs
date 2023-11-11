namespace ConsumerBank.Services.Options;

public class DbOptions
{
    public DbOptions(string database, string username, string password)
    {
        Password = password;
        Username = username;
        Database = database;
    }
    
    public string Database { get; set; }
    public string Username { get; set; }
    public string Password  { get; set; }

}
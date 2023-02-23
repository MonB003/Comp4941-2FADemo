namespace _2FADemo.Models
{
    // Represents a user in the database
    public class User
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Key { get; set; }
    }
}

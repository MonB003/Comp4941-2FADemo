namespace _2FADemo.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Key { get; set; }
    }
}

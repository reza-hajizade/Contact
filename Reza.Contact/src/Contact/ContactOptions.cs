namespace Contact
{
    public class ContactOptions
    {

    }

    // Settings for connecting to RabbitMQ 
    public sealed class BrokerOptions
    {
        // Section name in appsettings.json
        public const string SectionName = "BrokerOptions";

        public required string Host { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

}

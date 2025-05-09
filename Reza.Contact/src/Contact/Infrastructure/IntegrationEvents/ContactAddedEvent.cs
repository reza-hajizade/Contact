namespace UserInfo.Infrastructure.Consumer.IntegrationEvents;

// Event sent when a new contact is created for a user

public record ContactAddedEvent(string phoneNumber, int UserId, DateTime OccurredOn);




namespace GiftExchange;

public record ParticipantRecord(
    int Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? WishListURL,
    string? SpouseName
);
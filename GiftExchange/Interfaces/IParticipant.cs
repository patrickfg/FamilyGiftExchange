using GiftExchange;

namespace GiftExchange.Interfaces;

public interface IParticipant {
    int Id { get; }
    string FirstName { get; }
    string LastName { get; }
    string FullName { get; }
    string PhoneNumber { get; }
    string? WishListURL { get; }
    string? SpouseName { get; }

    IParticipant[] Exclusions { get; }
    IParticipant? AssignedRecipient { get; set; }
    void AddExclusion(IParticipant participant);
    ParticipantRecord ToRecord();
}
namespace GiftExchange.Interfaces;

/// <summary>
/// Defines the contract for a participant in the gift exchange, including their personal information, exclusions, and assigned recipient.
/// </summary>
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
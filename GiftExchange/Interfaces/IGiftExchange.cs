namespace GiftExchange.Interfaces;

/// <summary>
/// Defines the contract for a gift exchange, which includes a list of participants and a method to randomize their assigned recipients.
/// </summary>
public interface IGiftExchange {
	/// <summary>
	/// Gets the list of participants in the gift exchange.
	/// </summary>
	IParticipant[] Participants { get; }

	/// <summary>
	/// Randomizes the assigned recipients for each participant, ensuring that no participant is assigned to themselves or to any of their exclusions.
	/// </summary>
	void RandomizeRecipients();
}
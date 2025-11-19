namespace GiftExchange.Interfaces;

/// <summary>
/// Defines the contract for a participant source, which is responsible for providing participant data and handling persistence.
/// </summary>
public interface IParticipantSource {
    IParticipant[] GetParticipants();

	void AddParticipant(IParticipant participant);

	void LoadParticipants(string fileName);
	
	void RemoveParticipant(IParticipant participant);
    
	void SaveParticipants(string fileName);
}
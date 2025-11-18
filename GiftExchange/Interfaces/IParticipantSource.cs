namespace GiftExchange.Interfaces;

/// <summary>
/// Defines the contract for a participant source, which is responsible for providing participant data and handling persistence.
/// </summary>
public interface IParticipantSource {
    IParticipant[] GetParticipants();
    void LoadParticipants(string fileName);
    void SaveParticipants(string fileName);
}
namespace GiftExchange.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IParticipantSource {
    IParticipant[] GetParticipants();
    void LoadParticipants(string fileName);
    void SaveParticipants(string fileName);
}
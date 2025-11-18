using Serilog;
using GiftExchange.Interfaces;

namespace GiftExchange;

public class ParticipantSource : IParticipantSource {
    private List<IParticipant> _participants = new List<IParticipant>();

    public IParticipant[] GetParticipants(){
        return _participants.ToArray();
    }

    public void LoadParticipants(string fileName){
        Log.Debug("Loading participants from file: {FileName}", fileName);

        if (!File.Exists(fileName))
        {
            Log.Error("File not found: {FileName}", fileName);
            return;
        }

        try
        {
            var json = File.ReadAllText(fileName);
            var participants = System.Text.Json.JsonSerializer.Deserialize<ParticipantRecord[]>(json);

            if (participants is null || !participants.Any())
            {
                Log.Warning("No participants found in file: {FileName}", fileName);
                return;
            }

            List<IParticipant> loadedParticipants = new List<IParticipant>();
            foreach (var record in participants)
            {
                loadedParticipants.Add(new Participant(record));
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error loading participants from file: {FileName}", fileName);
        }
    }

    public void SaveParticipants(string fileName)
    {
        
    }
}
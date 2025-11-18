using GiftExchange.Interfaces;
using Serilog;

namespace GiftExchange;

public class GiftExchange : IGiftExchange
{
    // private readonly IParticipantSource _participantSource;
    private readonly IParticipant[] _participants;

    private readonly Random _random = new Random();

    public GiftExchange(IParticipantSource participantSource)
    {
        // _participantSource = participantSource;

        _participants = participantSource.GetParticipants();
    }

    public IParticipant[] Participants => _participants.ToArray();

    public void RandomizeRecipients()
    {
        Log.Debug("Starting gift recipient randomization");
        ValidateParticipants();

        int maxAttempts = _participants.Length - 1;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Log.Debug("Randomization attemp {Attemp} of {Max}", attempt + 1, maxAttempts);
            ResetAssignments();

            if (TryRandomizeAssignments())
            {
                return;
            }
        }
    }

    private bool TryRandomizeAssignments()
    {
        throw new NotImplementedException();
    }

    private void ValidateParticipants()
    {
        throw new NotImplementedException();
        
    }

    private void ResetAssignments()
    {
        throw new NotImplementedException();
        
    }
}
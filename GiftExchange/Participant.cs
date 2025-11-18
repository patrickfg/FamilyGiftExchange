using GiftExchange.Interfaces;

namespace GiftExchange;

public class Participant : IParticipant
{
    private readonly List<IParticipant> _exclusions = new List<IParticipant>();

    public Participant(int id, string firstName, string lastName, string phoneNumber, string? wishListURL = null, string? spouseName = null)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        WishListURL = wishListURL;
        SpouseName = spouseName;
    }
    public int Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string FullName => $"{FirstName} {LastName}";

    public string PhoneNumber { get; }

    public string? WishListURL { get; }

    public string? SpouseName { get; }

    public IParticipant[] Exclusions => _exclusions.ToArray();
    public IParticipant? AssignedRecipient { get; set; }

    public void AddExclusion(IParticipant participant)
    {
        throw new NotImplementedException();
    }

    public ParticipantRecord ToRecord()
    {
        throw new NotImplementedException();
    }
}
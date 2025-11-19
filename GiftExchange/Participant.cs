using GiftExchange.Interfaces;

namespace GiftExchange;

public class Participant : IParticipant {
	private readonly List<IParticipant> _exclusions = new List<IParticipant>();

	public Participant(int id, string firstName, string lastName, string phoneNumber, string? wishListURL = null, string? spouseName = null) {
		if (string.IsNullOrWhiteSpace(firstName))
			throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));
		if (string.IsNullOrWhiteSpace(lastName))
			throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));
		if (string.IsNullOrWhiteSpace(phoneNumber))
			throw new ArgumentException("Phone number cannot be null or whitespace.", nameof(phoneNumber));

		Id = id;
		FirstName = firstName;
		LastName = lastName;
		PhoneNumber = phoneNumber;
		WishListURL = wishListURL;
		SpouseName = spouseName;
	}

	public Participant(ParticipantRecord record) {
		Id = record.Id;
		FirstName = record.FirstName;
		LastName = record.LastName;
		PhoneNumber = record.PhoneNumber;
		WishListURL = record.WishListURL;
		SpouseName = record.SpouseName;
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

	public void AddExclusion(IParticipant participant) {
		if (participant == null)
			throw new ArgumentNullException(nameof(participant));
		else if (_exclusions.Contains(participant))
			throw new InvalidOperationException("Participant is already in the exclusion list.");
		_exclusions.Add(participant);
	}

	public ParticipantRecord ToRecord() {
		return new ParticipantRecord(
			Id,
			FirstName,
			LastName,
			PhoneNumber,
			WishListURL,
			SpouseName
		);
	}
}
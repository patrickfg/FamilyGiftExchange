using System;
using System.Collections.Generic;
using System.Text;

namespace GiftExchange.Tests;

public class ParticipantTests {

	[SetUp]
	public void Setup() {
		TestLogger.Initialize();
	}

	[Test]
	public void Participant_Creation_Succeeds() {
		var participant = new Participant(1, "First", "Last", "+1234567890", "http://example.com/wishlist", "Spouse Name");
		Assert.That(participant.Id, Is.EqualTo(1));
		Assert.That(participant.FirstName, Is.EqualTo("First"));
		Assert.That(participant.LastName, Is.EqualTo("Last"));
		Assert.That(participant.PhoneNumber, Is.EqualTo("+1234567890"));
		Assert.That(participant.WishListURL, Is.Not.Null);
		Assert.That(participant.WishListURL, Is.EqualTo("http://example.com/wishlist"));
		Assert.That(participant.SpouseName, Is.EqualTo("Spouse Name"));
	}

	[Test]
	public void Participant_Creation_FromRecord_Succeeds() {
		var record = new ParticipantRecord(
			1,
			"First",
			"Last",
			"+1234567890",
			"http://example.com/wishlist",
			"Spouse Name"
		);

		var participant = new Participant(record);
		Assert.That(participant.Id, Is.EqualTo(record.Id));
		Assert.That(participant.FirstName, Is.EqualTo(record.FirstName));
		Assert.That(participant.LastName, Is.EqualTo(record.LastName));
		Assert.That(participant.PhoneNumber, Is.EqualTo(record.PhoneNumber));
		Assert.That(participant.WishListURL, Is.Not.Null);
		Assert.That(participant.WishListURL, Is.EqualTo(record.WishListURL));
		Assert.That(participant.SpouseName, Is.EqualTo(record.SpouseName));
	}

	[TestCase("")]
	[TestCase(" ")]
	[TestCase("  ")]
	[TestCase("\t")]
	public void Participant_Requires_NonEmpty_FirstName(string  firstName) {
		Assert.Throws<ArgumentException>(() => new Participant(1, firstName, "Last", "+1234567890"));
	}

	public void Participant_Requires_NonNull_FirstName() {
		Assert.Throws<ArgumentException>(() => new Participant(1, null, "Last", "+1234567890"));
	}

	[TestCase("")]
	[TestCase(" ")]
	[TestCase("  ")]
	[TestCase("\t")]
	public void Participant_Requires_NonEmpty_LastName(string lastName) {
		Assert.Throws<ArgumentException>(() => new Participant(1, "First", lastName, "+1234567890"));
	}

	public void Participant_Requires_NonNull_LastName() {
		Assert.Throws<ArgumentException>(() => new Participant(1, "First", null, "+1234567890"));
	}

	[TestCase("")]
	[TestCase(" ")]
	[TestCase("  ")]
	[TestCase("\t")]
	public void Participant_Requires_NonEmpty_PhoneNumber(string phoneNumber) {
		Assert.Throws<ArgumentException>(() => new Participant(1, "First", "Last", phoneNumber));
	}

	public void Participant_Requires_NonNull_PhoneNumber() {
		Assert.Throws<ArgumentException>(() => new Participant(1, "First", "Last", null));
	}

	public void Participant_AddExclusion_Null_Throws() {
		var participant = new Participant(1, "First", "Last", "+1234567890");
		Assert.Throws<ArgumentNullException>(() => participant.AddExclusion(null!));
	}

	public void Participant_AddExclusion_Duplicate_Throws() {
		var participant = new Participant(1, "First", "Last", "+1234567890");
		var exclusion = new Participant(2, "Excl", "Participant", "+1987654321");
		participant.AddExclusion(exclusion);
		Assert.Throws<InvalidOperationException>(() => participant.AddExclusion(exclusion));
	}

	public void Participant_AddExclusion_Succeeds() {
		var participant = new Participant(1, "First", "Last", "+1234567890");
		var exclusion = new Participant(2, "Excl", "Participant", "+1987654321");
		participant.AddExclusion(exclusion);
		Assert.That(participant.Exclusions.Length, Is.EqualTo(1));
		Assert.That(participant.Exclusions[0], Is.EqualTo(exclusion));
	}

	public void Participant_ToRecord_Succeeds() {
		var participant = new Participant(1, "First", "Last", "+1234567890", "http://example.com/wishlist", "Spouse Name");
		var record = participant.ToRecord();
		Assert.That(record.Id, Is.EqualTo(participant.Id));
		Assert.That(record.FirstName, Is.EqualTo(participant.FirstName));
		Assert.That(record.LastName, Is.EqualTo(participant.LastName));
		Assert.That(record.PhoneNumber, Is.EqualTo(participant.PhoneNumber));
		Assert.That(record.WishListURL, Is.EqualTo(participant.WishListURL));
		Assert.That(record.SpouseName, Is.EqualTo(participant.SpouseName));
	}
}

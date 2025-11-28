using GiftExchange.Interfaces;

namespace GiftExchange.Tests;

public class GiftExchangeTests {
	[SetUp]
	public void Setup() {
		TestLogger.Initialize();

	}

	[Test]
	public void HappyPathTests_ScenarioA() {
		
		// Arrange
		var jane = new Participant(1, "Jane", "Doe", "+12345678900", "https://wishlist.example.com/janedoe657/2025", "John Doe");
		var john = new Participant(2, "John", "Doe", "+12345678901", "https://wishlist.example.com/johndoe7829/2025", "Jane Doe");
		var bridget = new Participant(3, "Bridget", "Jones", "+19876543210", "https://wishlist.example.com/bridgetjones29/2025", "Daniel Jones");
		var daniel = new Participant(4, "Daniel", "Jones", "+19876543210", "https://wishlist.example.com/danieljones43/2025", "Bridget Jones");
		var howard =new Participant(5, "Howard", "Jones", "+19876543211", "https://wishlist.example.com/howardjones1/2025", null);
		var tom =new Participant(6, "Tom", "Jones", "+19513577931", "https://wishlist.example.com/thomasjones324/2025", null);

		IParticipant[] participants = [jane, john, bridget, daniel, howard, tom];
		var source = new ParticipantSource(participants);

		var giftExchange = new GiftExchange(source);

		// Act
		giftExchange.RandomizeRecipients();
		var results = giftExchange.Participants;

		// Assert

		// Everyone should have someone to buy a gift for
		Assert.That(results.All(p => p.AssignedRecipient is not null));

		// No one should have gotten themselves
		Assert.That(results.All(p => p.Id != p.AssignedRecipient!.Id));

		// No one should have gotten their spouse
		Assert.That(jane.AssignedRecipient!.Id != john.Id);
		Assert.That(john.AssignedRecipient!.Id != jane.Id);
		Assert.That(bridget.AssignedRecipient!.Id != daniel.Id);
		Assert.That(daniel.AssignedRecipient!.Id != bridget.Id);

		// Make sure everyone is getting a gift
		Assert.That(results.Select(p => p.AssignedRecipient!.Id).Distinct().Count(), Is.EqualTo(participants.Length));
	}
}

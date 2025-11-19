using System.Diagnostics;

namespace GiftExchange.Tests;

public class MessageBuilderTests {

	[SetUp]
	public void Setup() {
		TestLogger.Initialize();
	}

	[Test]
	public void ParticipantIsRequired() { 
		var messageBuilder = new MessageBuilder();
		Assert.Throws<ArgumentNullException>(() => messageBuilder.CreateMessage(null!));
	}

	[Test]
	public void AssignedRecipientIsRequired() {
		var messageBuilder = new MessageBuilder();
		var participant = new Participant(1, "Test", "User", "+1234567890");
		Assert.Throws<InvalidOperationException>(() => messageBuilder.CreateMessage(participant));
	}

	[Test]
	public void MessageWithWishlistIsCreatedSuccessfully() {
		var messageBuilder = new MessageBuilder();
		var recipient = new Participant(2, "Recipient", "User", "+1234567891", "http://example.com/wishlist");
		var participant = new Participant(1, "Test", "User", "+1234567890") {
			AssignedRecipient = recipient
		};
		string message = messageBuilder.CreateMessage(participant);
		Debug.WriteLine(message);
		Assert.That(message, Does.Contain("Hello Test"));
		Assert.That(message, Does.Contain("you were chosen to get a gift for Recipient"));
		Assert.That(message, Does.Contain("http://example.com/wishlist"));
	}

	[Test]
	public void MessageWithoutWishlistIsCreatedSuccessfully() {
		var messageBuilder = new MessageBuilder();
		var recipient = new Participant(2, "Recipient", "User", "+1234567891");
		var participant = new Participant(1, "Test", "User", "+1234567890", "http://example.com/wishlist") {
			AssignedRecipient = recipient
		};
		string message = messageBuilder.CreateMessage(participant);
		Debug.WriteLine(message);
		Assert.That(message, Does.Contain("Hello Test"));
		Assert.That(message, Does.Contain("you were chosen to get a gift for Recipient"));
		Assert.That(message, Does.Not.Contain("http://example.com/wishlist"));
	}
}

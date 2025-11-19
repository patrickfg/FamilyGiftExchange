using GiftExchange.Interfaces;

namespace GiftExchange.Tests;

public class ParticipantSourceTests {
	
	[SetUp]
	public void Setup() {
		TestLogger.Initialize();
	}

	private IParticipant[] ReferenceParticipants {
		get {
			var jane = new Participant(1, "Jane", "Doe", "+12345678900", "https://wishlist.example.com/janedoe657/2025", "John Doe");
			var john = new Participant(2, "John", "Doe,", "+12345678901", "https://wishlist.example.com/johndoe7829/2025", "Jane Doe");
			var bridget = new Participant(3, "Bridget", "Jones,", "+19876543210", "https://wishlist.example.com/bridgetjones29/2025", "Daniel Jones");
			var daniel = new Participant(4, "Daniel", "Jones,", "+19876543210", "https://wishlist.example.com/danieljones43/2025", "Bridget Jones");
			var howard = new Participant(5, "Howard", "Jones,", "+19876543211", "https://wishlist.example.com/howardjones1/2025", null);
			var tom = new Participant(6, "Tom", "Jones,", "+19513577931", "https://wishlist.example.com/thomasjones324/2025", null);
			return new IParticipant[] { jane, john, bridget, daniel, howard, tom };
		}
	}

	[Test]
	public void TestParticipantSource_BasicInMemoryTest() {
		// In Memory test of ParticipantSource
		// Arrange
		var participants = ReferenceParticipants;

		// Act
		var source = new ParticipantSource(participants);
		var retrievedParticipants = source.GetParticipants();

		// Assert
		Assert.That(retrievedParticipants.Length, Is.EqualTo(participants.Length));
		for (int i = 0; i < participants.Length; i++) {
			Assert.That(retrievedParticipants[i].Id, Is.EqualTo(participants[i].Id));
			Assert.That(retrievedParticipants[i].FirstName, Is.EqualTo(participants[i].FirstName));
			Assert.That(retrievedParticipants[i].LastName, Is.EqualTo(participants[i].LastName));
			Assert.That(retrievedParticipants[i].PhoneNumber, Is.EqualTo(participants[i].PhoneNumber));
			Assert.That(retrievedParticipants[i].WishListURL, Is.EqualTo(participants[i].WishListURL));
			Assert.That(retrievedParticipants[i].SpouseName, Is.EqualTo(participants[i].SpouseName));
		}
	}

	[Test]
	public void TestParticipantSource_NullParticipants_ThrowsException() {
		// Arrange, Act & Assert
		Assert.Throws<ArgumentNullException>(() => new ParticipantSource(null!));
	}

	[Test]
	public void TestParticipantSource_EmptyParticipants_ThrowsException() {
		// Arrange, Act & Assert
		Assert.Throws<ArgumentException>(() => new ParticipantSource(new IParticipant[] { }));
	}

	[Test]
	public void TestParticipantSource_DuplicateParticipantIds_ThrowsException() {
		// Arrange
		var participants = new IParticipant[] {
			new Participant(1, "Jane", "Doe", "+12345678900"),
			new Participant(1, "John", "Doe", "+12345678901")
		};
		// Act & Assert
		Assert.Throws<ArgumentException>(() => new ParticipantSource(participants));
	}
	[Test]
	public void TestParticiantSource_PersistenceTest() {
		var participants = ReferenceParticipants;

		var writeSource = new ParticipantSource(participants);
		string tempFile = Path.GetTempFileName();
		try {
			writeSource.SaveParticipants(tempFile);
			var readSource = new ParticipantSource();
			readSource.LoadParticipants(tempFile);
			var retrievedParticipants = readSource.GetParticipants();
			Assert.That(retrievedParticipants.Length, Is.EqualTo(participants.Length));
			for (int i = 0; i < participants.Length; i++) {
				Assert.That(retrievedParticipants[i].Id, Is.EqualTo(participants[i].Id));
				Assert.That(retrievedParticipants[i].FirstName, Is.EqualTo(participants[i].FirstName));
				Assert.That(retrievedParticipants[i].LastName, Is.EqualTo(participants[i].LastName));
				Assert.That(retrievedParticipants[i].PhoneNumber, Is.EqualTo(participants[i].PhoneNumber));
				Assert.That(retrievedParticipants[i].WishListURL, Is.EqualTo(participants[i].WishListURL));
				Assert.That(retrievedParticipants[i].SpouseName, Is.EqualTo(participants[i].SpouseName));
			}
		} finally {
			if (File.Exists(tempFile)) {
				File.Delete(tempFile);
			}
		}
	}
}

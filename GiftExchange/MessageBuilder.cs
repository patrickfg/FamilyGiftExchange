using GiftExchange.Interfaces;
using System.Text;

namespace GiftExchange;

public class MessageBuilder : IMessageBuilder {
	public string CreateMessage(IParticipant participant) {
		if (participant == null) {
			throw new ArgumentNullException(nameof(participant));
		}

		if (participant.AssignedRecipient == null) {
			throw new InvalidOperationException("Participant has no assigned recipient.");
		}

		StringBuilder messageBuilder = new StringBuilder();
		messageBuilder.Append($"Hello {participant.FirstName}, for the {DateTime.Now.Year} Christmas gift exchange, you were chosen to get a gift for {participant.AssignedRecipient.FirstName}");
		if (!string.IsNullOrWhiteSpace(participant.AssignedRecipient.WishListURL)) {
			messageBuilder.AppendLine();
			messageBuilder.AppendLine("You can find some ideas from their wish list at:");
			messageBuilder.Append(participant.AssignedRecipient.WishListURL);
		}

		return messageBuilder.ToString();
	}
}

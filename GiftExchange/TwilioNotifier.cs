using GiftExchange.Interfaces;
using Serilog;

namespace GiftExchange;

public class TwilioNotifier : INotifier {
	private readonly TwilioConfiguration _configuration;
	private readonly IMessageBuilder _messageBuilder;

	public TwilioNotifier(IMessageBuilder messageBuilder, TwilioConfiguration configuration) {
		if (messageBuilder == null) {
			throw new ArgumentNullException(nameof(messageBuilder));
		}

		if (configuration == null) {
			throw new ArgumentNullException(nameof(configuration));
		}

		_messageBuilder = messageBuilder;
		_configuration = configuration;
	}

	public void NotifyParticipant(IParticipant participant) {
		if (string.IsNullOrWhiteSpace(participant.PhoneNumber)) {
			throw new ArgumentException($"Participant {participant.FullName} does not have a valid phone number.");
		}

		string message = _messageBuilder.CreateMessage(participant);
		SendMessage(participant, message);
	}

	private void SendMessage(IParticipant participant, string message) {
		Log.Information("Sending message to {PhoneNumber}: {Message}", participant.PhoneNumber, message);

		if (_configuration.Simulate) {
			Log.Information("Simulation mode is ON. Message not sent.");
			return;
		}


		string toPhoneNumber = _configuration.OverrideToPhoneNumberWith ?? participant.PhoneNumber;
		Twilio.TwilioClient.Init(_configuration.TwilioAccountSid, _configuration.TwilioAuthToken);
		var messageResource = Twilio.Rest.Api.V2010.Account.MessageResource.Create(
			body: message,
			from: new Twilio.Types.PhoneNumber(_configuration.OriginPhoneNumber),
			to: new Twilio.Types.PhoneNumber(toPhoneNumber)
		);

		Log.Information("Message sent to {PhoneNumber}. SID: {MessageSid}", toPhoneNumber, messageResource.Sid);
		Thread.Sleep(_configuration.DelayBetweenMessagesInMilliseconds);
	}
}

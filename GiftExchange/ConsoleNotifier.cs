using GiftExchange.Interfaces;

namespace GiftExchange;

public class ConsoleNotifier : INotifier {
	IMessageBuilder _MessageBuilder;
	public ConsoleNotifier(IMessageBuilder messageBuilder) {
		_MessageBuilder = messageBuilder;
	}

	public void NotifyParticipant(IParticipant participant) {
		Console.WriteLine("--------------------------------------------------");
		Console.WriteLine(_MessageBuilder.CreateMessage(participant));
		Console.WriteLine("--------------------------------------------------");
		Console.WriteLine();
	}
}

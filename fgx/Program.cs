using CommandLine;
using fgx;
using GiftExchange;
using GiftExchange.Interfaces;
using Serilog;

public class Program {
	public static int Main(string[] args) {

		var loggerConfiguration = new LoggerConfiguration()
		.MinimumLevel.Debug()
		.WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
		.WriteTo.File(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, path: "GiftExchange.log");


		Log.Logger = loggerConfiguration.CreateLogger();

		Log.Information("*****  Gift exchange starting at {time}  *****", DateTime.Now);
		var options = new Options();
		var result = Parser.Default.ParseArguments<Options>(args)
			.MapResult(
					options => RunAndReturnExitCode(options),
					errors => ReportParseErrors(errors)
			);

		Log.Debug("*****  Gift exchange exiting at {time}  *****", DateTime.Now);

		Log.CloseAndFlush();
		return result;
	}

	private static int RunAndReturnExitCode(Options options) {
		try {
			Console.WriteLine($"Input file: {options.InputFile}");
			Console.WriteLine($"Notification method: {options.NotificationMethod}");
			if (options.MockPhoneNumber != null) {
				Console.WriteLine($"Mock phone number: {options.MockPhoneNumber}");
			}

			var source = new ParticipantSource();
			source.LoadParticipants(options.InputFile);
			var participants = source.GetParticipants();

			IGiftExchange exchanger = new GiftExchange.GiftExchange(source);
			INotifier notifier = CreateNotifier(options);

			exchanger.RandomizeRecipients();
			foreach (var participant in exchanger.Participants) {
				Console.WriteLine($"{participant.FullName}'s phone number is {participant.PhoneNumber}");
				notifier.NotifyParticipant(participant);
			}

			return 0;
		}
		catch (Exception e) {
			Log.Error(e, "Fatal error.");
			return -2;
		}
	}

	private static int ReportParseErrors(IEnumerable<Error> errors) {
		
		Log.Error("Failed to parse command-line arguments.");

		foreach (var error in errors) {
			Log.Error("Parser Error: {@error}", error);
		}
		return -1;
	}

	private static INotifier CreateNotifier(Options options) {
		if (options.NotificationMethod.StartsWith("sms")) {
			return GetTwilioNotifier(options);
		}
		else if (options.NotificationMethod.StartsWith("text")) {
			return GetTwilioNotifier(options);
		}
		else {
			return GetConsoleNotifier();
		}
	}

	private static INotifier GetConsoleNotifier() {
		return new ConsoleNotifier(new MessageBuilder());
	}

	private static INotifier GetTwilioNotifier(Options options) {
		var configuration = TwilioConfiguration.FromFile();
		var messageBuilder = new MessageBuilder();
		if (!string.IsNullOrEmpty(options.MockPhoneNumber)) {
			configuration.OverrideToPhoneNumberWith = options.MockPhoneNumber;
		}
		return new TwilioNotifier(messageBuilder, configuration);
	}
}
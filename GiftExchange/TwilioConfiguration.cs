using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GiftExchange;

[Serializable]
public class TwilioConfiguration {

	public static TwilioConfiguration FromFile(string fileName = "Twilio.giftexchange.config") {
		if (!File.Exists(fileName)) {
			Log.Error("Twilio configuration file not found. Filename: {FileName}. Current directory: {CurrentDirectory}", fileName, Environment.CurrentDirectory);
			throw new FileNotFoundException("Configuration file not found", fileName);
		}

		var json = File.ReadAllText(fileName);
		var configuration = JsonSerializer.Deserialize<TwilioConfiguration>(json);

		if (configuration is null) {
			throw new InvalidOperationException("No configuration data found.");
		}

		return configuration;
	}

	[JsonPropertyName("fromPhoneNumber")]
	public required string OriginPhoneNumber { get; set; }

	public string? OverrideToPhoneNumberWith { get; set; }

	public bool Simulate { get; set; }

	[JsonIgnore]
	public string TwilioAccountSid => Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID") ?? throw new ArgumentNullException(nameof(TwilioAccountSid));

	[JsonIgnore]
	public string TwilioAuthToken => Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN") ?? throw new ArgumentNullException(nameof(TwilioAuthToken));

	public int DelayBetweenMessagesInMilliseconds { get; internal set; } = 1250;
}

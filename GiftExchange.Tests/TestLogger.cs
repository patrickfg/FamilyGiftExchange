using Serilog;

namespace GiftExchange.Tests;

internal static class TestLogger {
	private static bool _isInitialized = false;

	public static void Initialize() {
		if (_isInitialized) {
			return;
		}

		// Configure logging for tests
		var loggerConfiguration = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.Console();

		Log.Logger = loggerConfiguration.CreateLogger();

		_isInitialized = true;
	}
}

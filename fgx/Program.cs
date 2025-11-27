// See https://aka.ms/new-console-template for more information
using CommandLine;
using fgx;

public class Program {
	public static int Main(string[] args) {
		var options = new Options();
		var result = CommandLine.Parser.Default.ParseArguments<Options>(args)
			.MapResult(
					options => RunAndReturnExitCode(options),
					errors => ReportParseErrors(errors)
			);
		return result;
	}

	private static int RunAndReturnExitCode(Options options) {
		// Implement the main logic of your application here.
		Console.WriteLine($"Input file: {options.InputFile}");
		Console.WriteLine($"Notification method: {options.NotificationMethod}");
		if (options.MockPhoneNumber != null) {	
			Console.WriteLine($"Mock phone number: {options.MockPhoneNumber}");	
		}	
		return 0;
	}

	private static int ReportParseErrors(IEnumerable<Error> errors) {
		
		Console.WriteLine("Failed to parse command-line arguments.");

		foreach (var error in errors) {
			Console.WriteLine(error.ToString());
		}
		return -1;
	}
}
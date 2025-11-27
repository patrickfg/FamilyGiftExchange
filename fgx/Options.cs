using CommandLine;

namespace fgx;

public class Options {
    [Option('f', "file", Required = true, HelpText = "Path to the input file containing participant information.")]
    public string InputFile { get; set; } = null!;


    [Option('n', "notification-method", Required = false, HelpText = "Method to notify participants (e.g., email, sms).")]
    public string NotificationMethod { get; set; } = "print";

    [Option("mock-phone-number", Required = false, HelpText = "Mock phone number for testing SMS notifications.")]
    public string? MockPhoneNumber { get; set; } = null!;
}
using System.CommandLine;

namespace gitbit
{
    internal class CommandLineParser
    {
        public string? Command { get; }

        public CommandLineParser(string[] args)
        {
            Option<string> setCommand = new("--cmd")
            {
                Description = "Command - the main funciton to carry out."
            };

            RootCommand rootCommand = new("gitbit command line utility");
            rootCommand.Options.Add(setCommand);

            ParseResult parseResult = rootCommand.Parse(args);

            if (parseResult.GetValue(setCommand) is string parsedCommand)
            {
                Console.WriteLine($"Running command: {parsedCommand}");
            }
            else
            {
                throw new ArgumentException($"Must specify a command");
            }
        }
    }
}

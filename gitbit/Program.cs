using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;


namespace gitbit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());

            Console.OutputEncoding = Encoding.UTF8;
            string pipeInput = string.Empty;

            if (Console.IsInputRedirected)
            {
                using (var reader = Console.In)
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine($"Pipe input: {line}");
                        pipeInput = line;
                    }
                }
            }

            // add something to confirm branch name convention

            CommandLineParser cmdParsers = new(args);

            string consoleInputFile = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "CONIN$" : "/dev/tty";
            string commitMessage = string.Empty;

            using (Stream inputStream = new FileStream(consoleInputFile, FileMode.Open, FileAccess.Read))
            {
                Console.SetIn(new StreamReader(inputStream));

                Console.WriteLine("\nCommit message: ");
                string? input = Console.ReadLine();
                Console.WriteLine($"User input: {input}");
                commitMessage = input == null ? string.Empty : input;
            }

            using (Process gitcmd = new Process())
            {
                gitcmd.StartInfo.FileName = "git.exe";
                gitcmd.StartInfo.Arguments = $"commit -m \"{pipeInput}: [AD] - {commitMessage}\"";
                gitcmd.StartInfo.UseShellExecute = false;
                gitcmd.StartInfo.RedirectStandardOutput = true;
                gitcmd.Start();
                Console.WriteLine(gitcmd.StandardOutput.ReadToEnd());
                gitcmd.WaitForExit();
            }
        }
    }
}

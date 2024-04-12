using System.Diagnostics;

namespace WebBuilder2.Server.Utils;

public static class ScriptRunner
{
    public static async Task<bool> RunAsync(string scriptPath, params string[] args)
    {
        string shellArguments = $"/c \"{scriptPath}\" {string.Join(' ', args)}";

        // Create a new process to run the shell script
        Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = shellArguments,
                RedirectStandardOutput = true, // Redirect standard output for capturing script output
                UseShellExecute = false, // Don't use the system shell
                CreateNoWindow = true // Don't create a window
            }
        };

        // Start the process
        process.Start();

        // Wait for the process to exit asynchronously
        await process.WaitForExitAsync();

        // Check if the process exited successfully (exit code 0)
        return process.ExitCode == 0;
    }
}

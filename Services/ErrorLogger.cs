using System;
using System.IO;
using System.Threading.Tasks;

namespace DM_Notes.Services
{
    public static class ErrorLogger
    {
        private static readonly string LogFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DM_Notes", "error.log");

        /// <summary>
        /// Error-Logging
        /// </summary>
        /// <param name="context">In welcher Methode ist der Fehler aufgetreten</param>
        /// <param name="ex">Error-Text</param>
        public static async Task LogAsync(string context, Exception ex)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath)!);
                var message = $"[{DateTime.Now:G}] {context}: {ex}\n";
                await Task.Run(() => File.AppendAllText(LogFilePath, message));
            }
            catch (Exception loggingFailed)
            {
                await Console.Error.WriteLineAsync($"[Logger] Failed: {loggingFailed.Message}");
            }
        }
    }
}
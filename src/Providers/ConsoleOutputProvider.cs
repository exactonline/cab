using System;

namespace ConsoleAppBase.Providers
{
    /// <summary>
    /// A provider for outputting messages.
    /// </summary>
    public class ConsoleOutputProvider : IOutputProvider
    {
        /// <summary>
        /// Show an information level message.
        /// </summary>
        /// <param name="text">The text to show.</param>
        public void ShowInformation(string text)
        {
            Show(text, Console.ForegroundColor);
        }

        /// <summary>
        /// Show a warning level message.
        /// </summary>
        /// <param name="text">The text to show.</param>
        public void ShowWarning(string text)
        {
            Show(text, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Show a error level message.
        /// </summary>
        /// <param name="text">The text to show.</param>
        public void ShowError(string text)
        {
            Show(text, ConsoleColor.Red);
        }

        private void Show(string text, ConsoleColor color)
        {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previous;
        }
    }
}

namespace ConsoleAppBase.Providers
{
    /// <summary>
    /// Defines the provider for outputting messages.
    /// </summary>
    public interface IOutputProvider
    {
        /// <summary>
        /// Show an information level message.
        /// </summary>
        /// <param name="text">The text to show.</param>
        void ShowInformation(string text);

        /// <summary>
        /// Show a warning level message.
        /// </summary>
        /// <param name="text">The text to show.</param>
        void ShowWarning(string text);

        /// <summary>
        /// Show a error level message.
        /// </summary>
        /// <param name="text">The text to show.</param>
        void ShowError(string text);
    }
}

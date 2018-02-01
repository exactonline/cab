namespace ConsoleAppBase.Providers
{
    /// <summary>
    /// Defines a provider for retrieving info about the actual application executing.
    /// </summary>
    public interface IAppInfoProvider
    {
        /// <summary>
        /// Gets the short version of the version information.
        /// </summary>
        string ShortVersion { get; }

        /// <summary>
        /// Gets or sets the long version of the version information.
        /// </summary>
        string LongVersion { get; set; }

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        string AppName { get; set; }
    }
}

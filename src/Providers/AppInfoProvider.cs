using System.Reflection;

namespace ConsoleAppBase.Providers
{
    internal class AppInfoProvider : IAppInfoProvider
    {
        /// <summary>
        /// Metadata provider for Command.
        /// </summary>
        /// <param name="assemblyProvider">Provider which determines the execution entrypoint.</param>
        public AppInfoProvider(IAssemblyProvider assemblyProvider)
        {
            _assemblyInfo = assemblyProvider.MainAssembly.GetName();
        }

        /// <summary>
        /// Gets the version of the application from the assembly information.
        /// </summary>
        public string ShortVersion
        {
            get => _assemblyInfo.Version.ToString();
        }

        private string _longVersion;
        /// <summary>
        /// Gets or sets the customised version of the application, defaults to the short version.
        /// </summary>
        public string LongVersion
        {
            get => _longVersion ?? ShortVersion;
            set => _longVersion = value;
        }

        private string _appName;
        private AssemblyName _assemblyInfo;

        /// <summary>
        /// Gets or sets the application name, defaults to the name provided in the assembly information.
        /// </summary>
        public string AppName
        {
            get => _appName ?? _assemblyInfo.Name;
            set => _appName = value;
        }
    }
}

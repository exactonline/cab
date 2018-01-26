using System.Reflection;

namespace ConsoleAppBase.Providers
{
    internal class AssemblyProvider : IAssemblyProvider
    {
        /// <summary>
        /// Gets the execution starting point of the application.
        /// </summary>
        public Assembly MainAssembly
        {
            get => Assembly.GetEntryAssembly();
        }
    }
}

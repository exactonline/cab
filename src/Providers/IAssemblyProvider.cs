using System.Reflection;

namespace ConsoleAppBase.Providers
{
    internal interface IAssemblyProvider
    {
        /// <summary>
        /// Gets the execution starting point of the application.
        /// </summary>
        Assembly MainAssembly { get; }
    }
}

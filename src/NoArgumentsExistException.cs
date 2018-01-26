using System;

namespace ConsoleAppBase
{
    /// <summary>
    /// The exception that is thrown when an argument is provided to a command without argument properties.
    /// </summary>
    [Serializable]
    internal class NoArgumentsExistException : Exception
    {
        public NoArgumentsExistException()
            : base("No arguments exist for this command.")
        { }
    }
}

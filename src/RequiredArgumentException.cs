using System;

namespace ConsoleAppBase
{
    /// <summary>
    /// The exception that is thrown when not all required arguments of a command are present.
    /// </summary>
    [Serializable]
    internal class RequiredArgumentException : Exception
    {
        public RequiredArgumentException(string argumentName)
            : base($"Argument '{argumentName}' is required.")
        { }
    }
}

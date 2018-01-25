using System;

namespace ConsoleAppBase.Attributes
{
    /// <summary>
    /// Class attribute to define a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        /// <remarks>If not set the class name will be used instead.</remarks>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the command (e.g. shown in usage messages).
        /// </summary>
        public string Description { get; set; }
    }
}

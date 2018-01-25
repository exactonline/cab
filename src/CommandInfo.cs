using ConsoleAppBase.Attributes;
using System;

namespace ConsoleAppBase
{
    /// <summary>
    /// Base command types definition class.
    /// </summary>
    /// <typeparam name="T">The attribute to include in the definition.</typeparam>
    public class BaseInfo<T> where T : Attribute
    {
        public T Attribute { get; set; }
        public Type Type { get; set; }
    }

    /// <summary>
    /// Describes a command.
    /// </summary>
    public class CommandInfo : BaseInfo<CommandAttribute> { }

    /// <summary>
    /// Describes a command argument.
    /// </summary>
    public class CommandArgumentInfo : BaseInfo<CommandArgumentAttribute> { }

    /// <summary>
    /// Describes a command option.
    /// </summary>
    public class CommandOptionInfo : BaseInfo<CommandOptionAttribute> { }

}

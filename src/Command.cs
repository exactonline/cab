using ConsoleAppBase.Attributes;
using ConsoleAppBase.Providers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleAppBase
{
    /// <summary>
    /// Defines a command for a console enabled application.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Gets or sets the templated identifier(s) of the help command option.
        /// </summary>
        public string HelpOptionTemplate { get; set; }

        /// <summary>
        /// Gets or sets the templated identifier(s) of the version command option.
        /// </summary>
        public string VersionOptionTemplate { get; set; }

        /// <summary>
        /// Gets or sets the provider used to find information about the actual application.
        /// </summary>
        public IAppInfoProvider AppInfo { get; set; }

        /// <summary>
        /// Gets or sets the provider used to output messages.
        /// </summary>
        public IOutputProvider Output { get; set; }

        /// <summary>
        /// Executes the command or a subcommand with parsing and validating the given arguments.
        /// </summary>
        /// <param name="args">The arguments to execute the command or subcommand with.</param>
        /// <returns>
        /// Return value usefull in console applications, usually 0 is OK and anything else is NOT OK.
        /// </returns>
        public int Execute(string[] args)
        {
            return 1;
        }

        /// <summary>
        /// Meant to be overriden and perform the execute steps of this command, after all the properties have values 
        /// from the parsed arguments and have been validated.
        /// </summary>
        /// <returns>
        /// Return value usefull in console applications, usually 0 is OK and anything else is NOT OK.
        /// </returns>
        public virtual int OnExecute()
        {
            ShowUsage();
            return 1;
        }

        /// <summary>
        /// Shows the help for this command.
        /// </summary>
        public void ShowHelp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows the usage for this command.
        /// </summary>
        public void ShowUsage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows the version of the application
        /// </summary>
        public void ShowVersion()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a <see cref="CommandInfo"/> description of this instance.
        /// </summary>
        public CommandInfo GetInfo()
        {
            return GetInfo(GetType());
        }

        /// <summary>
        /// Get a list of <see cref="CommandInfo"/> descriptions of the parent commands of this instance.
        /// </summary>
        public IEnumerable<CommandInfo> GetParentCommands()
        {
            return GetParentCommands(GetType());
        }

        /// <summary>
        /// Get a list of <see cref="CommandInfo"/> descriptions of the sub-commands of this instance.
        /// </summary>
        public IEnumerable<CommandInfo> GetSubcommands()
        {
            return GetSubcommands(GetType());
        }

        /// <summary>
        /// Get a list of <see cref="CommandArgumentInfo"/> descriptions of the command arguments of this instance.
        /// </summary>
        public IEnumerable<CommandArgumentInfo> GetArguments()
        {
            return GetArguments(GetType());
        }

        /// <summary>
        /// Get a list of <see cref="CommandOptionInfo"/> descriptions of the command options of this instance.
        /// </summary>
        public IEnumerable<CommandOptionInfo> GetOptions()
        {
            return GetOptions(GetType());
        }

        /// <summary>
        /// Get a list of <see cref="CommandArgumentInfo"/> descriptions of the required command arguments of this instance.
        /// </summary>
        public IEnumerable<CommandArgumentInfo> GetRequiredArguments()
        {
            return GetRequiredArguments(GetType());
        }

        private CommandInfo GetInfo(Type commandType)
        {
            var attr = commandType.GetCustomAttribute<CommandAttribute>();

            if (attr == null) return null;

            var info = new CommandInfo()
            {
                Attribute = commandType.GetCustomAttribute<CommandAttribute>(),
                Type = commandType
            };

            // if still not set, set it to the command type name
            info.Attribute.Name = info.Attribute.Name ?? commandType.Name;

            return info;
        }

        private IEnumerable<CommandInfo> GetParentCommands(Type type)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CommandInfo> GetSubcommands(Type type)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CommandArgumentInfo> GetArguments(Type type)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CommandArgumentInfo> GetRequiredArguments(Type type)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CommandOptionInfo> GetOptions(Type type)
        {
            throw new NotImplementedException();
        }
    }
}

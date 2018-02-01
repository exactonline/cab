using ConsoleAppBase.Attributes;
using ConsoleAppBase.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleAppBase
{
    /// <summary>
    /// Defines a command for a console enabled application.
    /// </summary>
    public abstract class Command
    {
        private readonly ArgumentParser _argumentParser = new ArgumentParser();

        /// <summary>
        /// Gets or sets the description used in the help information.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the templated identifier(s) of the help command option.
        /// </summary>
        public virtual string HelpOptionTemplate { get; set; } = "-h|--help";

        /// <summary>
        /// Gets or sets the templated identifier(s) of the version command option.
        /// </summary>
        public virtual string VersionOptionTemplate { get; set; } = "-v|--version";

        private IAppInfoProvider _appInfo;

        /// <summary>
        /// Gets or sets the provider used to find information about the actual application.
        /// </summary>
        public IAppInfoProvider AppInfo
        {
            get => _appInfo = _appInfo ?? new AppInfoProvider(AssemblyProvider);
            set => _appInfo = value;
        }

        private IOutputProvider _outputProvider;

        /// <summary>
        /// Gets or sets the provider used to output messages.
        /// </summary>
        public IOutputProvider Output
        {
            protected get => _outputProvider = _outputProvider ?? new ConsoleOutputProvider();
            set => _outputProvider = value;
        }

        private Help _help;

        /// <summary>
        /// Gets or sets the provider used to generate help information.
        /// </summary>
        public Help Help
        {
            get => _help = _help ?? new Help(this);
            set => _help = value;
        }

        private IAssemblyProvider _assemblyProvider;

        /// <summary>
        /// Gets or sets the execution starting point of the application.
        /// </summary>
        internal IAssemblyProvider AssemblyProvider
        {
            private get => _assemblyProvider = _assemblyProvider ?? new AssemblyProvider();
            set => _assemblyProvider = value;
        }

        /// <summary>
        /// Executes the command or a subcommand with parsing and validating the given arguments.
        /// </summary>
        /// <param name="args">The arguments to execute the command or subcommand with.</param>
        /// <returns>
        /// Return value usefull in console applications, usually 0 is OK and anything else is NOT OK.
        /// </returns>
        public int Execute(string[] args)
        {
            try
            {
                return ParseAndExecute(this, args);
            }
            catch (Exception ex) when (
                ex is RequiredArgumentException ||
                ex is NoArgumentsExistException ||
                ex is System.ComponentModel.DataAnnotations.ValidationException)
            {
                Output.ShowError(ex.Message);
            }

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
            Output.ShowInformation(Help.Full);
        }

        /// <summary>
        /// Shows the usage for this command.
        /// </summary>
        public void ShowUsage()
        {
            Output.ShowInformation(Help.Usage);
        }

        /// <summary>
        /// Shows the version of the application
        /// </summary>
        public void ShowVersion()
        {
            Output.ShowInformation(Help.Version);
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

        /// <summary>
        /// Gets all argument properties of a given command.
        /// </summary>
        /// <param name="commandType">The type of command from where the argument properties will be retrieved.</param>
        /// <returns>key value pairs of the attribute and accompanying propertyinfo.</returns>
        internal static Dictionary<CommandArgumentAttribute, PropertyInfo> GetArgumentProperties(Type commandType)
        {
            return (from p in commandType.GetRuntimeProperties()
                    let attr = p.GetCustomAttribute<CommandArgumentAttribute>()
                    let tmp = attr != null ? attr.Name = attr.Name ?? p.Name : null
                    where attr != null
                    orderby attr.Position
                    select new { Info = p, Attribute = attr })
                    .ToDictionary(k => k.Attribute, v => v.Info);
        }

        /// <summary>
        /// Gets all option properties of a given command.
        /// </summary>
        /// <param name="commandType">The type of command from where the option properties will be retrieved.</param>
        /// <returns>key value pairs of the attribute and accompanying propertyinfo.</returns>
        internal static Dictionary<CommandOptionAttribute, PropertyInfo> GetOptionProperties(Type commandType)
        {
            return (from p in commandType.GetRuntimeProperties()
                    let attr = p.GetCustomAttribute<CommandOptionAttribute>()
                    let tmp = attr != null ? attr.Template = attr.Template ?? $"-{p.Name}" : null
                    where attr != null
                    orderby attr.Template
                    select new { Info = p, Attribute = attr }).ToDictionary(k => k.Attribute, v => v.Info);
        }

        private int ParseAndExecute(Command command, string[] args)
        {
            if (args == null || !args.Any())
            {
                var firstRequiredArgument = GetRequiredArguments(command.GetType()).FirstOrDefault();
                if (firstRequiredArgument != null)
                    throw new RequiredArgumentException(firstRequiredArgument.Attribute.Name);

                return command.OnExecute();
            }

            // check if subcommand 
            var subcommands = GetSubcommands(command.GetType());
            var firstArgument = args.First().ToLower();
            var subCommandType = subcommands?.FirstOrDefault(c => c.Attribute.Name.ToLower() == firstArgument)?.Type;

            if (subCommandType != null)
            {
                var subCommandInst = Activator.CreateInstance(subCommandType) as Command;

                return ParseAndExecute(subCommandInst, args.Skip(1).ToArray());
            }

            if (_argumentParser.IsHelpOption(args, command))
            {
                command.ShowUsage();
                return 0;
            }

            if (command.GetType().BaseType == typeof(Command) && _argumentParser.IsVersionOption(args, command))
            {
                command.ShowVersion();
                return 0;
            }

            _argumentParser.Parse(args, command);

            return command.OnExecute();
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
            return GetParentCommandTypes(type, AssemblyProvider).Select(GetInfo);
        }

        private IEnumerable<CommandInfo> GetSubcommands(Type type)
        {
            return GetSubcommandTypes(type, AssemblyProvider).Select(GetInfo);
        }

        private IEnumerable<CommandArgumentInfo> GetArguments(Type type)
        {
            return GetArgumentProperties(type).Select(a => new CommandArgumentInfo()
            {
                Attribute = a.Key,
                Type = a.Value.DeclaringType
            });
        }

        private IEnumerable<CommandArgumentInfo> GetRequiredArguments(Type type)
        {
            return GetArguments(type)
                .Where(a => a.Attribute.Required);
        }

        private IEnumerable<CommandOptionInfo> GetOptions(Type type)
        {
            return GetOptionProperties(type).Select(a => new CommandOptionInfo()
            {
                Attribute = a.Key,
                Type = a.Value.DeclaringType
            });
        }

        private static IEnumerable<Type> GetParentCommandTypes(Type commandType, IAssemblyProvider assemblyProvider)
        {
            if (commandType == null) return Enumerable.Empty<Type>();

            var parentType = commandType.BaseType;

            if (parentType == typeof(Command))
                return Enumerable.Empty<Type>();

            var commandAttr = parentType.GetCustomAttribute<CommandAttribute>();

            var list = new List<Type>();

            if (commandAttr != null)
            {
                list.Add(parentType);

                var parentsOfParent = GetParentCommandTypes(parentType, assemblyProvider);
                list.AddRange(parentsOfParent);
            }
            else
            {
                var parentsOfParentBase = GetParentCommandTypes(parentType.BaseType, assemblyProvider);
                list.AddRange(parentsOfParentBase);
            }

            return list.ToArray();
        }

        private static IEnumerable<Type> GetSubcommandTypes(Type commandType, IAssemblyProvider assemblyProvider)
        {
            var assembly = assemblyProvider.MainAssembly;
            var types = assembly.GetTypes();
            var childCommands = types.Where(t => t.BaseType == commandType);
            var baseCommands = childCommands.Where(t => t.GetCustomAttribute<CommandAttribute>() == null);
            var inheritingCommands = baseCommands.SelectMany(b => GetSubcommandTypes(b, assemblyProvider));
            return childCommands.Except(baseCommands).Concat(inheritingCommands);
        }
    }
}

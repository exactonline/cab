using ConsoleAppBase.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleAppBase
{
    internal class HelpBuilder
    {
        private Command _command;
        private Help _helpText;

        private IEnumerable<CommandAttribute> _subCommandsHelp;
        private IEnumerable<CommandArgumentAttribute> _argumentsHelp;
        private IEnumerable<CommandOptionAttribute> _optionsHelp;

        /// <summary>
        /// Gathers information of all attributes in a <see cref="Command"/> and stores it in a <see cref="Help"/>.
        /// </summary>
        /// <param name="command">The Command from which the information is retrieved.</param>
        /// <param name="helpText">The Help where all attribute information is stored.</param>
        public HelpBuilder(Command command, Help helpText)
        {
            _command = command;
            _helpText = helpText;
        }

        /// <summary>
        /// Construction of all help information in the Help object related to the Command.
        /// </summary>
        public void BuildHelp()
        {
            _subCommandsHelp = _command.GetSubcommands().Select(c => c.Attribute).OrderBy(c => c.Name);
            _argumentsHelp = _command.GetArguments().Select(a => a.Attribute);
            _optionsHelp = _command.GetOptions().Select(kv => kv.Attribute).OrderBy(a => a.Template);

            var builder = new StringBuilder();
            builder
                .AppendLine(_command.AppInfo.AppName)
                .AppendLine(VersionInformation())
                .AppendLine(UsageInformation());

            _helpText.Full = builder.ToString();
        }

        private string UsageInformation()
        {
            var str = new StringBuilder().AppendLine().Append($"Usage: " + _command.AppInfo.AppName);

            var parents = _command.GetParentCommands();
            foreach (var item in parents)
            {
                str.Append($" {item.Attribute.Name}");
            }

            var commandInfo = _command.GetInfo()?.Attribute.Name;
            str.Append(commandInfo != null ? $" {commandInfo}" : "")
                .Append(_subCommandsHelp.Any() ? " [command]" : "")
                .Append(_argumentsHelp.Any() ? " [arguments]" : "")
                .Append(" [options]");

            if (_subCommandsHelp.Any())
                str.Append(SubCommandsHelp());

            if (_argumentsHelp.Any())
                str.Append(ArgumentsHelp());

            str.Append(OptionsHelp());

            _helpText.Usage = str.ToString();
            return _helpText.Usage;
        }

        private string VersionInformation()
        {
            _helpText.Version = _command.AppInfo.ShortVersion;
            return new StringBuilder().AppendLine(_command.AppInfo.LongVersion).ToString();
        }

        private string SubCommandsHelp()
        {
            var table = CreateTableString(_subCommandsHelp.ToDictionary(k => k.Name, v => v.Description));
            return new StringBuilder()
                .AppendLine().AppendLine()
                .AppendLine("Commands:")
                .Append(table).ToString();
        }

        private string ArgumentsHelp()
        {
            string requiredDescription(CommandArgumentAttribute attr)
            {
                return attr.Required ? $"{attr.Description} [required]" : attr.Description;
            }

            var table = CreateTableString(_argumentsHelp.ToDictionary(k => k.Name, v => requiredDescription(v)));

            return new StringBuilder()
                .AppendLine().AppendLine()
                .AppendLine("Arguments:")
                .Append(table).ToString();
        }

        private string OptionsHelp()
        {
            var options = new List<CommandOptionAttribute>() {
                new CommandOptionAttribute() {
                    Template = _command.HelpOptionTemplate, Description = "Show help information"
                }
            };

            if (_command.GetType().BaseType == typeof(Command))
            {
                options.Add(new CommandOptionAttribute()
                {
                    Template = _command.VersionOptionTemplate,
                    Description = "Show version information"
                });
            }

            options.AddRange(_optionsHelp);

            var table = CreateTableString(options.ToDictionary(k => k.Template, v => v.Description));

            return new StringBuilder()
                .AppendLine().AppendLine()
                .AppendLine("Options:")
                .Append(table).ToString();
        }

        private string CreateTableString(Dictionary<string, string> details)
        {
            var longest = details.Keys.OrderByDescending(i => i.Length).First().Length;
            var prepared = details.Select(a => new StringBuilder(MatchIndentationFor(a.Key, longest)).AppendLine(a.Value).ToString());
            return string.Join("", prepared).TrimEnd();
        }

        private string MatchIndentationFor(string attribute, int length)
        {
            return "  " + attribute + new String(' ', length + 2 - attribute.Length);
        }
    }
}

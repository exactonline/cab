using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleAppBase
{
    internal class ArgumentParser
    {
        /// <summary>
        /// Parses the given arguments for the related command and fills all arguments and options.
        /// </summary>
        /// <param name="args">The arguments to execute the command or subcommand with.</param>
        /// <param name="command">The command or subcommand on which the parsing is applied.</param>
        public void Parse(string[] args, Command command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if the arguments contain the custom help template.
        /// </summary>
        /// <param name="args">The arguments passed in from commandline.</param>
        /// <param name="command">The command on which the parsing is applied.</param>
        /// <returns>true if the arguments contain the custom help template, else false.</returns>
        internal bool IsHelpOption(IEnumerable<string> args, Command command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if the arguments contain the custom version template.
        /// </summary>
        /// <param name="args">The arguments passed in from commandline.</param>
        /// <param name="command">The command on which the parsing is applied.</param>
        /// <returns>true if the arguments contain the custom version template, else false.</returns>
        internal bool IsVersionOption(IEnumerable<string> args, Command command)
        {
            throw new NotImplementedException();
        }

        private bool IsAnyMatchingTemplate(IEnumerable<string> args, string template)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<string> FillAllArguments(IEnumerable<string> args, Command command)
        {
            throw new NotImplementedException();
        }
        
        private void FillAllOptions(IEnumerable<string> args, Command command)
        {
            throw new NotImplementedException();
        }

        private object ConvertValueForProperty(PropertyInfo property, string arg)
        {
            throw new NotImplementedException();
        }

        private bool IsOption(string arg, Command command)
        {
            throw new NotImplementedException();
        }

        private PropertyInfo FindOptionProperty(string arg, Command command)
        {
            throw new NotImplementedException();
        }

        private bool OptionTemplateContains(string template, string arg)
        {
            throw new NotImplementedException();
        }

        private bool IsSimpleType(Type type)
        {
            throw new NotImplementedException();
        }
    }
}

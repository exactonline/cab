using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            if (args == null || !args.Any()) return;

            var optionOnlyValues = FillAllArguments(args, command);

            FillAllOptions(optionOnlyValues, command);

            Validator.ValidateObject(command, new ValidationContext(command), validateAllProperties: true);
        }

        /// <summary>
        /// Check if the arguments contain the custom help template.
        /// </summary>
        /// <param name="args">The arguments passed in from commandline.</param>
        /// <param name="command">The command on which the parsing is applied.</param>
        /// <returns>true if the arguments contain the custom help template, else false.</returns>
        internal bool IsHelpOption(IEnumerable<string> args, Command command)
        {
            return IsAnyMatchingTemplate(args, command.HelpOptionTemplate);
        }

        /// <summary>
        /// Check if the arguments contain the custom version template.
        /// </summary>
        /// <param name="args">The arguments passed in from commandline.</param>
        /// <param name="command">The command on which the parsing is applied.</param>
        /// <returns>true if the arguments contain the custom version template, else false.</returns>
        internal bool IsVersionOption(IEnumerable<string> args, Command command)
        {
            return IsAnyMatchingTemplate(args, command.VersionOptionTemplate);
        }

        private bool IsAnyMatchingTemplate(IEnumerable<string> args, string template)
        {
            return args.Any(a => OptionTemplateContains(template, a));
        }

        /// <summary>
        /// Parses passed in arguments and fills all properties with the <see cref="CommandArgumentAttribute"/> Attribute.
        /// </summary>
        /// <param name="args">passed in arguments.</param>
        /// <param name="command"></param>
        /// <returns>remaining arguments which were not filled in properties.</returns>
        private IEnumerable<string> FillAllArguments(IEnumerable<string> args, Command command)
        {
            var arguments = Command.GetArgumentProperties(command.GetType()).Select(s => s.Value).ToArray();
            if (!arguments.Any())
            {
                if (IsOption(args.FirstOrDefault(), command))
                {
                    return args;
                }
                throw new NoArgumentsExistException();
            }

            var counter = 0;

            foreach (var value in args)
            {
                if (IsOption(value, command))
                {
                    break;
                }

                var argument = arguments[counter];
                object propValue = ConvertValueForProperty(argument, value);
                argument.SetValue(command, propValue);
                counter++;
            }

            var nextRequiredArgument = command.GetRequiredArguments().Skip(counter).FirstOrDefault();
            if (nextRequiredArgument != null)
            {
                throw new RequiredArgumentException(nextRequiredArgument.Attribute.Name);
            }

            return args.Skip(counter);
        }
        
        private void FillAllOptions(IEnumerable<string> args, Command command)
        {
            PropertyInfo option = null;
            foreach (var arg in args)
            {
                var matchingOption = FindOptionProperty(arg, command);

                if (matchingOption != null)
                {
                    if (matchingOption.PropertyType == typeof(bool))
                    {
                        matchingOption.SetValue(command, true);
                    }
                    else
                    {
                        option = matchingOption;
                    }
                }
                else
                {
                    object propValue = ConvertValueForProperty(option, arg);
                    option.SetValue(command, propValue);
                }
            }
        }

        private object ConvertValueForProperty(PropertyInfo property, string arg)
        {
            var propertyType = property.PropertyType;

            if (propertyType.IsEnum)
                return Enum.Parse(propertyType, arg);
            else if (IsSimpleType(propertyType))
                return Convert.ChangeType(arg, propertyType);
            else
                return JsonConvert.DeserializeObject(arg, propertyType);
        }

        private bool IsOption(string arg, Command command)
        {
            if (string.IsNullOrEmpty(arg)) return false;

            return FindOptionProperty(arg, command) != null;
        }

        private PropertyInfo FindOptionProperty(string arg, Command command)
        {
            var options = Command.GetOptionProperties(command.GetType());
            return options.FirstOrDefault(o => OptionTemplateContains(o.Key.Template, arg)).Value;
        }

        private bool OptionTemplateContains(string template, string arg)
        {
            return template.Split('|').Contains(arg);
        }

        private bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }
    }
}

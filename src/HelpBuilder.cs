using System;
using System.Collections.Generic;

namespace ConsoleAppBase
{
    internal class HelpBuilder
    {
        /// <summary>
        /// Gathers information of all attributes in a <see cref="Command"/> and stores it in a <see cref="Help"/>.
        /// </summary>
        /// <param name="command">The Command from which the information is retrieved.</param>
        /// <param name="helpText">The Help where all attribute information is stored.</param>
        public HelpBuilder(Command command, Help helpText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Construction of all help information in the Help object related to the Command.
        /// </summary>
        public void BuildHelp()
        {
            throw new NotImplementedException();
        }

        private string UsageInformation()
        {
            throw new NotImplementedException();
        }

        private string VersionInformation()
        {
            throw new NotImplementedException();
        }

        private string SubCommandsHelp()
        {
            throw new NotImplementedException();
        }

        private string ArgumentsHelp()
        {
            throw new NotImplementedException();
        }

        private string OptionsHelp()
        {
            throw new NotImplementedException();
        }

        private string CreateTableString(Dictionary<string, string> details)
        {
            throw new NotImplementedException();
        }

        private string MatchIndentationFor(string attribute, int length)
        {
            throw new NotImplementedException();
        }
    }
}

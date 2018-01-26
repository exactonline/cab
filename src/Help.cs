using System;

namespace ConsoleAppBase
{
    public class Help
    {
        private Command _command;
        private HelpBuilder _builder;
        private string _full;
        private string _version;
        private string _usage;

        private void Build()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Help object used to get help information on the given Command.
        /// </summary>
        /// <param name="command">Command from which the help information is derived.</param>
        public Help(Command command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helptext containing information on all subcommands, arguments and options.
        /// </summary>
        public string Full { get; set; }

        /// <summary>
        /// Version of the console application.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Shows the usage of the current Command.
        /// </summary>
        public string Usage { get; set; }
    }
}

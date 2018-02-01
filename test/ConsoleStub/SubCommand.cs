using ConsoleAppBase.Attributes;

namespace ConsoleAppBase.Test.Unit.ConsoleStub
{
    [Command(Name= "subcommand")]
    class SubCommand : MainCommand
    {
        [CommandArgument(0, Name = "simple-argument", Description = "A simple argument")]
        public string Simple { get; set; }

        [CommandArgument(1, Name = "custom-argument", Description = "A custom type argument")]
        public CustomObject Custom { get; set; }

        [CommandOption(Template = "-s|--long", Description = "A multi template option")]
        public string MultiTemplateOption { get; set; }

        [CommandOption(Template = "--long-type-option")]
        public long LongOption { get; set; }

        public override int OnExecute()
        {
            return GetType().GetHashCode();
        }
    }
}

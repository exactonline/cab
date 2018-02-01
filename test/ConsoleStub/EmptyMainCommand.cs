using ConsoleAppBase.Attributes;

namespace ConsoleAppBase.Test.Unit.ConsoleStub
{
    class EmptyMainCommand : Command
    {
        [CommandArgument(0)]
        int Argument { get; set; }

        public override int OnExecute()
        {
            return Argument;
        }
    }
}

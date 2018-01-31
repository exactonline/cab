using ConsoleAppBase.Attributes;

namespace ConsoleAppBase.Test.Unit.ConsoleStub
{
    [Command(Name = "inherited-command", Description = "A command used to test the nesting functionality")]
    class NestedFunctionalityCommand : BaseFunctionalityCommand
    {
        public override int OnExecute()
        {
            return this.InheritanceScore + 2;
        }
    }
}

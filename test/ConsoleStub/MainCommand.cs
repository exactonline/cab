namespace ConsoleAppBase.Test.Unit.ConsoleStub
{
    class MainCommand : Command
    {
        public MainCommand()
        {
            AssemblyProvider = new TestAssemblyProvider();
        }
    }
}

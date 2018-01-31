using System.Reflection;
using ConsoleAppBase.Providers;
using ConsoleAppBase.Test.Unit.ConsoleStub;

namespace ConsoleAppBase.Test.Unit
{
    internal class TestAssemblyProvider : IAssemblyProvider
    {
        public Assembly MainAssembly => typeof(MainCommand).Assembly;
    }
}

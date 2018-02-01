using ConsoleAppBase.Providers;
using ConsoleAppBase.Test.Unit.ConsoleStub;
using Moq;
using Xunit;

namespace ConsoleAppBase.Test.Unit
{
    public class CommandUnitTests
    {
        [Fact]
        public void Execution_OverridingExecuteOnMainCommand_ExecuteMainCommand()
        {
            //Arrange
            var arguments = new string[] { "2" };
            var command = new EmptyMainCommand();

            //Act
            var returned = command.Execute(arguments);

            //Assert
            Assert.Equal(2, returned);
        }

        [Fact]
        public void Execution_CallingASubCommandWithDifferentCasing_CallExecuteOnSubCommand()
        {
            //Arrange
            var arguments = new[] { "SUBCOMMAND" };
            var command = new MainCommand();

            //Act
            var hashCode = command.Execute(arguments);

            //Assert
            Assert.Equal(typeof(SubCommand).GetHashCode(), hashCode);
        }

        [Fact]
        public void Execution_CallingANestedSubCommand_CallExecuteOnNestedSubCommand()
        {
            //Arrange
            var arguments = new[] { "inherited-command" };
            var command = new MainCommand();

            //Act
            var sum = command.Execute(arguments);

            //Assert
            Assert.Equal(3, sum);
        }

        [Fact]
        public void Execution_CallingWithHelpOption_CallToHelpTextProvider()
        {
            //Arrange
            var outputMock = new Mock<IOutputProvider>();

            var arguments = new[] { "-h" };
            var command = new MainCommand()
            {
                Output = outputMock.Object
            };

            //Act
            command.Execute(arguments);

            //Assert
            outputMock.Verify(o => o.ShowInformation(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void Execution_CallingWithVersionOption_CallToVersionProvider()
        {
            //Arrange
            var appInfoMock = new Mock<IAppInfoProvider>();

            var arguments = new[] { "-v" };
            var command = new MainCommand()
            {
                AppInfo = appInfoMock.Object
            };

            //Act
            command.Execute(arguments);

            //Assert
            appInfoMock.Verify(a => a.LongVersion, Times.Once());
        }
    }
}

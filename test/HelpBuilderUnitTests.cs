using ConsoleAppBase.Providers;
using ConsoleAppBase.Test.Unit.ConsoleStub;
using Xunit;

namespace ConsoleAppBase.Test.Unit
{
    public class HelpBuilderUnitTests
    {
        [Fact]
        public void HelpBuilding_BuildingFullHelp_OutputsVersionInformationToHelp()
        {
            //Arrange
            var command = new SubCommand();
            var builder = new HelpBuilder(command, command.Help);

            //Act
            builder.BuildHelp();

            //Assert
            Assert.Equal("1.0.0.0", command.Help.Version);
        }

        [Fact]
        public void HelpBuilding_BuildingFullHelp_OutputsUsageInformationToHelp()
        {
            //Arrange
            var command = new SubCommand();
            command.AppInfo.AppName = "TestConsoleApp";

            var builder = new HelpBuilder(command, command.Help);

            //Act
            builder.BuildHelp();

            //Assert
            Assert.Contains("Usage: TestConsoleApp subcommand [arguments] [options]", command.Help.Usage);
        }

        [Fact]
        public void HelpBuilding_BuildingFullHelp_OutputsAllSubCommandsToHelp()
        {
            //Arrange
            var command = new MainCommand();
            var builder = new HelpBuilder(command, command.Help);

            //Act
            builder.BuildHelp();

            //Assert
            Assert.Contains("inherited-command", command.Help.Usage);
            Assert.Contains("required", command.Help.Usage);
            Assert.Contains("subcommand", command.Help.Usage);

        }

        [Fact]
        public void HelpBuilding_BuildingFullHelp_OutputsAllArgumentsToHelp()
        {
            //Arrange
            var command = new SubCommand();
            var builder = new HelpBuilder(command, command.Help);

            //Act
            builder.BuildHelp();

            //Assert
            Assert.Contains("simple-argument", command.Help.Usage);
            Assert.Contains("custom-argument", command.Help.Usage);
        }

        [Fact]
        public void HelpBuilding_BuildingFullHelp_OutputsAllOptionsToHelp()
        {
            //Arrange
            var command = new SubCommand();
            var builder = new HelpBuilder(command, command.Help);

            //Act
            builder.BuildHelp();

            //Assert
            Assert.Contains("-s|--long", command.Help.Usage);
            Assert.Contains("--long-type-option", command.Help.Usage);
        }
    }
}

using ConsoleAppBase.Providers;
using ConsoleAppBase.Test.Unit.ConsoleStub;
using Moq;
using Xunit;

namespace ConsoleAppBase.Test.Unit
{
    public class ArgumentParserUnitTests
    {
        [Fact]
        public void Parsing_ProvidingArgumentsToCommandWithoutArguments_NoArgumentExistsCalled()
        {
            //Arrange
            var outputMock = new Mock<IOutputProvider>();
            var errorMessage = new NoArgumentsExistException().Message;

            var arguments = new[] { "non-existing-command" };
            var command = new MainCommand()
            {
                Output = outputMock.Object
            };

            //Act
            command.Execute(arguments);

            //Assert
            outputMock.Verify(o => o.ShowError(It.Is<string>(s => s.Equals(errorMessage))), Times.Once());
        }

        [Fact]
        public void Parsing_ProvidingNoArgumentsToCommandWithRequiredArguments_RequiredArgumentMissingCalled()
        {
            //Arrange
            var outputMock = new Mock<IOutputProvider>();
            var errorMessage = new RequiredArgumentException("required-integer-argument").Message;

            var arguments = new string[] { };
            var command = new RequiredArgumentsCommand()
            {
                Output = outputMock.Object
            };

            //Act
            command.Execute(arguments);

            //Assert
            outputMock.Verify(o => o.ShowError(It.Is<string>(s => s.Equals(errorMessage))), Times.Once());
        }

        [Fact]
        public void Parsing_ProvidingNotEnoughArgumentsToCommandWithRequiredArguments_RequiredArgumentMissingCalled()
        {
            //Arrange
            var outputMock = new Mock<IOutputProvider>();
            var errorMessage = new RequiredArgumentException("required-string-argument").Message;

            var arguments = new[] { "1" };
            var command = new RequiredArgumentsCommand()
            {
                Output = outputMock.Object
            };

            //Act
            command.Execute(arguments);

            //Assert
            outputMock.Verify(o => o.ShowError(It.Is<string>(s => s.Equals(errorMessage))), Times.Once());
        }

        [Fact]
        public void Parsing_ProvidingArgumentOutsideOfDataAnnotationRange_CallToHelpTextProvider()
        {
            //Arrange
            var outputMock = new Mock<IOutputProvider>();
            var errorMessage = "The field RequiredInteger must be between 0 and 10.";

            var arguments = new string[] { "11", "requiredString" };
            var command = new RequiredArgumentsCommand()
            {
                Output = outputMock.Object
            };

            //Act
            var a = command.Execute(arguments);

            //Assert
            outputMock.Verify(o => o.ShowError(It.Is<string>(s => s.Equals(errorMessage))), Times.Once());
        }

        [Fact]
        public void Parsing_ProvidingOptionToCommandWithMultiTemplateOption_ValidateAgainstAllTargets()
        {
            //Arrange
            var shortArguments = new[] { "-s", "ShortValue" };
            var longArguments = new[] { "--long", "LongValue" };

            var command = new SubCommand();

            //Act
            command.Execute(shortArguments);
            var shortResult = command.MultiTemplateOption;

            command.Execute(longArguments);
            var longResult = command.MultiTemplateOption;

            //Assert
            Assert.Equal("ShortValue", shortResult);
            Assert.Equal("LongValue", longResult);
        }

        [Fact]
        public void Parsing_ProvidingArgumentToCommandWithArgumentAndOption_FillArgumentAndLeaveOptionEmpty()
        {
            //Arrange
            var arguments = new[] { "argumentValue" };
            var command = new SubCommand();

            //Act
            command.Execute(arguments);

            //Assert
            Assert.Equal("argumentValue", command.Simple);
            Assert.Null(command.MultiTemplateOption);
        }

        [Fact]
        public void Parsing_ProvidingSimpleTypeOption_FillOptionWithCorrectSimpleType()
        {
            //Arrange
            var arguments = new[] { "--long-type-option", long.MaxValue.ToString() };
            var command = new SubCommand();

            //Act
            command.Execute(arguments);

            //Assert
            Assert.Equal(long.MaxValue, command.LongOption);
        }

        [Fact]
        public void Parsing_ProvidingComplexTypeArgument_FillArgumentWithCorrectComplexType()
        {
            //Arrange
            var arguments = new[] { "", "{\"StringProperty\":\"StringValue\",\"IntegerProperty\":1}" };
            var command = new SubCommand();

            //Act
            command.Execute(arguments);

            //Assert
            Assert.Equal("StringValue", command.Custom.StringProperty);
            Assert.Equal(1, command.Custom.IntegerProperty);
        }
    }
}

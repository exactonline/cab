using ConsoleAppBase.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ConsoleAppBase.Test.Unit.ConsoleStub
{
    [Command(Name = "required", Description = "A command with an required argument")]
    class RequiredArgumentsCommand : MainCommand
    {
        [CommandArgument(1, Name = "required-integer-argument", Description = "A required integer type argument", Required = true)]
        [Range(0, 10)]
        public int RequiredInteger { get; set; }

        [CommandArgument(2, Name = "required-string-argument", Description = "A required string type argument", Required = true)]
        string RequiredString { get; set; }
    }
}

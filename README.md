# CAB (ConsoleAppBase)

Like many other console frameworks, CAB provides a basic layout for creating console applications. It is easy to set up, small in size, and is available for the recent .NET platforms.

Unlike other console frameworks for .NET, arguments that are passed into the console are not flattened and validated as text. Instead, they're formatted as structured data, that can directly be used by the application.

```csharp
[Command(Name = "example", Description = "A test command")]
class ExampleCommand : Command
    {
        [CommandArgument(1, Name = "divisible-int" Description = "A divising argument")]
        public int IntegerArgument { get; set; }

        public override int OnExecute()
        {
            Console.WriteLine(IntegerArgument / 2);
            return 0;
        }
    }
```

CAB uses reflection to lookup all properties with a `CommandOption` or `CommandArgument` attribute assigned to it. Validation for type correctness and null checks are done before the command's functionality is executed.

The `ExampleCommand` is easily created by inherting from the `Command` class from CAB. The Command attribute above the class will provide the parser with a name (how the command is called by the user) and a description for the help functionality. The CommandArgument attribute serves a similar purpose, the first number determines the call order of the arguments, Name and description for the help functionality.

The names will also provide handles for the 'command parsing tree'. This tree will allow you to organise the application call flow and share command attributes based on inheritence.
The `ExampleCommand` will be the start of the 'command parsing tree'. This command can be called by `[application-name] example 10`, which will print out `5`. When we create a new Command which inherits from the `ExampleCommand', we can add it to the command tree.

```csharp
[Command(Name = "nested", Description = "A nested command")]
class NestedCommand : ExampleCommand
    {
        public override int OnExecute()
        {
            Console.WriteLine(IntegerArgument * 2);
            return 0;
        }
    }
```

The `NestedCommand` can be called by `[application-name] example nested 10`, which will print out `20`.
All `CommandOption` and `CommandArgument` attributes will be inherited from the `ExampleCommand` and can be reused in the child commands.

For more information, checkout the wiki.

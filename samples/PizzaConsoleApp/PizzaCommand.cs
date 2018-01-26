using ConsoleAppBase.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace PizzaConsoleApp
{
    [Command(Name = "pizza", Description = "A command to order pizza's")]
    class PizzaCommand : MainCommand
    {
        [CommandArgument(1, Name = "type", Description = "The type of pizza you want to order", Required = true)]
        string PizzaType { get; set; }

        [CommandArgument(2, Name = "amount", Description = "the amount of pizza's")]
        [Range(0, int.MaxValue)]
        int AmountOfPizzas { get; set; }

        [CommandOption(Template = "-s|--size", Description = "The size of the pizza")]
        PizzaSize Size { get; set; }

        public override int OnExecute()
        {
            Console.WriteLine($"You've ordered {AmountOfPizzas} {Size} {PizzaType} pizzas");
            return 0;
        }

        private enum PizzaSize
        {
            Small, Medium, Big
        }
    }
}

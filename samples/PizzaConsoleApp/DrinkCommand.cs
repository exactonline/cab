using ConsoleAppBase.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaConsoleApp
{
    [Command(Name = "drink", Description = "A command to order drinks")]
    class DrinkCommand : MainCommand
    {
        [CommandArgument(1, Description = "The drinks you want to order")]
        IEnumerable<Drink> Drinks { get; set; }

        public override int OnExecute()
        {
            Console.WriteLine("You've ordered:");

            var drinks = Drinks.Select(d => d.Size + "ml " + d.Type + "\n");
            Console.WriteLine(string.Concat(drinks).TrimEnd());

            return 0;
        }

        private class Drink
        {
            public string Type { get; set; }
            public int Size { get; set; }
        }
    }
}

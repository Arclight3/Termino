using Termino;
using Termino.Models;

// Create an instance of TerminoUI
var terminoUI = new TerminoUI();

// Print a menu to the user
terminoUI.PrintMenu(TerminoMenu.Create("Choose an action", "Action 1", "Action 2", "Action 3"));

// Process the user selected option
var actionAnswer = terminoUI.ReadUserInput().Name;
switch (actionAnswer)
{
    case "Action 1":
        // TODO: Execute action 1
        break;

    case "Action 2":
        // TODO: Execute action 2
        break;
}

Console.ReadKey();

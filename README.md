# TerminoUI
A very simple but cool looking and customizable text UI for your console tools.

## Interface interaction

You only need **3** keys to interact with it: **Up**, **Down**, **Enter**.
Use the **Up** and **Down** arrow keys to navigate through options, then press **Enter** to select an option.

### Customization

If you are not satisfied with the default theme you can tweak it's options using the ```Termino.Models.Themes.TerminoTheme``` class.

Some of the customization options are:
- Colors (for title, options, currently selected option)
- Top margin
- Left margin
- Options indicator character
- Display/Hide cursor
- Loop navigation between options

## Code Examples

## TerminoUI

### Print a menu and return the selected option
```c#

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
```

### Change default theme options
```c#

// Create an instance of TerminoUI
var terminoUI = new TerminoUI();

// Declare a theme with custom options
var theme = new TerminoTheme
{
    TitleForegroundColor = ConsoleColor.DarkYellow
};

// Set the theme
terminoUI.SetTheme(theme);

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
```

## TerminoUI.Logging


### Setup and use Termino.Logging
```c#

var builder = Host.CreateApplicationBuilder(args);

// Add TerminoUI and Termino.Logging to the DI container
builder.Services.AddLogging(config =>
{
    config.AddTerminoLogging();
    config.SetMinimumLevel(builder.Configuration.GetValue<LogLevel>("MinimumLogLevel"));
});

var host = builder.Build();

// Manually get a new instace of Termino UI.
// Note: You can also inject it into your services
using var serviceScope = host.Services.CreateScope();
var terminoUI = serviceScope.ServiceProvider.GetRequiredService<TerminoUI>();

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
```

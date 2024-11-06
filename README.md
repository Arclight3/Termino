# Contents
- [Termino](#termino)
  - [Installation](#installation)
  - [Interface interaction](#interface-interaction)
  - [Customization](#customization)
  - [Code Examples](#code-examples)
    - [Print and interact with a menu](#print-and-interact-with-a-menu)
    - [Change default theme options](#change-default-theme-options)
- [Termino.Logging](#terminologging)
  - [Installation](#installation-1)
  - [Code Examples](#code-examples-1)
    - [Setup and use Termino and Termino.Logging in apps that use Host builder](#setup-and-use-termino-and-terminologging-in-apps-that-use-host-builder)
- [Donations](#donations)
  
# Termino

A very simple but cool looking, customizable and interactive text UI for your console tools.

## Installation

```dotnet add package Termino```

## Interface interaction

You only need **3** keys to interact with it: **Up**, **Down**, **Enter**.
Use the **Up** and **Down** arrow keys to navigate through options, then press **Enter** to select an option.

## Customization

If you are not satisfied with the default theme you can tweak it's options using the ```Termino.Models.Themes.TerminoTheme``` class.

Some of the customization options are:
- Colors (for title, options, currently selected option)
- Top margin
- Left margin
- Options indicator character
- Display/Hide cursor
- Loop navigation between options

## Code Examples

### Print and interact with a menu
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


# Termino.Logging

- Integrates Termino with with `Microsoft.Extensions.Logging`.
- Supports structured logging.

## Installation
```dotnet add package Termino.Logging```

## Code Examples

### Setup and use Termino and Termino.Logging in apps that use Host builder
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

// Get from DI an ILogger<T> in your services (e.g. via dependency injection)
private readonly ILogger<MyService> _logger;

public MyService(ILogger<MyService> logger)
{
    _logger = logger;
}

// Log messages in TerminoUI
_logger.LogInformation("Hello {@name}.", name);

```

# Donations

## Be careful and donate only if it is within your possibilities, because there is no refund system! Remember that you don't need to donate! You can use however you want this product totally for free. Thank you!

<a href="https://buymeacoffee.com/arclight" target="_blank"><img src="https://buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: 41px !important;width: 174px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a>


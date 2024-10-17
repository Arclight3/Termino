using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Text;
using Termino.Models.Themes;

namespace Termino.Logging.Extensions;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddTerminoLogging(this ILoggingBuilder builder, TerminoUI? terminoUI = default, TerminoTheme? theme = default, bool clearExistingProvider = true)
    {
        terminoUI ??= new();
        Console.OutputEncoding = Encoding.UTF8;

        theme ??= new()
        {
            MenuTheme = new()
            {
                TitleForegroundColor = ConsoleColor.DarkCyan,
                DisplayCursor = true
            }
        };
        terminoUI.SetTheme(theme);

        if (clearExistingProvider)
            builder.ClearProviders();

        builder.Services.AddSingleton(theme.MessagesTheme);
        builder.Services.AddSingleton(terminoUI);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TerminoLoggerProvider>());
            
        return builder;
    }
}
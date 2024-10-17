using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Termino.Models.Themes;

namespace Termino.Logging;

public class TerminoLoggerProvider : ILoggerProvider
{
    private readonly TerminoUI _terminoUI;
    private readonly TerminoMessagesTheme _messagesTheme;

    private readonly ConcurrentDictionary<string, TerminoLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

    public TerminoLoggerProvider(TerminoUI terminoUI, TerminoMessagesTheme messagesTheme)
    {
        _terminoUI = terminoUI;
        _messagesTheme = messagesTheme;
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new TerminoLogger(_terminoUI, _messagesTheme));

    public void Dispose() =>
        _loggers.Clear();
}
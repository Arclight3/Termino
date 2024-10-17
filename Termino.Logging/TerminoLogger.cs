using Microsoft.Extensions.Logging;
using Termino.Models;
using Termino.Models.Themes;

namespace Termino.Logging;

public class TerminoLogger : ILogger
{
    public LogLevel MinimumLogLevel { get; set; }

    private readonly TerminoUI _terminoUI;
    private readonly TerminoMessagesTheme _messagesTheme;

    private static readonly AsyncLocal<Stack<Dictionary<string, object>>> _scopes = new();

    public TerminoLogger(TerminoUI terminoUI, TerminoMessagesTheme messagesTheme)
    {
        _terminoUI = terminoUI ?? new();
        _messagesTheme = messagesTheme ?? new();

        MinimumLogLevel = LogLevel.Debug;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        var scope = state as IEnumerable<KeyValuePair<string, object>>;
        if (scope is not null)
        {
            if (_scopes.Value is null)
                _scopes.Value = new Stack<Dictionary<string, object>>();

            var scopeDictionary = scope.ToDictionary(item => item.Key, item => item.Value);
            _scopes.Value.Push(scopeDictionary);
        }

        return new ScopePopper();
    }

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel is not LogLevel.None && logLevel > MinimumLogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel) is false) return;
        if (logLevel < MinimumLogLevel) return;

        if (formatter is null) throw new ArgumentNullException(nameof(formatter));

        var (messageTemplate, messageArguments) = ExtractMessageTemplateAndArguments(state);

        // Combine current scope arguments with message arguments, if any
        if (_scopes.Value is { Count: > 0 })
        {
            var scopeArgs = _scopes.Value.Peek();
            foreach (var scopeArg in scopeArgs)
            {
                if (!messageArguments.ContainsKey(scopeArg.Key))
                    messageArguments.Add(scopeArg.Key, scopeArg.Value);
            }
        }

        if (!string.IsNullOrEmpty(messageTemplate) || exception is not null)
            WriteMessage(logLevel, messageTemplate, messageArguments, exception);
    }

    private (string? MessageTemplate, Dictionary<string, object?> MessageArguments) ExtractMessageTemplateAndArguments<TState>(TState state)
    {
        var originalMessageArguments = state as IReadOnlyList<KeyValuePair<string, object?>>;
        _ = originalMessageArguments ?? throw new ArgumentNullException(nameof(originalMessageArguments));

        var messageTemplate = originalMessageArguments.First(x => x.Key == "{OriginalFormat}").Value?.ToString();
        var messageArguments = new Dictionary<string, object?>();

        foreach (var logArgument in originalMessageArguments)
        {
            if (logArgument.Key is "{OriginalFormat}") continue;

            messageArguments.Add(logArgument.Key, logArgument.Value);
        }

        return (messageTemplate, messageArguments);
    }

    private void WriteMessage(LogLevel logLevel, string? messageTemplate, Dictionary<string, object?> messageArguments, Exception? exception)
    {
        if (messageArguments.Count is 0)
            WriteBasicMessage(logLevel, messageTemplate, exception);
        else
            WriteAdvancedMessage(logLevel, messageTemplate, messageArguments, exception);
    }

    private void WriteBasicMessage(LogLevel logLevel, string? message, Exception? exception)
    {
        if (exception is not null)
            message = $"{message}{exception}";

        switch (logLevel)
        {
            case LogLevel.Trace:
                _terminoUI.PrintLine(message, _messagesTheme.TraceMessageColor);
                break;
            case LogLevel.Debug:
                _terminoUI.PrintLine(message, _messagesTheme.DebugMessageColor);
                break;
            case LogLevel.Information:
                _terminoUI.PrintLine(message, _messagesTheme.InformationalMessageColor);
                break;
            case LogLevel.Warning:
                _terminoUI.PrintLine(message, _messagesTheme.WarningMessageColor);
                break;
            case LogLevel.Error:
                _terminoUI.PrintLine(message, _messagesTheme.ErrorMessageColor);
                break;
            case LogLevel.Critical:
                _terminoUI.PrintLine(message, _messagesTheme.CriticalMessageColor);
                break;
            case LogLevel.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
        }
    }

    private void WriteAdvancedMessage(LogLevel logLevel, string? messageTemplate, Dictionary<string, object?> messageArguments, Exception? exception)
    {
        var messageTemplateParts = ExtractMessageTemplateParts(messageTemplate);
        var (textColor, tokenTextColor) = GetTextColor(logLevel);

        var textTokens = new List<TerminoTextToken>();

        for (var charIndex = 0; charIndex < messageTemplateParts.Count; charIndex++)
        {
            var messageTemplatePart = messageTemplateParts[charIndex];

            if (messageTemplatePart.StartsWith('{'))
            {
                messageTemplatePart = messageTemplatePart[1..^1];

                messageArguments.TryGetValue(messageTemplatePart, out var messageTemplatePartValue);
                messageTemplatePart = messageTemplatePartValue?.ToString();

                textTokens.Add(new TerminoTextToken(messageTemplatePart, tokenTextColor));
            }
            else
            {
                textTokens.Add(new TerminoTextToken(messageTemplatePart, textColor));
            }
        }

        _terminoUI.PrintTokensLine(textTokens.ToArray());
    }

    private static List<string> ExtractMessageTemplateParts(string? messageTemplate)
    {
        var messageTemplateParts = new List<string>();

        if (messageTemplate is not null)
        {
            var i = 0;
            var j = 0;
            while (j < messageTemplate.Length)
            {
                if (messageTemplate[j] == '{')
                {
                    if (j > i)
                    {
                        messageTemplateParts.Add(messageTemplate[i..j]);
                        i = j;
                    }

                    j = messageTemplate.IndexOf('}', j) + 1;

                    messageTemplateParts.Add(messageTemplate[i..j]);

                    i = j;
                }
                else
                {
                    j++;
                }

                if (j == messageTemplate.Length && messageTemplate[j - 1] != '}')
                    messageTemplateParts.Add(messageTemplate[i..j]);
            }
        }

        return messageTemplateParts;
    }

    private (ConsoleColor TextColor, ConsoleColor TokenTextColor) GetTextColor(LogLevel logLevel)
    {
        var textColor = logLevel switch
        {
            LogLevel.Trace => _messagesTheme.TraceMessageColor,
            LogLevel.Debug => _messagesTheme.DebugMessageColor,
            LogLevel.Information => _messagesTheme.InformationalMessageColor,
            LogLevel.Warning => _messagesTheme.WarningMessageColor,
            LogLevel.Error => _messagesTheme.ErrorMessageColor,
            LogLevel.Critical => _messagesTheme.CriticalMessageColor,
            LogLevel.None => default,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };

        var tokenTextColor = logLevel switch
        {
            LogLevel.Trace => _messagesTheme.TraceMessageTokenColor,
            LogLevel.Debug => _messagesTheme.DebugMessageTokenColor,
            LogLevel.Information => _messagesTheme.InformationalMessageTokenColor,
            LogLevel.Warning => _messagesTheme.WarningMessageTokenColor,
            LogLevel.Error => _messagesTheme.ErrorMessageTokenColor,
            LogLevel.Critical => _messagesTheme.CriticalMessageTokenColor,
            LogLevel.None => default,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };

        return (textColor, tokenTextColor);
    }

    private class ScopePopper : IDisposable
    {
        public void Dispose() => _scopes.Value?.Pop();
    }
}
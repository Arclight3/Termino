namespace Termino.Models.Themes;

public class TerminoMessagesTheme
{
    public ConsoleColor TraceMessageColor { get; set; } = ConsoleColor.DarkGray;
    public ConsoleColor TraceMessageTokenColor { get; set; } = ConsoleColor.Gray;

    public ConsoleColor DebugMessageColor { get; set; } = ConsoleColor.DarkGray;
    public ConsoleColor DebugMessageTokenColor { get; set; } = ConsoleColor.Gray;

    public ConsoleColor InformationalMessageColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor InformationalMessageTokenColor { get; set; } = ConsoleColor.DarkGreen;

    public ConsoleColor WarningMessageColor { get; set; } = ConsoleColor.Yellow;
    public ConsoleColor WarningMessageTokenColor { get; set; } = ConsoleColor.DarkYellow;

    public ConsoleColor ErrorMessageColor { get; set; } = ConsoleColor.DarkRed;
    public ConsoleColor ErrorMessageTokenColor { get; set; } = ConsoleColor.Red;

    public ConsoleColor CriticalMessageColor { get; set; } = ConsoleColor.Red;
    public ConsoleColor CriticalMessageTokenColor { get; set; } = ConsoleColor.DarkRed;
}
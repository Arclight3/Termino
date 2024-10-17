namespace Termino.Models;

public record TerminoTextToken(string? Text)
{
    public TerminoTextToken(string? text, ConsoleColor? foregroundColor)
        : this(text) =>
        ForegroundColor = foregroundColor;

    public TerminoTextToken(string? text, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        : this(text) =>
        (ForegroundColor, BackgroundColor) = (foregroundColor, backgroundColor);

    public ConsoleColor? ForegroundColor { get; set; }
    public ConsoleColor? BackgroundColor { get; set; }
}
namespace Termino.Models;

public record TerminoMenu
{
    public string Title { get; set; } = default!;
    public List<TerminoMenuOption> Options { get; set; } = new();

    internal int TopStartPosition { get; set; }
    internal int LeftStartPosition { get; set; }
    internal int TopEndPosition { get; set; }
    internal int LeftEndPosition { get; set; }

    public static TerminoMenu Create(string title, List<TerminoMenuOption> options) =>
        new()
        {
            Title = title,
            Options = options
        };

    public static TerminoMenu Create(string title, params TerminoMenuOption[] options) =>
        new()
        {
            Title = title,
            Options = options.ToList()
        };

    public static TerminoMenu Create(string title, params string[] options) =>
        new()
        {
            Title = title,
            Options = options.Select(TerminoMenuOption.Create).ToList()
        };
}
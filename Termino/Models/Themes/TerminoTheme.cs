namespace Termino.Models.Themes;

public class TerminoTheme
{
    // General
    public int LeftMarginColumns { get; set; } = 2;
    public int TopMarginLines { get; set; } = 0;

    // Menu
    public TerminoMenuTheme MenuTheme { get; set; } = new();
    public TerminoMessagesTheme MessagesTheme { get; set; } = new();
}
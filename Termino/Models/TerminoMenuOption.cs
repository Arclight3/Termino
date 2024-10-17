namespace Termino.Models;

public record TerminoMenuOption(string Name)
{
    public bool IsActive { get; set; }

    public static TerminoMenuOption Create(string name) => new(name);
}
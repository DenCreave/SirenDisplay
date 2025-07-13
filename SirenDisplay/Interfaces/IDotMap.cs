namespace SirenDisplay.Interfaces;

public interface IDotMap
{
    string Name { get; }
    bool IsStatic { get; }
    void IncreaseDots();
    void DecreaseDots();
}
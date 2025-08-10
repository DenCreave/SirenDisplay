namespace SirenDisplay.Interfaces;

public interface IDotMap
{
    string Name { get; }
    int ID { get; }
    bool IsStatic { get; }
    void IncreaseDots();
    void DecreaseDots();
}
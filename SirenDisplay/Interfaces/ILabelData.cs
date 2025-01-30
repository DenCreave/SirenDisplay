using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface ILabelData
{
    public string OffLabel {get;}
    public string[] AnimatingLabel {get;}
    public string PendingLabel {get;}
    public string SirenLabel {get;}
}
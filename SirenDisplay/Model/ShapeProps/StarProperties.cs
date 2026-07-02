using SirenDisplay.Interfaces;

namespace SirenDisplay.Model.ShapeProps;

public class StarProperties : IShapeProperties
{
    public Vertex Nil { get; set; }
    public Vertex End { get; set; }
}
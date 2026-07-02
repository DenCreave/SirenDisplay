namespace SirenDisplay.Model;

public class Constellation
{
    public Vertex[] Vertices { get; set; }
    public VexEdge[] Edges { get; set; }
    
    public int LayerLevel { get; set; }
}
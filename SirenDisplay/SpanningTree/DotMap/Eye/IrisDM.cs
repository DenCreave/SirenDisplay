using System;
using System.Collections.Generic;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.DMProps;
using SirenDisplay.SpanningTree.Shapes;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.SpanningTree.DotMap.Eye;

public sealed class IrisDM : IDotMapFactory<IrisProperties>
{
    public ThemeGroup Group { get; } = ThemeGroup.Eye;
    public DMName Name { get; } = DMName.Iris;
    public int LayerLevel { get; } = 1;
    public IrisProperties UniqueProps { get; }
    public AnimatrixController Controls { get; }

    public IrisDM(AnimatrixController controller)
    {
        Controls = controller;
        UniqueProps = new IrisProperties()
        {
            ShapeMultiplier = 32
        };
    }
    
    public Constellation GenerateVertices()
    {
        List<Vertex> vertices = [];
        List<VexEdge> adjacency = [];
        for (int i = 0; i < UniqueProps.ShapeMultiplier; i++)
        {
            double angleInRadians = (Math.PI * 2 / UniqueProps.ShapeMultiplier) * i;

            var irisShape = new IrisBase();
            for (int j = 0; j < irisShape.Shapes.Length; j++)
            {
                irisShape.Shapes[j].Cox *= UniqueProps.Scale;

                irisShape.Shapes[j].Coy *= UniqueProps.Scale;
                irisShape.Shapes[j].Coy += UniqueProps.IrisRadiusOffset;

                irisShape.Shapes[j].ID = i * irisShape.Shapes.Length + j;
                
                Controls.Rotate(irisShape.Shapes[j], angleInRadians);
                vertices.Add(irisShape.Shapes[j]);
            }
        }
        
        int shapecount = new IrisBase().Shapes.Length;
        for (int i = 0; i < vertices.Count; i++)
        {
            adjacency.Add(new VexEdge()
            {
                A = vertices[i],
                B = vertices[(i + shapecount) % vertices.Count],
                RelationType = EdgeRelType.Line,
                Group = Insignia.Emerald
            });
            if (i % shapecount == 0)
            {
                adjacency.Add(new VexEdge()
                {
                    A = vertices[i],
                    B = vertices[(i + shapecount) % vertices.Count],
                    RelationType = EdgeRelType.Arc,
                    Group = Insignia.Amethyst
                });
            }
            if (i % (shapecount*2) == 0)
            {
                adjacency.Add(new VexEdge()
                {
                    A = vertices[i],
                    B = vertices[(i + ((shapecount*2) -1)) % vertices.Count],
                    RelationType = EdgeRelType.Line,
                    Group = Insignia.Amethyst
                });
            }
            else if (i % (shapecount*2) == (shapecount*2)-1 )
            {
                adjacency.Add(new VexEdge()
                {
                    A = vertices[i],
                    B = vertices[(i + 1) % vertices.Count],
                    RelationType = EdgeRelType.Line,
                    Group = Insignia.Amethyst
                });
            }
        }

        return new Constellation()
        {
            Vertices = vertices.ToArray(),
            Edges = adjacency.ToArray(),
            LayerLevel = this.LayerLevel,
        };
    }
}
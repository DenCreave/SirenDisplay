using System;
using System.Collections.Generic;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.Shapes;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.DMProps;

namespace SirenDisplay.Assets.SpanningTree.DotMap.Eye;

public sealed class RingOfIrisDM : IDotMapFactory<IrisProperties>
{
    public ThemeGroup Group { get; } = ThemeGroup.Eye;
    public DMName Name { get; } = DMName.RingOfIris;
    public int LayerLevel { get; } = 2;
    public IrisProperties UniqueProps { get; }
    public AnimatrixController Controls { get; }
    public RingOfIrisDM(AnimatrixController controller)
    {
        Controls = controller;
        UniqueProps = new IrisProperties(); // the defaults are great (16 shape*)
    }

    public Constellation GenerateVertices()
    {
        List<Vertex> vertices = [];
        List<VexEdge> adjacency = [];
        for (int i = 0; i < UniqueProps.ShapeMultiplier; i++)
        {
            double angleInRadians = (Math.PI * 2 / UniqueProps.ShapeMultiplier) * i;
            var iris = new Star();
            for (int j = 0; j < iris.Shapes.Length; j++)
            {
                iris.Shapes[j].Cox *= UniqueProps.Scale;

                iris.Shapes[j].Coy *= UniqueProps.Scale;
                iris.Shapes[j].Coy += UniqueProps.IrisRadiusOffset;
                
                iris.Shapes[j].ID = i * iris.Shapes.Length + j;
                
                Controls.Rotate(iris.Shapes[j], angleInRadians);
                vertices.Add(iris.Shapes[j]);
            }
        }

        int shapecount = new Star().Shapes.Length;
        for (int i = 0; i < vertices.Count; i++)
        {
            if (i % shapecount == 0)
            {
                adjacency.Add(new VexEdge()
                {
                    A = vertices[i],
                    B = vertices[(i - (shapecount/2) < 0) 
                        ? vertices.Count - (shapecount/2)
                        : i - (shapecount/2)
                    ],
                    RelationType = EdgeRelType.Arc
                });
            }
            adjacency.Add(new VexEdge()
            {
                A = vertices[i],
                B = vertices[(i + 1)% vertices.Count  ],
                RelationType = EdgeRelType.Line
            });
        }

        return new Constellation()
        {
            Vertices = vertices.ToArray(),
            Edges = adjacency.ToArray()
        };
    }
}
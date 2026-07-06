using System;
using System.Collections.Generic;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.DMProps;
using SirenDisplay.SpanningTree.Shapes;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.SpanningTree.DotMap.Eye;

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
        List<VexEdge> nilEndCache = [];
        for (int i = 0; i < UniqueProps.ShapeMultiplier; i++)
        {
            // in UI programming (screen) Y axis is flipped, and sine cosine will rotate COUNTER-clockwise
            //instead of clockwise. to make it clockwise, ill give it negative radians.
            // //ai called this the Threaded-bead effect (good to know such concept exists)
            double angleInRadians = -(Math.PI * 2 / UniqueProps.ShapeMultiplier) * i;
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
            
            nilEndCache.Add(new VexEdge()
            {
                A = iris.UniqueProps.Nil,
                B = iris.UniqueProps.End,
            });
        }

        //new way for connecting shapes
        for (int i = 0; i < nilEndCache.Count; i++)
        {
            adjacency.Add(new VexEdge()
            {
                /*//was a beautiful mistake, now im making it intentional
                A = nilEndCache[i].B,
                B = nilEndCache[(i+1)%nilEndCache.Count].A,*/
                //this way im connecting 2 shapes, intentionnaly, and will go through them
                //ai called this the Threaded-bead effect (good to know such concept exists)
                A = nilEndCache[i].A,
                B = nilEndCache[(i+1)%nilEndCache.Count].B, 
                RelationType = EdgeRelType.Arc,
                Group = Insignia.Sapphire
            });
        }
        
        int shapecount = new Star().Shapes.Length;
        for (int i = 0; i < vertices.Count; i++)
        {
            /* old way of calculating connecting shapes.
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
            }*/
            adjacency.Add(new VexEdge()
            {
                A = vertices[i],
                B = vertices[((i + 1) % shapecount)+(i/shapecount)*shapecount],
                RelationType = (i % 3 == 1) ? EdgeRelType.Arc : EdgeRelType.Line,
                Group = Insignia.Ruby
            });
        }

        

        return new Constellation()
        {
            Vertices = vertices.ToArray(),
            Edges = adjacency.ToArray(),
            LayerLevel = LayerLevel
        };
    }
}
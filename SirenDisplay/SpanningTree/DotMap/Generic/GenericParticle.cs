using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.DMProps;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.SpanningTree.DotMap.Generic;

public class GenericParticle : IDotMapFactory<ParticleProps>
{
    public ThemeGroup Group { get; } = ThemeGroup.Generic;
    public DMName Name { get; } = DMName.Particle;
    public int LayerLevel { get; } = 0;

    public ParticleProps UniqueProps { get; }

    public GenericParticle()
    {
        UniqueProps = new ParticleProps()
        {
            DotLimit = 500,
        };
    }

    public Constellation GenerateVertices()
    {
        Vertex[] retme = new Vertex[UniqueProps.DotLimit];
        for (int i = 0; i < UniqueProps.DotLimit; i++)
        {
            retme[i] = new Vertex()
            {
                ID = i,
            };
        }

        return new Constellation()
        {
            Vertices = retme,
            Edges = null,
            LayerLevel = LayerLevel,
        };
    }
}
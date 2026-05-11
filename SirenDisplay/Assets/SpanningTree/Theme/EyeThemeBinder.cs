using System.Collections.Generic;
using System.Linq;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.Theme;

public class EyeThemeBinder : IThemeBinder
{
    private readonly DotMapLoader _mapLoader;
    private readonly TorrentLayerLoader _layerLoader;
    public ThemeGroup ThemeGroup { get; }

    public EyeThemeBinder(DotMapLoader mapLoader, TorrentLayerLoader layerLoader)
    {
        _mapLoader = mapLoader;
        _layerLoader = layerLoader;
        ThemeGroup = ThemeGroup.Eye;
    }

    public Animap[] ElicitateAnimap()
    {
        List<Animap> tmp = [];
        tmp.Add(new Animap()
        {
            TorrentLayer = _layerLoader.Layers.Where(x => x.Group == ThemeGroup.Eye 
                                                                    && x.Name == TLName.Top)
                .First(),
            Graph = _mapLoader.Maps.Where(x=> x.Group == ThemeGroup.Generic 
                                              && x.Name == DMName.Particle)
                .First()
                .GenerateVertices()
        });
        
        tmp.Add(new Animap()
        {
            TorrentLayer = _layerLoader.Layers.Where(x => x.Group == ThemeGroup.Eye 
                                                          && x.Name == TLName.Bottom)
                .First(),
            Graph = _mapLoader.Maps.Where(x=> x.Group == ThemeGroup.Generic 
                                              && x.Name == DMName.Particle)
                .First()
                .GenerateVertices()
        });
        
        tmp.Add(new Animap()
        {
            TorrentLayer = _layerLoader.Layers.Where(x => x.Group == ThemeGroup.Eye 
                                                          && x.Name == TLName.Mid)
                .First(),
            Graph = _mapLoader.Maps.Where(x=> x.Group == ThemeGroup.Eye 
                                              && x.Name == DMName.Iris)
                .First()
                .GenerateVertices()
        });
        
        tmp.Add(new Animap()
        {
            TorrentLayer = _layerLoader.Layers.Where(x => x.Group == ThemeGroup.Eye 
                                                          && x.Name == TLName.Mid)
                .First(),
            Graph = _mapLoader.Maps.Where(x=> x.Group == ThemeGroup.Eye 
                                              && x.Name == DMName.RingOfIris)
                .First()
                .GenerateVertices()
        });
        return tmp.ToArray();
    }
}
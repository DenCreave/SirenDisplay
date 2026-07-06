using System.Collections.Generic;
using System.Linq;
using SirenDisplay.Interfaces;
using SirenDisplay.SpanningTree.DotMap;
using SirenDisplay.SpanningTree.TorrentLayer;

namespace SirenDisplay.SpanningTree.Theme;

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
            TorrentLayer = _layerLoader.CreateNewLayer(ThemeGroup.Eye, TLName.Top),
            Graph = _mapLoader.Maps.Where(x=> x.Group == ThemeGroup.Generic 
                                              && x.Name == DMName.Particle)
                .First()
                .GenerateVertices()
        });
        
        tmp.Add(new Animap()
        {
            TorrentLayer = _layerLoader.CreateNewLayer(ThemeGroup.Eye, TLName.Bottom),
            Graph = _mapLoader.Maps.Where(x=> x.Group == ThemeGroup.Generic 
                                              && x.Name == DMName.Particle)
                .First()
                .GenerateVertices()
        });
        
        var mid_layer= _layerLoader.CreateNewLayer(ThemeGroup.Eye, TLName.Mid);
        
        
        tmp.Add(new Animap()
        {
            TorrentLayer = mid_layer,
            Graph = _mapLoader.Maps.Where(x=> x.Group == ThemeGroup.Eye 
                                              && x.Name == DMName.Iris)
                .First()
                .GenerateVertices()
        });
        
        tmp.Add(new Animap()
        {
            TorrentLayer = mid_layer,
            Graph = _mapLoader.Maps.Where(x=> x.Group == ThemeGroup.Eye 
                                              && x.Name == DMName.RingOfIris)
                .First()
                .GenerateVertices()
        });
        return tmp.ToArray();
    }
}
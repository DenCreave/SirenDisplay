using System.Collections.Generic;
using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface ITorrentLayer
{
    TLGroup Group { get; }
    string Name { get; }
    List<Vertex> TorrentPath { get; } 
    //todo, maybe add a layer order too
}
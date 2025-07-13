using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface ITorrentLayer
{
    string Name { get; }
    ObservableCollection<Vertex> Vertices { get; set; }
    void IncreaseVertices(){}
    void DecreaseVertices(){}
}
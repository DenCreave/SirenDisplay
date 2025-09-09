using System;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface IDotMap
{
    DMGroup Group { get; }
    TLGroup? TorrentGroup { get; } //null means it belongs to no torrentlayer 
    TLName? TorrentLayerType { get; } //value is only null if it doesnt belong to a layer 
    DMName Name { get; } 
    int LayerLevel { get; } //to set an order
    int? DotLimit { get; } //if its null it shouldnt generate dots but use predefined ones
    /// <summary>
    /// bool? GenerateWithRandomVector
    /// null: no vector given to randomly generated vertices
    /// true: randomly generate value value for random point
    /// false: use predefined vector value defined in MeshHeapController
    /// </summary>
    bool? GenerateWithRandomVector { get; }
    bool IsStatic { get; } //shown automatically, doesnt use TL spawn area
    TimeSpan? ActivationDelay { get; } //null: no delay, otherwise the dotmap starts animation later
    bool ManuallyDefinedEdges { get; } //instead of algorythm, predefine edges in MeshHeap
    double? AlgorithmThreshold { get; } //whats the enabled/disabled ratio in which the algorithm should run?
    Vertex[]? Vertices { get; } //if it has any predefined vertices to use
    //todo: add colors or maybe just add a color manager itself, idk
}
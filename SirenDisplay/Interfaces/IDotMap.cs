using System;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface IDotMap
{
    DMGroup Group { get; }
    TLGroup? TorrentGroup { get; } //null means it belongs to no torrentlayer 
    TLName? TorrentLayerType { get; } //value is only null if it doesnt belong to a layer 
    STCGroup? ControllerGroup { get; } //if its null it means it requires no custom controller
    STCName? ControllerName { get; } //if its null it means it requires no custom controller
    DMName Name { get; } 
    int LayerLevel { get; } //to set an order

    #region generation
    bool ManuallyGenerateDots { get; }
    int? DotLimit { get; } //if its null it shouldnt generate dots
    bool GenerateWithRandomVector { get; }
    double? InitVectorMin { get; }
    double? InitVectorMax { get; }
    #endregion generation
    TimeSpan? ActivationDelay { get; } //null: no delay, otherwise the dotmap starts animation later
    bool IsStatic { get; } //shown automatically, doesnt use TL spawn area
    bool ManuallyDefinedEdges { get; } //instead of algorythm, predefine edges in STCControllers
    /// <summary>
    /// whats the enabled/disabled ratio in which the algorithm should run?
    /// null: doesnt run the algorythm, manually defined (see bool ManuallyDefinedEdges)
    /// </summary>
    double? AlgorithmThreshold { get; } 
    bool IsUsePredefinedVectices { get; }
    Vertex[]? Vertices { get; set; } //if it has any predefined vertices to use
    //todo: add colors or maybe just add a color manager itself, idk
}
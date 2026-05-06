using System;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.DotMap.Eye;

public sealed class EyeBottomDM : IDotMap
{
    public DMGroup Group => DMGroup.Eye;
    public TLGroup? TorrentGroup => TLGroup.Eye;
    public TLName? TorrentLayerType => TLName.Bottom;
    public STCGroup? ControllerGroup => null;
    public STCName? ControllerName => null;
    public DMName Name => DMName.EyeBottom;
    public int LayerLevel => 0;
    public bool ManuallyGenerateDots =>false;
    public int? DotLimit => 50;
    public bool GenerateWithRandomVector => true;
    public double? InitVectorMin => 10;
    public double? InitVectorMax => 50;
    public bool IsStatic => false;
    public TimeSpan? ActivationDelay => null;
    public bool ManuallyDefinedEdges =>false;
    public double? AlgorithmThreshold => 0.6;
    public Vertex[]? Vertices { get; set; } = null;
    public bool IsUsePredefinedVectices => false;
}
using System;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.DotMap.Eye;

public sealed class RingOfIrisDM : IDotMap
{
    public DMGroup Group => DMGroup.Eye;
    public TLGroup? TorrentGroup => TLGroup.Eye;
    public TLName? TorrentLayerType => TLName.Mid;
    public STCGroup? ControllerGroup => STCGroup.Eye;
    public STCName? ControllerName => STCName.RingOfIris;
    public DMName Name => DMName.RingOfIris;
    public int LayerLevel => 2;
    public bool ManuallyGenerateDots => false;
    public int? DotLimit => null;
    public bool GenerateWithRandomVector => false;
    public double? InitVectorMin => null;
    public double? InitVectorMax => null;
    public bool IsStatic => true;
    public TimeSpan? ActivationDelay => TimeSpan.FromMilliseconds(3000);
    public bool ManuallyDefinedEdges => true;
    public double? AlgorithmThreshold => null;
    public Vertex[]? Vertices { get; }
}
using System;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.DotMap.Eye;

public sealed class EyeBottomDM : IDotMap
{
    public DMGroup Group => DMGroup.Eye;
    public TLGroup? TorrentGroup => TLGroup.Eye;
    public TLName? TorrentLayerType => TLName.Bottom;
    public DMName Name => DMName.EyeBottom;
    public int LayerLevel => 0;
    public int? DotLimit => 50;
    public bool? GenerateWithRandomVector => true;
    public bool IsStatic => false;
    public TimeSpan? ActivationDelay => null;
    public bool ManuallyDefinedEdges =>false;
    public double? AlgorithmThreshold => 0.6;
    public Vertex[]? Vertices => null;
}
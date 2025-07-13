using System.Net.Http.Headers;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SirenDisplay.Model;

public sealed partial class Vertex : ObservableObject
{ 
    [ObservableProperty]
    private int _x;
    [ObservableProperty]
    private int _y;
    private DispatcherTimer _timer { get; set; }

    /// <summary>
    /// set the direction and speed for each tick
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vertex SetVector(int x, int y)
    {
        
        return this;
    }

    public Vertex StartVertex()
    {
        return this;
    }

    public Vertex StopVertex()
    {
        return this;
    }
    
    

}
using System;
using Avalonia.Media.Imaging;
using Avalonia.Svg.Skia;

namespace SirenDisplay.Controllers;

public sealed class AlarmButtonController
{
    public Bitmap DefaultImage { get; }
    public Bitmap ActivatedImage { get; }
    public Bitmap CurrentImage { get; set; }
    
    public bool IsActive { get; set; }

    public AlarmButtonController()
    {
        /*
        string path = "/Assets/Images/clockbuttonclock.svg"; 
        
        DefaultImage = new Bitmap(path);
        
        DefaultImage = new SvgImage 
        {
            Source = new SvgSource( new Uri(path)) 
        };
        path = "/Assets/Images/alarmbuttonver1.svg";
        ActivatedImage = new SvgImage()
        {
            Source = new SvgSource(new Uri(path))
        };
        
        
        CurrentImage = DefaultImage;
        IsActive = false;*/
    }

    public void SwitchImage()
    { 
        IsActive=!IsActive;
        CurrentImage = IsActive ? ActivatedImage : DefaultImage;
    }
}
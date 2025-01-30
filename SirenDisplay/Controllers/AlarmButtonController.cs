using System;
using Avalonia.Media.Imaging;
using Avalonia.Svg.Skia;

namespace SirenDisplay.Controllers;

public sealed class AlarmButtonController
{
    public bool IsActive { get; set; }

    public AlarmButtonController()
    {

    }

    public void SwitchImage()
    { 
        IsActive=!IsActive;
       // CurrentImage = IsActive ? ActivatedImage : DefaultImage;
    }
}
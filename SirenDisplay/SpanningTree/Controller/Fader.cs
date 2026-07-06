using System;

namespace SirenDisplay.SpanningTree.Controller;

public sealed class Fader
{
    public double FadeInDelay { get; init; }
    public double FadeInDuration { get; init; }
    public double Lifetime { get; init; }
    public double FadeOutDelay { get; init; }
    public double FadeOutDuration { get; init; }
    
    public double TotalElapsedSeconds { get; set; } // will be updated in ITorrentLayer.UpdateState
    public double LayerOpacity { get; set; } // will be calculated in ITorrentLayer.UpdateState
    
    public bool IsAlive => (TotalElapsedSeconds < (FadeInDelay + FadeInDuration + Lifetime + FadeOutDelay + FadeOutDuration));

    
    public void UpdateTime(double deltaTime)
    {
        TotalElapsedSeconds += deltaTime;
        
        double fadeOutStart = FadeOutDelay + Lifetime + FadeInDuration + FadeInDelay; 
        double fadeOutEnd = FadeOutDuration + FadeOutDelay + Lifetime + FadeInDuration + FadeInDelay;

        // calculate Fade In (0.0 to 1.0)
        // ternary to prevent division by 0
        double fadeInVal = FadeInDuration > 0 
            ? Math.Clamp((TotalElapsedSeconds - FadeInDelay) / FadeInDuration, 0.0, 1.0) 
            : (TotalElapsedSeconds >= FadeInDelay) ? 1.0 : 0.0;

        double fadeOutVal = 1;
        if (TotalElapsedSeconds > fadeOutStart)
        {
            // calculate Fade Out (1.0 to 0.0)
            fadeOutVal = FadeOutDuration > 0 
                ? Math.Clamp(1.0 - ((TotalElapsedSeconds - fadeOutStart) / FadeOutDuration), 0.0, 1.0) 
                : (TotalElapsedSeconds > fadeOutEnd) ? 0.0 : 1.0;
        }
        
        LayerOpacity = Math.Min(fadeInVal, fadeOutVal);

    }

    public void Reset()
    {
        double fadeOutStart = FadeOutDelay + Lifetime + FadeInDuration + FadeInDelay;
        double fadeOutEnd = FadeOutDuration + FadeOutDelay + Lifetime + FadeInDuration + FadeInDelay;
        
        double fadeInVal = FadeInDuration > 0 
            ? Math.Clamp((TotalElapsedSeconds - FadeInDelay) / FadeInDuration, 0.0, 1.0) 
            : (TotalElapsedSeconds >= FadeInDelay) ? 1.0 : 0.0;

        double fadeOutVal = 1;
        if (TotalElapsedSeconds > fadeOutStart)
        {
            fadeOutVal = FadeOutDuration > 0 
                ? Math.Clamp(1.0 - ((TotalElapsedSeconds - fadeOutStart) / FadeOutDuration), 0.0, 1.0) 
                : (TotalElapsedSeconds > fadeOutEnd) ? 0.0 : 1.0;
        }
        

        if (fadeInVal >= 1.0 && TotalElapsedSeconds < fadeOutStart )
        {
            TotalElapsedSeconds = FadeInDelay + FadeInDuration;
        }else if (TotalElapsedSeconds >= fadeOutStart  && fadeOutVal < 1.0)
        {
            TotalElapsedSeconds = FadeInDelay + (FadeInDuration * fadeOutVal);
        }
    }
    
    
}
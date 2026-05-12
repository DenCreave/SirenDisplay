using System.Threading;

namespace SirenDisplay.Assets.SpanningTree.Controller;

public class Spawner
{
    public double SpawnInterval { get; set; }  // in seconds 
    
    public double LifeTime { get; set; } // how long to call forth vertices
    public double TotalElapsedSeconds { get; set; }
    
    public double _timeSinceLastSpawn { get; set; }
    private int _pendingSpawns;

    public void Reset()
    {
        TotalElapsedSeconds = 0;
        /*
        _pendingSpawns = 0; // Don't forget to clear the queue!
        _timeSinceLastSpawn = 0; what if i do?*/

    }

    public void UpdateTime(double dt)
    {
        _timeSinceLastSpawn += dt;
        TotalElapsedSeconds += dt;
        if (TotalElapsedSeconds<LifeTime)
        {
            if (_timeSinceLastSpawn >= SpawnInterval)
            {
                int spawnsToAdd = (int)(_timeSinceLastSpawn / SpawnInterval);


                Interlocked.Add(ref _pendingSpawns, spawnsToAdd);
                //_pendingSpawns+= (int)(_timeSinceLastSpawn/SpawnInterval);
                _timeSinceLastSpawn %= SpawnInterval;
            }
        }
    }
    
    public bool TryConsumeSpawn()
    {
        // Fast check: if it's 0, don't bother
        if (_pendingSpawns <= 0 || TotalElapsedSeconds > LifeTime) return false;

        // Thread-safe decrement. 
        // If the result is >= 0, this specific thread successfully claimed the spawn!
        int remaining = Interlocked.Decrement(ref _pendingSpawns);
        
        if (remaining >= 0) 
        {
            return true; 
        }

        // If it went negative, another thread stole the last one a microsecond before us.
        // Put the counter back to 0 and return false.
        Interlocked.Increment(ref _pendingSpawns);
        return false;
    }


}
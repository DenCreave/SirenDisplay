using System;
using SirenDisplay.Model;
using SirenDisplay.Model.TLProps;

namespace SirenDisplay.Controllers;

/// <summary>
/// the deduplication class
/// collect all the functions for animations here, should they be used by STC, Mesh or anything
/// </summary>
public sealed class AnimatrixController
{
    public void Rotate(Vertex shape, double angleInRadians)
    {
        double tempex = shape.Cox;
        double tempey = shape.Coy;

        // Calculate the sine and cosine of the angle
        double cosTheta = Math.Cos(angleInRadians);
        double sinTheta = Math.Sin(angleInRadians);

        // Apply the rotation formulas and round them too
        shape.Cox = tempex * cosTheta - tempey * sinTheta;
        shape.Coy = tempex * sinTheta + tempey * cosTheta;
    }

    public double[] HaltonSequencer1D(int size, int bass = 2)
    {
        double[] result = new double[size];
        for (int i = 0; i < size; i++)
        {
            result[i] = HaltonSequence1D(i, bass);
        }

        return result;
    }

    public double HaltonSequence1D(int index, int bass = 2)
    {
        double result = 0;
        double fractiMulti = 1;
        while (index > 0)
        {
            fractiMulti /= bass;
            result += fractiMulti * (index % bass);
            index /= bass;
        }

        return result;
    }

    public Vertex Vortexer(VortexProperties uniqueProps, Vertex vertex)
    {
        // --- SAFETY CHECK: Prevent silent crashes! ---
        if (vertex.TargetPathIndex >= uniqueProps.TorrentPath.Length)
        {
            return vertex; // Do nothing, let the TorrentLayer despawn it
        }

        // Get our current segment points A and B
        int prevIndex = vertex.TargetPathIndex - 1;
        if (prevIndex < 0) prevIndex = 0;

        var pointA = uniqueProps.TorrentPath[prevIndex];
        var pointB = uniqueProps.TorrentPath[vertex.TargetPathIndex];

        // ==========================================
        // STEP 1: THE DIRECTION OF THE WALL (A to B)
        // ==========================================
        double wallX = pointB.X - pointA.X;
        double wallY = pointB.Y - pointA.Y;

        // Pythagorean theorem to find the total length of the wall
        double wallLength = Math.Sqrt((wallX * wallX) + (wallY * wallY));

        // Normalize it to get our "compass needle" (length of 1)
        double dirX = wallLength > 0 ? wallX / wallLength : 0;
        double dirY = wallLength > 0 ? wallY / wallLength : 0;

        // ==========================================
        // STEP 2: THE SHADOW (Dot Product)
        // ==========================================
        // First, get the vector from A to our vertex Q
        double vecToQX = vertex.Cox - pointA.X;
        double vecToQY = vertex.Coy - pointA.Y;

        // The Dot Product math: (X * X) + (Y * Y)
        // This calculates exactly how far along the wall the shadow falls.
        double shadowProgress = (vecToQX * dirX) + (vecToQY * dirY);

        // Now we find the exact X,Y coordinates of the Shadow (S)
        // We start at A, and move forward by 'shadowProgress' pixels.
        double shadowX = pointA.X + (dirX * shadowProgress);
        double shadowY = pointA.Y + (dirY * shadowProgress);

        // ==========================================
        // STEP 3: THE OFFSET VECTOR
        // ==========================================
        // The arrow pointing from the Shadow (S) to the Dot (Q)
        double offsetX = vertex.Cox - shadowX;
        double offsetY = vertex.Coy - shadowY;

        // todo absolute distance from the line, will have to use this
        // in the calculation for the lateral oscillation

        // ==========================================
        // NEW STEP 3.5: THE SIGNED DISTANCE
        // ==========================================
        // We take the wall's forward direction and rotate it 90 degrees 
        // using YOUR exact rule: x = y, y = -x
        double lateralDirX = dirY;
        double lateralDirY = -dirX;


        // We use the Dot Product to compare our Offset Vector against this Lateral Direction.
        // This gives us our exact distance from the line!
        // Positive number = We are on the "Right" side of the line.
        // Negative number = We are on the "Left" side of the line.
        double signedDistance = (offsetX * lateralDirX) + (offsetY * lateralDirY);


        // ==========================================
        // NEW STEP 3.6: SPRING PHYSICS (THE CORRIDOR)
        // ==========================================
        // 1. Use the Hash to get the scrambled Halton value!
        uint id = (uint)vertex.ID;
        uint hashY = id * 2246822519u;
        int indexY = (int)(hashY % (uint)uniqueProps.Noise.HaltonValuesY.Length);
        
        double halton = uniqueProps.Noise.HaltonValuesY[indexY];
        
        // 2. Scale BOTH the Deadzone AND the Speed!
        double personalDeadzone = uniqueProps.DeadzoneRadius * halton;
        double personalMinSpeed = uniqueProps.MinLateralSpeed * halton;
       
        if (signedDistance > personalDeadzone)
        {
            double excessDistance = signedDistance - personalDeadzone;
            vertex.LateralVector -= (excessDistance * uniqueProps.SpringStiffness);
        }
        else if (signedDistance < -personalDeadzone)
        {
            double excessDistance = Math.Abs(signedDistance) - personalDeadzone;
            vertex.LateralVector += (excessDistance * uniqueProps.SpringStiffness);
        }
        else
        {
            // Now, if personalMinSpeed is 0, it won't kick it outward!
            if (Math.Abs(vertex.LateralVector) < personalMinSpeed)
            {
                vertex.LateralVector = (vertex.LateralVector >= 0) ? personalMinSpeed : -personalMinSpeed; 
            }
        }


        // todo absolute ends here

        // ==========================================
        // STEP 4: FINDING B'
        // ==========================================
        // We take Point B, and apply the exact same offset arrow to it.
        double bPrimeX = pointB.X + offsetX;
        double bPrimeY = pointB.Y + offsetY;

        // ==========================================
        // STEP 5: MOVE THE DOT
        // ==========================================
        // We tell the vertex to look at B'. 
        // Because Q and B' share the same offset, the forward direction 
        // calculated inside this function will be perfectly parallel to AB!

        // For this test, we set Flow to 10, and Lateral to 0.
        // (If Lateral is 0, it should perfectly follow the parallel line without drifting).
        vertex.CalculateVectorDirection(bPrimeX, bPrimeY, 10, vertex.LateralVector);

        // ==========================================
        // STEP 6: DID WE PASS B?
        // ==========================================
        // If the shadow has traveled further than the total length of the wall,
        // it means we have passed point B! Time to target the next point.
        if (shadowProgress >= wallLength)
        {
            ++vertex.TargetPathIndex;
            // todo  this is where the cutoff should be for spawn despawn.


            /* // Loop back to the start if we reached the end of the whole path
             if (vertex.TargetPathIndex > uniqueProps.TorrentPath.Length - 1)
             {
                 vertex.TargetPathIndex = 1;

                 // todo the respawn and despawn logic should come here.
                 vertex.Cox = uniqueProps.TorrentPath[0].X;
                 vertex.Coy = uniqueProps.TorrentPath[0].Y;
             }*/
        }

        return vertex;
    }
}
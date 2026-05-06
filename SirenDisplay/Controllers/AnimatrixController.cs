using System;
using SirenDisplay.Model;

namespace SirenDisplay.Controllers;

/// <summary>
/// the deduplication class
/// collect all the functions for animations here, should they be used by STC, Mesh or anything
/// </summary>
public sealed class AnimatrixController
{
    public void Rotate(Vertex shape, double angleInRadians)
    {
        //todo, with this it should be put in place, we'll see tho, should find a way to test it
        double tempex = shape.Cox;
        double tempey = shape.Coy;

        // Calculate the sine and cosine of the angle
        double cosTheta = Math.Cos(angleInRadians);
        double sinTheta = Math.Sin(angleInRadians);

        // Apply the rotation formulas and round them too
        shape.Cox =Math.Round( tempex * cosTheta - tempey * sinTheta, 2);
        shape.Coy = Math.Round( tempex * sinTheta + tempey * cosTheta, 2);
    }
    ///todo figure out a way so that it works on all resolutions
    /// something to do with the ratios (extra math yay)
    

    public void ShapeConnected(bool cycleConnected)
    {
        //todo im way tooo ded
    }
    // for ones that have a single begin and end
    public void ShapeChain(Vertex[] shapes)
    {
        //todo
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
        while (index>0)
        {
            fractiMulti/=bass;
            result += fractiMulti * (index % bass);
            index /= bass;
        }
        return result;
    }

    public Vertex Vortexer(VortexProperties uniqueProps, Vertex vertex)
    {
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
        /*double deadzoneRadius = 20.0; // The boundary of our tube
        double springStiffness = 0.05; // How hard the rubber band pulls back (0.1 is smooth, 0.5 is snappy)
        double minLateralSpeed = 20; */
        // If we haven't set a lateral speed yet, give it a starting push!
        if (vertex.LateralVector == 0) vertex.LateralVector = uniqueProps.MinLateralSpeed;

        // this IF ELSE checks the OUTSIDE of the deadzone
        // Check if we crossed the RIGHT boundary
        if (signedDistance > uniqueProps.DeadzoneRadius)
        {
            
            // Calculate how far past the boundary we are
            double excessDistance = signedDistance - uniqueProps.DeadzoneRadius;
        
            // The rubber band pulls us to the LEFT (negative)
            // If we are 10px past, it pulls with a force of -1.0. If 100px past, force is -10.0!
            vertex.LateralVector -= (excessDistance * uniqueProps.SpringStiffness);
        }
        // Check if we crossed the LEFT boundary
        else if (signedDistance < -uniqueProps.DeadzoneRadius)
        {
            // Calculate how far past the boundary we are (using Math.Abs to make it positive for the math)
            double excessDistance = Math.Abs(signedDistance) - uniqueProps.DeadzoneRadius;
        
            // The rubber band pulls us to the RIGHT (positive)
            vertex.LateralVector += (excessDistance * uniqueProps.SpringStiffness);
        }
        // 3. INSIDE THE DEADZONE!
        else
        {
            // We are safely inside the deadzone. 
            // Let's check if the vertex is being "lazy" (moving too slow).
            // Math.Abs turns -1 into 1, so we can check the raw speed regardless of direction.
            if (Math.Abs(vertex.LateralVector) < uniqueProps.MinLateralSpeed)
            {
                // It is moving too slow! Which way was it trying to go?
                if (vertex.LateralVector >= 0)
                {
                    // It was moving Right (or exactly 0). Boost it to the Right!
                    vertex.LateralVector = uniqueProps.MinLateralSpeed; 
                }
                else
                {
                    // It was moving Left. Boost it to the Left!
                    vertex.LateralVector = -uniqueProps.MinLateralSpeed; 
                }
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
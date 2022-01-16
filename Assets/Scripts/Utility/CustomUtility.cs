using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap
{
    public static class CustomUtility
    {
        // This functions is useful for converting Leap Vectors into Unity's Vector3. I've 
        // changed it's output to work with the mapping functionality required of it
        public static Vector3 LeapMapHands(Vector vector)
        {
            return new Vector3(vector.x/2.5f, vector.y/5f, -vector.z)/1000f;
        }

        // This function returns the Leap Vector as a Unity Vector3 as it should be. I've only called
        // it in relation to the Orientation in case I needed to change the outputs
        public static Vector3 LeapMapOrientation(Vector vector)
        {
            return LeapToVector3(vector);
        }
        
        // Converts a Leap Vector to a Vector3
        public static Vector3 LeapToVector3(Vector vector)
        {
            return new Vector3(vector.x, vector.y, -vector.z);
        }


    }




}




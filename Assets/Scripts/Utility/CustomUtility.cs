using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap
{
    public static class CustomUtility
    {
        public static Vector3 LeapVectorToUnityVector3(Vector vector)
        {
            return new Vector3(-vector.x, -vector.z, vector.y) / 1000f;
        }

        public static Vector3 LeapMapHands(Vector vector)
        {
            return new Vector3(vector.x/2.5f, vector.y/5f, -vector.z)/1000f;
        }

        public static Vector3 LeapMapOrientation(Vector vector)
        {
            return new Vector3(vector.x, vector.y, -vector.z);
        }


    }




}




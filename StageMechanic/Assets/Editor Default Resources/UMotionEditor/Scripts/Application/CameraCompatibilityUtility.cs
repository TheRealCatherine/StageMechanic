using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMotionEditor
{
    public static class CameraCompatibilityUtility
    {
        //********************************************************************************
        // Public Properties
        //********************************************************************************

        //********************************************************************************
        // Private Properties
        //********************************************************************************

        //********************************************************************************
        // Public Methods
        //********************************************************************************

        public static void SetAllowHdr(Camera camera, bool hdrAllowed)
        {
            #if UNITY_5_6_OR_NEWER
            camera.allowHDR = hdrAllowed;
            #else
            camera.hdr = hdrAllowed;
            #endif
        }

        public static bool GetAllowHdr(Camera camera)
        {
            #if UNITY_5_6_OR_NEWER
            return camera.allowHDR;
            #else
            return camera.hdr;
            #endif
        }

        //********************************************************************************
        // Private Methods
        //********************************************************************************
    }
}

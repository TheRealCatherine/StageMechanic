using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMotionEditor
{
    public static class VersionCompatibilityUtility
    {
        //********************************************************************************
        // Public Properties
        //********************************************************************************

        public static bool Unity2017_3_OrNewer
        {
            get
            {
                #if UNITY_2017_3_OR_NEWER
                return true;
                #else
                return false;
                #endif
            }
        }

        public static bool HasDepthSortingBug
        {
            get
            {
                #if UNITY_2017_3
                return true;
                #else
                return false;
                #endif
            }
        }

        //********************************************************************************
        // Private Properties
        //********************************************************************************

        //********************************************************************************
        // Public Methods
        //********************************************************************************

        //********************************************************************************
        // Private Methods
        //********************************************************************************
    }
}

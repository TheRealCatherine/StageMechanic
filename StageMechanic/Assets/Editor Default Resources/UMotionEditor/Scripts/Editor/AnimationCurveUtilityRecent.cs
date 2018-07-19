using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UMotionEditor
{
	public static class AnimationCurveUtilityRecent
	{
		//********************************************************************************
		// Public Properties
		//********************************************************************************

        public static bool Implemented
        {
            get
            {
                #if UNITY_5_5_OR_NEWER
                return true;
                #else
                return false;
                #endif
            }
        }

		//********************************************************************************
		// Private Properties
		//********************************************************************************
		
		//----------------------
		// Inspector
		//----------------------
		
		//----------------------
		// Internal
		//----------------------

		//********************************************************************************
		// Public Methods
		//********************************************************************************
		
        public static void SetKeyBroken(AnimationCurve curve, int index, bool broken)
        {
            #if UNITY_5_5_OR_NEWER
            AnimationUtility.SetKeyBroken(curve, index, broken);
            #endif
        }

        public static void SetKeyLeftTangentMode(AnimationCurve curve, int index, int tangentMode)
        {
            #if UNITY_5_5_OR_NEWER
            AnimationUtility.SetKeyLeftTangentMode(curve, index, (AnimationUtility.TangentMode)tangentMode);
            #endif
        }

        public static void SetKeyRightTangentMode(AnimationCurve curve, int index, int tangentMode)
        {
            #if UNITY_5_5_OR_NEWER
            AnimationUtility.SetKeyRightTangentMode(curve, index, (AnimationUtility.TangentMode)tangentMode);
            #endif
        }

		//********************************************************************************
		// Private Methods
		//********************************************************************************
	}
}

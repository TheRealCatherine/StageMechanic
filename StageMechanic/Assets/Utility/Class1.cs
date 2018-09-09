using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class HWStats : MonoBehaviour
{
	private void Awake()
	{
		var type = Type.GetType("UnityEditor.PlayerSettings,UnityEditor");
		if (type != null)
		{
			var propertyInfo = type.GetProperty("submitAnalytics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			if (propertyInfo != null)
			{
				{
					var value = (bool)propertyInfo.GetValue(null, null);
					Debug.LogFormat("PlayerSettings.submitAnalytics {0}", value);
				}
				if (propertyInfo.CanWrite)
				{
					propertyInfo.SetValue(null, false, null);

					var value = (bool)propertyInfo.GetValue(null, null);
					Debug.LogFormat("PlayerSettings.submitAnalytics {0}", value);
				}
			}
		}
	}
}
#endif

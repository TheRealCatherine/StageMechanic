using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaProxy_SkyboxManager
{
    public int count
	{
		get
		{
			return SkyboxManager.Instance.Skyboxes.Length;
		}
	}

	public int index
	{
		get
		{
			return Array.IndexOf(SkyboxManager.Instance.Skyboxes, RenderSettings.skybox);
		}
		set
		{
			SkyboxManager.SetSkybox(value);
		}
	}

	public void next()
	{
		SkyboxManager.NextSkybox();
	}
}

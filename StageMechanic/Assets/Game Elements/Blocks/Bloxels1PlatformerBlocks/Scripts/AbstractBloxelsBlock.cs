using UnityEngine;
using System.Collections;

public abstract class AbstractBloxelsBlock : AbstractBlock
{

	MeshRenderer Placeholder;
	MeshRenderer StaticMesh;

	public override void Awake()
	{
		base.Awake();
		GravityFactor = 0;
	}

	internal override void Update()
	{
		base.Update();
		if (Placeholder != null && StaticMesh != null)
		{
			if (BlockManager.PlayMode)
			{
				StaticMesh.gameObject.SetActive(true);
				Placeholder.gameObject.SetActive(false);
			}
			else
			{
				StaticMesh.gameObject.SetActive(false);
				Placeholder.gameObject.SetActive(true);
			}
		}
	}

	public abstract override string TypeName { get; set; }

	public abstract void ApplyTheme(AbstractBlockTheme theme);
}

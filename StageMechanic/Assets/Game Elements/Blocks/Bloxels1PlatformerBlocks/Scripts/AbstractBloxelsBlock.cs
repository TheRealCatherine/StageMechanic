using UnityEngine;
using System.Collections;

public abstract class AbstractBloxelsBlock : AbstractBlock
{

	public MeshRenderer Placeholder;
	public MeshRenderer StaticMesh;

	protected MeshRenderer PlaceholderInstance;
	protected MeshRenderer StaticMeshInstance;

	public override void Awake()
	{
		base.Awake();
		GravityFactor = 0;
		if(Placeholder != null)
			PlaceholderInstance = Instantiate(Placeholder, transform);
		if (StaticMesh != null)
		{
			StaticMeshInstance = Instantiate(StaticMesh, transform);
			StaticMeshInstance.gameObject.SetActive(false);
		}
	}

	internal override void Update()
	{
		base.Update();
		if (Placeholder != null && StaticMesh != null)
		{
			if (BlockManager.PlayMode)
			{
				StaticMeshInstance.gameObject.SetActive(true);
				PlaceholderInstance.gameObject.SetActive(false);
			}
			else
			{
				StaticMeshInstance.gameObject.SetActive(false);
				PlaceholderInstance.gameObject.SetActive(true);
			}
		}
	}

	public abstract override string TypeName { get; set; }

	public abstract void ApplyTheme(AbstractBlockTheme theme);
}

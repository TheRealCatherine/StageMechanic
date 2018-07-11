using UnityEngine;

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

		GameManager.GameMode currentMode = (BlockManager.PlayMode ? GameManager.GameMode.Play : GameManager.GameMode.StageEdit);
		if (Placeholder != null && StaticMesh != null)
		{
			if (currentMode == GameManager.GameMode.StageEdit)
			{
				StaticMeshInstance.gameObject.SetActive(false);
				PlaceholderInstance.gameObject.SetActive(true);
			}
			else if (currentMode == GameManager.GameMode.Play)
			{
				StaticMeshInstance.gameObject.SetActive(true);
				PlaceholderInstance.gameObject.SetActive(false);
			}
		}

		if (BlockManager.PlayMode && _lastMode != GameManager.GameMode.Play)
		{
			OnGameModeChange(_lastMode, GameManager.GameMode.Play);
		}
		else if((!BlockManager.PlayMode) && _lastMode != GameManager.GameMode.StageEdit)
		{
			OnGameModeChange(_lastMode, GameManager.GameMode.StageEdit);
		}
	}

	//TODO clean this up when refactoring BlockManager code into GameManager
	//TODO move this to AbstractBlock
	//TODO this is not always getting called after an undo
	private GameManager.GameMode _lastMode = (BlockManager.PlayMode?GameManager.GameMode.Play:GameManager.GameMode.StageEdit);
	internal virtual void OnGameModeChange( GameManager.GameMode oldMode, GameManager.GameMode newMode )
	{
		_lastMode = newMode;
	}

	public abstract override string TypeName { get; set; }

	public abstract void ApplyTheme(AbstractBlockTheme theme);
}

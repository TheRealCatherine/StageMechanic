using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockThemeListPopulator : MonoBehaviour {

	//TODO make this generic
	//TODO combine this with the item one
	public Cathy1BlockFactory Cathy1BlockFactory;
	public Button ButtonPrefab;
	public BlockTypeScrollBoxPopulator BlockList;
	public ScrollRect Box;

	void Start()
	{
		foreach (Cathy1BlockTheme theme in Cathy1BlockFactory.Themes)
		{
			Debug.Log(theme.Name);
			Sprite icon = theme.Icon;
			if(icon == null)
				icon = theme.BasicBlockIcon;
			if (icon != null)
			{
				Button newButton = Instantiate(ButtonPrefab, transform) as Button;
				newButton.GetComponentInChildren<Text>().text = theme.Name;
				newButton.image.sprite = icon;
				newButton.onClick.AddListener(OnThemeClicked);
			}
		}
	}

	void OnThemeClicked()
	{
		Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
		foreach (Cathy1BlockTheme theme in Cathy1BlockFactory.Themes)
		{
			if (theme.Name == clickedButton.GetComponentInChildren<Text>().text)
			{
				Cathy1BlockFactory.ApplyTheme(theme);
				BlockList.Repopulate();
				Box.gameObject.SetActive(false);
				return;
			}
		}
	}
}

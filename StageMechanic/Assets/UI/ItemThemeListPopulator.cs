using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemThemeListPopulator : MonoBehaviour {

	//TODO make this generic
	//TODO combine this with the block one
	public Cat5ItemFactory Cat5ItemFactory;
	public Button ButtonPrefab;
	public ItemTypeScrollBoxPopulator ItemList;
	public ScrollRect Box;

	void Start()
	{
		foreach (Cat5ItemTheme theme in Cat5ItemFactory.Themes)
		{
			Debug.Log(theme.Name);
			Sprite icon = theme.Icon;
			if(icon == null)
				icon = theme.XFactorIcon;
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
		foreach (Cat5ItemTheme theme in Cat5ItemFactory.Themes)
		{
			if (theme.Name == clickedButton.GetComponentInChildren<Text>().text)
			{
				Cat5ItemFactory.ApplyTheme(theme);
				ItemList.Repopulate();
				//Box.gameObject.SetActive(false);
				return;
			}
		}
	}
}

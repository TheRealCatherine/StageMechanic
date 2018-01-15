using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTypleScrollBoxPopulator : MonoBehaviour {

    public Button ButtonPrefab;

	// Use this for initialization
	void Start () {
        Cathy1BlockFactory factory = BlockManager.Instance.Cathy1BlockFactory;
        List<string> types = factory.BlockTypeNames;
        int count = 0;
        foreach(string name in types)
        {
            Sprite icon = factory.IconForType(name);
            if (icon != null)
            {
                Button newButton = Instantiate(ButtonPrefab, transform) as Button;
                newButton.GetComponentInChildren<Text>().text = name;
                newButton.image.sprite = icon;
                newButton.onClick.AddListener(OnBlockClicked);
                ++count;
            }
        }
	}

    void OnBlockClicked()
    {
        Cathy1BlockFactory factory = BlockManager.Instance.Cathy1BlockFactory;
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        BlockManager.Instance.CreateBlockAtCursor("Cathy1 Internal", clickedButton.GetComponentInChildren<Text>().text);
    }
}

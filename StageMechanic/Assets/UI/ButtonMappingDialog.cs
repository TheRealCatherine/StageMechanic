using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMappingDialog : MonoBehaviour
{
    public GameObject MainLayout;
    public GameObject ButtonMappingText;
    public GameObject ButtonMappingButton;
    public GameObject AddItemButton;

    private List<GameObject> addedItems = new List<GameObject>();

    private void Start()
    {
        Populate();
    }

    private void OnEnable()
    {
        Populate();
    }

    private void OnDisable()
    {
        foreach (GameObject obj in addedItems)
            Destroy(obj);
        addedItems.Clear();
    }

    public void Refresh()
    {
        if (!enabled)
            return;
        foreach (GameObject obj in addedItems)
            Destroy(obj);
        addedItems.Clear();
        Populate();
    }

    public void Populate()
    {
        Dictionary<string, string[]> list = PlayerManager.Player1InputOptions;
        if (list == null)
            return;
        foreach (KeyValuePair<string, string[]> item in list)
        {
            Text text = Instantiate(ButtonMappingText, MainLayout.transform).GetComponent<Text>();
            text.text = item.Key;
            Text separator = Instantiate(ButtonMappingText, MainLayout.transform).GetComponent<Text>();
            addedItems.Add(text.gameObject);
            addedItems.Add(separator.gameObject);

            foreach (string key in item.Value)
            {
                Button button = Instantiate(ButtonMappingButton, MainLayout.transform).GetComponent<Button>();
                button.GetComponentInChildren<Text>().text = key;
                button.onClick.AddListener(OnRemoveClicked);
                addedItems.Add(button.gameObject);

            }
            Button addButton = Instantiate(AddItemButton, MainLayout.transform).GetComponent<Button>();
            addedItems.Add(addButton.gameObject);

            if ((item.Value.Length+1) % 2 != 0)
                addedItems.Add(Instantiate(ButtonMappingText, MainLayout.transform));
        }
    }

    private void OnRemoveClicked()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        PlayerManager.RemoveKeyBinding(0, null, clickedButton.GetComponentInChildren<Text>().text);
        Refresh();
    }
}

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
    public enum State
    {
        Normal,
        WaitingForKey
    }

    public State CurrentState = State.Normal;
    private string currentActionGroup;

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
                button.GetComponent<ButtonMappingButtonProperties>().ActionName = item.Key;
                button.GetComponent<ButtonMappingButtonProperties>().Value = key;
                button.onClick.AddListener(OnRemoveClicked);
                addedItems.Add(button.gameObject);

            }
            Button addButton = Instantiate(AddItemButton, MainLayout.transform).GetComponent<Button>();
            addButton.GetComponent<ButtonMappingButtonProperties>().ActionName = item.Key;
            addButton.onClick.AddListener(OnAddClicked);
            addedItems.Add(addButton.gameObject);

            if ((item.Value.Length+1) % 2 != 0)
                addedItems.Add(Instantiate(ButtonMappingText, MainLayout.transform));
        }
    }

    private void OnRemoveClicked()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        PlayerManager.RemoveKeyBinding(0, clickedButton.GetComponent<ButtonMappingButtonProperties>().ActionName, clickedButton.GetComponent<ButtonMappingButtonProperties>().Value);
        Refresh();
    }

    private void OnAddClicked()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        clickedButton.interactable = false;
        currentActionGroup = clickedButton.GetComponent<ButtonMappingButtonProperties>().ActionName;
        CurrentState = State.WaitingForKey;
    }

    public void RegisterKey(string key)
    {
        PlayerManager.AddKeyBinding(0, currentActionGroup, key);
        CurrentState = State.Normal;
        Refresh();
    }

    private void Update()
    {
        if (CurrentState != State.WaitingForKey)
            return;

        //Arrows
        if (Input.GetKey(KeyCode.UpArrow))
            RegisterKey("up");
        else if (Input.GetKey(KeyCode.DownArrow))
            RegisterKey("down");
        else if (Input.GetKey(KeyCode.LeftArrow))
            RegisterKey("left");
        else if (Input.GetKey(KeyCode.RightArrow))
            RegisterKey("right");

        //Modifiers
        else if (Input.GetKey(KeyCode.LeftShift))
            RegisterKey("left shift");
        else if (Input.GetKey(KeyCode.RightShift))
            RegisterKey("right shift");
        else if (Input.GetKey(KeyCode.LeftControl))
            RegisterKey("left control");
        else if (Input.GetKey(KeyCode.RightControl))
            RegisterKey("right control");
        else if (Input.GetKey(KeyCode.LeftAlt))
            RegisterKey("left alt");
        else if (Input.GetKey(KeyCode.RightAlt))
            RegisterKey("right alt");

        //Numbers
        else if (Input.GetKey(KeyCode.Alpha1))
            RegisterKey("1");
        else if (Input.GetKey(KeyCode.Alpha2))
            RegisterKey("2");
        else if (Input.GetKey(KeyCode.Alpha3))
            RegisterKey("3");
        else if (Input.GetKey(KeyCode.Alpha4))
            RegisterKey("4");
        else if (Input.GetKey(KeyCode.Alpha5))
            RegisterKey("5");
        else if (Input.GetKey(KeyCode.Alpha6))
            RegisterKey("6");
        else if (Input.GetKey(KeyCode.Alpha7))
            RegisterKey("7");
        else if (Input.GetKey(KeyCode.Alpha8))
            RegisterKey("8");
        else if (Input.GetKey(KeyCode.Alpha9))
            RegisterKey("9");
        else if (Input.GetKey(KeyCode.Alpha0))
            RegisterKey("0");

        //Letters
        else if (Input.GetKey(KeyCode.W))
            RegisterKey("w");
        else if (Input.GetKey(KeyCode.A))
            RegisterKey("a");
        else if (Input.GetKey(KeyCode.S))
            RegisterKey("s");
        else if (Input.GetKey(KeyCode.D))
            RegisterKey("d");
    }
}

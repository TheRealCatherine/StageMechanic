/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using CnControls;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMappingDialog : MonoBehaviour
{
    public GameObject MainLayout;
    public GameObject ButtonMappingText;
    public GameObject ButtonMappingButton;
    public GameObject AddItemButton;
    public Text Gamepad1Name;

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

    public void Populate(int playerNumber = 0)
    {
        Debug.Assert(Gamepad1Name != null);
        string[] gamepads = Input.GetJoystickNames();
        if (gamepads.Length > playerNumber)
            Gamepad1Name.text = gamepads[playerNumber];


        Dictionary<string, string[]> list = PlayerManager.PlayerInputOptions(playerNumber);
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

    public void TryRegisterJoystick()
    {

        if (CnInputManager.GetAxis("joystick 1 3rd axis") > 0f)
            RegisterKey("joystick 1 3rd axis +");
        else if (CnInputManager.GetAxis("joystick 1 3rd axis") < 0f)
            RegisterKey("joyustick 1 3rd axis -");
        else if (CnInputManager.GetAxis("joystick 1 4th axis") > 0f)
            RegisterKey("joystick 1 4th axis +");
        else if (CnInputManager.GetAxis("joystick 1 4th axis") < 0f)
            RegisterKey("joyustick 1 4th axis -");
        else if (CnInputManager.GetAxis("joystick 1 5th axis") > 0f)
            RegisterKey("joystick 1 5th axis +");
        else if (CnInputManager.GetAxis("joystick 1 5th axis") < 0f)
            RegisterKey("joyustick 1 5th axis -");
        else if (CnInputManager.GetAxis("joystick 1 6th axis") > 0f)
            RegisterKey("joystick 1 6th axis +");
        else if (CnInputManager.GetAxis("joystick 1 6th axis") < 0f)
            RegisterKey("joyustick 1 6th axis -");
        else if (CnInputManager.GetAxis("joystick 1 7th axis") > 0f)
            RegisterKey("joyustick 1 6th axis +");
        else if (CnInputManager.GetAxis("joystick 1 7th axis") < 0f)
            RegisterKey("joyustick 1 7th axis -");
        else if (CnInputManager.GetAxis("joystick 1 8th axis") > 0f)
            RegisterKey("joyustick 1 8th axis +");
        else if (CnInputManager.GetAxis("joystick 1 8th axis") < 0f)
            RegisterKey("joyustick 1 8th axis -");

        else if (CnInputManager.GetAxis("joystick 1 Y axis") < 0)
            RegisterKey("joystick 1 Y axis +");
        else if (CnInputManager.GetAxis("joystick 1 Y axis") > 0)
            RegisterKey("joystick 1 Y axis -");
        else if (CnInputManager.GetAxis("joystick 1 X axis") < 0)
            RegisterKey("joystick 1 X axis -");
        else if (CnInputManager.GetAxis("joystick 1 X axis") > 0)
            RegisterKey("joystick 1 X axis +");


        //Joystick 1 Buttons
        else if (Input.GetKey(KeyCode.Joystick1Button0))
            RegisterKey("joystick 1 button 0");
        else if (Input.GetKey(KeyCode.Joystick1Button1))
            RegisterKey("joystick 1 button 1");
        else if (Input.GetKey(KeyCode.Joystick1Button2))
            RegisterKey("joystick 1 button 2");
        else if (Input.GetKey(KeyCode.Joystick1Button3))
            RegisterKey("joystick 1 button 3");
        else if (Input.GetKey(KeyCode.Joystick1Button4))
            RegisterKey("joystick 1 button 4");
        else if (Input.GetKey(KeyCode.Joystick1Button5))
            RegisterKey("joystick 1 button 5");
        else if (Input.GetKey(KeyCode.Joystick1Button6))
            RegisterKey("joystick 1 button 6");
        else if (Input.GetKey(KeyCode.Joystick1Button7))
            RegisterKey("joystick 1 button 7");
        else if (Input.GetKey(KeyCode.Joystick1Button8))
            RegisterKey("joystick 1 button 8");
        else if (Input.GetKey(KeyCode.Joystick1Button9))
            RegisterKey("joystick 1 button 9");
        else if (Input.GetKey(KeyCode.Joystick1Button10))
            RegisterKey("joystick 1 button 10");
        else if (Input.GetKey(KeyCode.Joystick1Button11))
            RegisterKey("joystick 1 button 11");
        else if (Input.GetKey(KeyCode.Joystick1Button12))
            RegisterKey("joystick 1 button 12");
        else if (Input.GetKey(KeyCode.Joystick1Button13))
            RegisterKey("joystick 1 button 13");
        else if (Input.GetKey(KeyCode.Joystick1Button14))
            RegisterKey("joystick 1 button 14");
        else if (Input.GetKey(KeyCode.Joystick1Button15))
            RegisterKey("joystick 1 button 15");
        else if (Input.GetKey(KeyCode.Joystick1Button16))
            RegisterKey("joystick 1 button 16");
        else if (Input.GetKey(KeyCode.Joystick1Button17))
            RegisterKey("joystick 1 button 17");
        else if (Input.GetKey(KeyCode.Joystick1Button18))
            RegisterKey("joystick 1 button 18");
        else if (Input.GetKey(KeyCode.Joystick1Button19))
            RegisterKey("joystick 1 button 19");

        else if (CnInputManager.GetButton("Grab"))
            RegisterKey("Grab");

        //Joystick 2

        //Joystick 2 D-Pad
        if (CnInputManager.GetAxis("joystick 2 6th axis") > 0f)
            RegisterKey("joystick 2 6th axis +");
        else if (CnInputManager.GetAxis("joystick 2 6th axis") < 0f)
            RegisterKey("joyustick 2 6th axis -");
        else if (CnInputManager.GetAxis("joystick 2 7th axis") > 0f)
            RegisterKey("joyustick 2 6th axis +");
        else if (CnInputManager.GetAxis("joystick 2 7th axis") < 0f)
            RegisterKey("joyustick 2 7th axis -");

        //Joystick 2 Left Stick
        else if (CnInputManager.GetAxis("joystick 2 Y axis") < 0)
            RegisterKey("joystick 2 Y axis +");
        else if (CnInputManager.GetAxis("joystick 2 Y axis") > 0)
            RegisterKey("joystick 2 Y axis -");
        else if (CnInputManager.GetAxis("joystick 2 X axis") < 0)
            RegisterKey("joystick 2 X axis -");
        else if (CnInputManager.GetAxis("joystick 2 X axis") > 0)
            RegisterKey("joystick 2 X axis +");

        //Joystick 2 Buttons
        else if (Input.GetKey(KeyCode.Joystick2Button0))
            RegisterKey("joystick 2 button 0");
        else if (Input.GetKey(KeyCode.Joystick2Button1))
            RegisterKey("joystick 2 button 1");
        else if (Input.GetKey(KeyCode.Joystick2Button2))
            RegisterKey("joystick 2 button 2");
        else if (Input.GetKey(KeyCode.Joystick2Button3))
            RegisterKey("joystick 2 button 3");
        else if (Input.GetKey(KeyCode.Joystick2Button4))
            RegisterKey("joystick 2 button 4");
        else if (Input.GetKey(KeyCode.Joystick2Button5))
            RegisterKey("joystick 2 button 5");
        else if (Input.GetKey(KeyCode.Joystick2Button6))
            RegisterKey("joystick 2 button 6");
        else if (Input.GetKey(KeyCode.Joystick2Button7))
            RegisterKey("joystick 2 button 7");
        else if (Input.GetKey(KeyCode.Joystick2Button8))
            RegisterKey("joystick 2 button 8");
        else if (Input.GetKey(KeyCode.Joystick2Button9))
            RegisterKey("joystick 2 button 9");
        else if (Input.GetKey(KeyCode.Joystick2Button10))
            RegisterKey("joystick 2 button 10");
        else if (Input.GetKey(KeyCode.Joystick2Button11))
            RegisterKey("joystick 2 button 11");
        else if (Input.GetKey(KeyCode.Joystick2Button12))
            RegisterKey("joystick 2 button 12");
        else if (Input.GetKey(KeyCode.Joystick2Button13))
            RegisterKey("joystick 2 button 13");
        else if (Input.GetKey(KeyCode.Joystick2Button14))
            RegisterKey("joystick 2 button 14");
        else if (Input.GetKey(KeyCode.Joystick2Button15))
            RegisterKey("joystick 2 button 15");
        else if (Input.GetKey(KeyCode.Joystick2Button16))
            RegisterKey("joystick 2 button 16");
        else if (Input.GetKey(KeyCode.Joystick2Button17))
            RegisterKey("joystick 2 button 17");
        else if (Input.GetKey(KeyCode.Joystick2Button18))
            RegisterKey("joystick 2 button 18");
        else if (Input.GetKey(KeyCode.Joystick2Button19))
            RegisterKey("joystick 2 button 19");

        //Joystick 3 Left Stick
        else if (CnInputManager.GetAxis("joystick 3 Y axis") < 0)
            RegisterKey("joystick 3 Y axis +");
        else if (CnInputManager.GetAxis("joystick 3 Y axis") > 0)
            RegisterKey("joystick 3 Y axis -");
        else if (CnInputManager.GetAxis("joystick 3 X axis") < 0)
            RegisterKey("joystick 3 X axis -");
        else if (CnInputManager.GetAxis("joystick 3 X axis") > 0)
            RegisterKey("joystick 3 X axis +");

        //Joystick 4 Left Stick
        else if (CnInputManager.GetAxis("joystick 4 Y axis") < 0)
            RegisterKey("joystick 4 Y axis +");
        else if (CnInputManager.GetAxis("joystick 4 Y axis") > 0)
            RegisterKey("joystick 4 Y axis -");
        else if (CnInputManager.GetAxis("joystick 4 X axis") < 0)
            RegisterKey("joystick 4 X axis -");
        else if (CnInputManager.GetAxis("joystick 4 X axis") > 0)
            RegisterKey("joystick 4 X axis +");
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
            Debug.Log("Detected key code: " + e.keyCode);
    }

    private void Update()
    {
        if (CurrentState != State.WaitingForKey)
            return;

        TryRegisterJoystick();

        //Modifiers
        if (Input.GetKey(KeyCode.LeftShift))
            RegisterKey("left shift");
        else if (Input.GetKey(KeyCode.RightShift))
            RegisterKey("right shift");
        else if (Input.GetKey(KeyCode.LeftControl))
            RegisterKey("left ctrl");
        else if (Input.GetKey(KeyCode.RightControl))
            RegisterKey("right ctrl");
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
        else if (Input.GetKey(KeyCode.Keypad1))
            RegisterKey("[1]");
        else if (Input.GetKey(KeyCode.Keypad2))
            RegisterKey("[2]");
        else if (Input.GetKey(KeyCode.Keypad3))
            RegisterKey("[3]");
        else if (Input.GetKey(KeyCode.Keypad4))
            RegisterKey("[4]");
        else if (Input.GetKey(KeyCode.Keypad5))
            RegisterKey("[5]");
        else if (Input.GetKey(KeyCode.Keypad6))
            RegisterKey("[6]");
        else if (Input.GetKey(KeyCode.Keypad7))
            RegisterKey("[7]");
        else if (Input.GetKey(KeyCode.Keypad8))
            RegisterKey("[8]");
        else if (Input.GetKey(KeyCode.Keypad9))
            RegisterKey("[9]");
        else if (Input.GetKey(KeyCode.Keypad0))
            RegisterKey("[0]");

        else if (Input.inputString != null && Input.inputString.Length > 0)
            RegisterKey(Input.inputString.ToLower());

    }
}

/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
 using CnControls;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockEditDialog : MonoBehaviour {

    public IBlock CurrentBlock;
    public Button BlockButton;
    public Dpad BlockTypeCycleButtons;
    public GameObject PropertyList;
    public InputField NameField;
    public InputField PosXField;
    public InputField PosYField;
    public InputField PosZField;

    public Button OKButton;
    public Button ApplyButton;
    public Button CancelButton;
    public Button ApplyToTypeButton;

    public Text ListLabelPrefab;
    public InputField ListStringFieldPrefab;

    private List<GameObject> addedFields = new List<GameObject>();
    private float period;

    void Start()
    {
        CancelButton.onClick.AddListener(Hide);
        ApplyButton.onClick.AddListener(Apply);
        OKButton.onClick.AddListener(Accept);
        ApplyToTypeButton.onClick.AddListener(ApplyToType);
    }

    public void Show(IBlock block = null)
    {
        Clear();
        CurrentBlock = block;
        BlockManager.Cursor.transform.position = block.Position;
        gameObject.SetActive(true);
        Refresh();
    }

    public void Clear()
    {
        CurrentBlock = null;
        foreach(GameObject obj in addedFields)
        {
            Destroy(obj);
        }
        addedFields.Clear();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Clear();
    }

    public void Apply()
    {
        if (CurrentBlock == null)
            return;
        if (!string.IsNullOrWhiteSpace(NameField.text))
            CurrentBlock.Name = NameField.text;
        Vector3 position = Vector3.zero;
        position.x = float.Parse(PosXField.text);
        position.y = float.Parse(PosYField.text);
        position.z = float.Parse(PosZField.text);
        CurrentBlock.Position = position;
        BlockManager.Cursor.transform.position = position;

        Dictionary<string, string> properties = new Dictionary<string, string>();
        foreach (GameObject obj in addedFields)
        {
            SinglePropertyWithDefault holder = obj.GetComponent<SinglePropertyWithDefault>();
            if (holder != null)
            {
                InputField field = holder.GetComponent<InputField>();
                if(field != null)
                {
                    if(!string.IsNullOrWhiteSpace(field.text))
                        properties.Add(holder.PropertyName, field.text);
                }
            }
        }
        if (properties.Count > 0)
            CurrentBlock.Properties = properties;
    }

    public void ApplyToType()
    {
        if (CurrentBlock == null)
            return;
        if (!string.IsNullOrWhiteSpace(NameField.text))
            CurrentBlock.Name = NameField.text;
        Vector3 position = Vector3.zero;
        position.x = float.Parse(PosXField.text);
        position.y = float.Parse(PosYField.text);
        position.z = float.Parse(PosZField.text);
        CurrentBlock.Position = position;
        BlockManager.Cursor.transform.position = position;

        Dictionary<string, string> properties = new Dictionary<string, string>();
        foreach (GameObject obj in addedFields)
        {
            SinglePropertyWithDefault holder = obj.GetComponent<SinglePropertyWithDefault>();
            if (holder != null)
            {
                InputField field = holder.GetComponent<InputField>();
                if (field != null)
                {
                    if (!string.IsNullOrWhiteSpace(field.text))
                        properties.Add(holder.PropertyName, field.text);
                }
            }
        }
        if (properties.Count > 0)
        {
            List<IBlock> blocks = BlockManager.GetBlocksOfType(CurrentBlock.TypeName);
            foreach(IBlock block in blocks)
                block.Properties = properties;
        }
    }

    public void Accept()
    {
        Apply();
        Hide();
    }

    public void Refresh()
    {
        if (CurrentBlock != null)
        {
            //TODO make this generic
            Sprite icon = BlockManager.Instance.Cathy1BlockFactory.IconForType(CurrentBlock.TypeName);
            BlockButton.image.sprite = icon;
            BlockButton.GetComponentInChildren<Text>().text = CurrentBlock.TypeName;
            NameField.text = CurrentBlock.Name;
            PosXField.text = CurrentBlock.Position.x.ToString();
            PosYField.text = CurrentBlock.Position.y.ToString();
            PosZField.text = CurrentBlock.Position.z.ToString();

            foreach(KeyValuePair<string,KeyValuePair<Type,string>> property in CurrentBlock.DefaultProperties)
            {
                Text label = Instantiate(ListLabelPrefab, PropertyList.transform) as Text ;
                addedFields.Add(label.gameObject);
                label.text = property.Key;

                InputField stringField = Instantiate(ListStringFieldPrefab, PropertyList.transform) as InputField;
                addedFields.Add(stringField.gameObject);
                stringField.placeholder.GetComponent<Text>().text = property.Value.Value;
                SinglePropertyWithDefault holder = stringField.GetComponent<SinglePropertyWithDefault>();
                Debug.Assert(holder != null);
                holder.PropertyName = property.Key;
                holder.PropertyType = property.Value.Key;
                holder.PropertyDefaultValue = property.Value.Value;

                foreach (KeyValuePair<string, string> setProperty in CurrentBlock.Properties)
                {
                    if (setProperty.Key == property.Key)
                        stringField.text = setProperty.Value;
                }

            }

        }
    }


    void Update () {
        period += Time.deltaTime;
        bool next = (CnInputManager.GetAxis("joystick 1 X axis") > 0f && period > InputManager.JoystickThrottleRate);
        bool prev = (CnInputManager.GetAxis("joystick 1 X axis") < 0f && period > InputManager.JoystickThrottleRate);
        if (next)
        {
            List<string> types = BlockManager.Instance.Cathy1BlockFactory.BlockTypeNames;
            int index = types.IndexOf(CurrentBlock.TypeName);
            if (++index >= types.Count)
                index = 0;
            IBlock newBlock = BlockManager.CreateBlockAt(CurrentBlock.Position,"Cathy1 Internal", types[index]);
            Clear();
            CurrentBlock = newBlock;
            Refresh();
            period = 0f;
        }
        else if (prev)
        {
            List<string> types = BlockManager.Instance.Cathy1BlockFactory.BlockTypeNames;
            int index = types.IndexOf(CurrentBlock.TypeName);
            if (--index <= 0 )
                index = types.Count-1;
            IBlock newBlock = BlockManager.CreateBlockAt(CurrentBlock.Position, "Cathy1 Internal", types[index]);
            Clear();
            CurrentBlock = newBlock;
            Refresh();
            period = 0f;
        }
    }
}

/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using CnControls;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEditDialog : MonoBehaviour {

    public IItem CurrentItem;
    public Button ItemButton;
    public Dpad BlockTypeCycleButtons;
    public GameObject PropertyList;
    public InputField NameField;
    public InputField PosXField;
    public InputField PosYField;
    public InputField PosZField;

    public Button OKButton;
   // public Button ApplyButton;
    public Button CancelButton;
    public Button ApplyToTypeButton;

    public Text ListLabelPrefab;
    public InputField ListStringFieldPrefab;

    private List<GameObject> addedFields = new List<GameObject>();
    private float period;

    void Start()
    {
        CancelButton.onClick.AddListener(Hide);
        //ApplyButton.onClick.AddListener(Apply);
        OKButton.onClick.AddListener(Accept);
        ApplyToTypeButton.onClick.AddListener(ApplyToType);
    }

    public void Show(IItem item = null)
    {
        Clear();
        CurrentItem = item;
        BlockManager.Cursor.transform.position = item.Position;
        gameObject.SetActive(true);
        Refresh();
    }

    public void Clear()
    {
        CurrentItem = null;
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
        if (CurrentItem == null)
            return;
        if (!string.IsNullOrWhiteSpace(NameField.text))
            CurrentItem.Name = NameField.text;
        Vector3 position = Vector3.zero;
        position.x = float.Parse(PosXField.text);
        position.y = float.Parse(PosYField.text);
        position.z = float.Parse(PosZField.text);
        CurrentItem.Position = position;
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
            CurrentItem.Properties = properties;
    }

    public void ApplyToType()
    {
        if (CurrentItem == null)
            return;
        if (!string.IsNullOrWhiteSpace(NameField.text))
            CurrentItem.Name = NameField.text;
        Vector3 position = Vector3.zero;
        position.x = float.Parse(PosXField.text);
        position.y = float.Parse(PosYField.text);
        position.z = float.Parse(PosZField.text);
        CurrentItem.Position = position;
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
            List<IItem> items = ItemManager.GetItemsOfType(CurrentItem.TypeName);
            foreach(IItem item in items)
                item.Properties = properties;
        }
    }

    public void Accept()
    {
        Apply();
        Hide();
    }

    public void Refresh()
    {
        if (CurrentItem != null)
        {
            Sprite icon = ItemManager.Instance.ItemFactories[ItemManager.ItemFactoryIndex((CurrentItem as AbstractItem).Palette)].IconForType(CurrentItem.TypeName);
            ItemButton.image.sprite = icon;
            ItemButton.GetComponentInChildren<Text>().text = CurrentItem.TypeName;
            NameField.text = CurrentItem.Name;
            PosXField.text = CurrentItem.Position.x.ToString();
            PosYField.text = CurrentItem.Position.y.ToString();
            PosZField.text = CurrentItem.Position.z.ToString();

            foreach(KeyValuePair<string,DefaultValue> property in CurrentItem.DefaultProperties)
            {
                Text label = Instantiate(ListLabelPrefab, PropertyList.transform) as Text ;
                addedFields.Add(label.gameObject);
                label.text = property.Key;

				//TODO support other types of properties
				InputField stringField = Instantiate(ListStringFieldPrefab, PropertyList.transform) as InputField;
				if (property.Value.TypeInfo == typeof(MultilinePlaintext))
				{
					stringField.lineType = InputField.LineType.MultiLineNewline;
				}
				else
				{
					stringField.lineType = InputField.LineType.SingleLine;
				}

				addedFields.Add(stringField.gameObject);
				stringField.placeholder.GetComponent<Text>().text = property.Value.Value;
				SinglePropertyWithDefault holder = stringField.GetComponent<SinglePropertyWithDefault>();
				Debug.Assert(holder != null);
				holder.PropertyName = property.Key;
				holder.PropertyType = property.Value.TypeInfo;
				holder.PropertyDefaultValue = property.Value.Value;

				foreach (KeyValuePair<string, string> setProperty in CurrentItem.Properties)
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
            List<string> types = new List<string>(ItemManager.Instance.ItemFactories[0].ItemTypeNames);
            int index = types.IndexOf(CurrentItem.TypeName);
            if (++index >= types.Count)
                index = 0;
            IItem newItem = ItemManager.CreateItemAt(CurrentItem.Position,"Cat5 Internal", types[index]);
            Clear();
            CurrentItem = newItem;
            Refresh();
            period = 0f;
        }
        else if (prev)
        {
			List<string> types = new List<string>(ItemManager.Instance.ItemFactories[0].ItemTypeNames);
			int index = types.IndexOf(CurrentItem.TypeName);
            if (--index <= 0 )
                index = types.Count-1;
			IItem newItem = ItemManager.CreateItemAt(CurrentItem.Position, "Cat5 Internal", types[index]);
			Clear();
            CurrentItem = newItem;
            Refresh();
            period = 0f;
        }
    }
}

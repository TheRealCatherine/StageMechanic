using CnControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//TODO this used to be 2 separate classes now mushed together, there is lots of duplicate code that can be cleaned up
public class PropertiesDialog : MonoBehaviour {

	public IBlock CurrentBlock;
	public IItem CurrentItem;

	public ISceneObject Target;

	public Button BlockButton;
	public Dpad BlockTypeCycleButtons;
	public GameObject PropertyList;
	public InputField NameField;
	public InputField PosXField;
	public InputField PosYField;
	public InputField PosZField;
	public Button OKButton;
	public Button CancelButton;
	public Button ApplyToTypeButton;
	public Text ListLabelPrefab;
	public InputField ListStringFieldPrefab;

	public AbstractPropertyField[] FieldTypes;

	protected List<GameObject> addedFields = new List<GameObject>();
	protected float period;

	void Start()
	{
		//TODO do this in Unity UI
		CancelButton.onClick.AddListener(Hide);
		OKButton.onClick.AddListener(Accept);
		ApplyToTypeButton.onClick.AddListener(ApplyToType);
	}

	public void Show(ISceneObject target = null)
	{
		Clear();
		Target = target;
		CurrentBlock = Target as IBlock;
		CurrentItem = Target as IItem;
		gameObject.SetActive(true);
		Refresh();
	}

	public void Clear()
	{
		CurrentBlock = null;
		CurrentItem = null;
		Target = null;
		foreach (GameObject obj in addedFields)
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
		if (Target != null)
			ApplyTarget();
		else
			Hide();
	}

	private void ApplyTarget()
	{
		if (Target is null)
			return;
		if (!string.IsNullOrWhiteSpace(NameField.text))
			Target.Name = NameField.text;
		Vector3 position = Vector3.zero;
		position.x = float.Parse(PosXField.text);
		position.y = float.Parse(PosYField.text);
		position.z = float.Parse(PosZField.text);
		Target.Position = position;
		BlockManager.Cursor.transform.position = position;

		Dictionary<string, string> properties = new Dictionary<string, string>();
		foreach (GameObject obj in addedFields)
		{
			IPropertyField prop = obj.GetComponent<IPropertyField>();
			if (prop != null)
			{
				string value = prop.Value;
				if (!string.IsNullOrWhiteSpace(value))
					properties.Add(prop.PropertyName, value);
			}
		}
		if (properties.Count > 0)
			Target.Properties = properties;
	}

	public void ApplyToType()
	{
		if (CurrentBlock != null)
		{
			ApplyBlocksToType();
			Hide();
		}
		else if (CurrentItem != null)
		{
			ApplyItemsToType();
			Hide();
		}
		else
			Hide();

	}

	private void ApplyBlocksToType()
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
			IPropertyField prop = obj.GetComponent<IPropertyField>();
			if (prop != null)
			{
				string value = prop.Value;
				if (!string.IsNullOrWhiteSpace(value))
					properties.Add(prop.PropertyName, value);
			}
		}
		if (properties.Count > 0)
		{
			List<IBlock> blocks = BlockManager.GetBlocksOfType(CurrentBlock.TypeName);
			foreach (IBlock block in blocks)
				block.Properties = properties;
		}
	}

	private void ApplyItemsToType()
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
			IPropertyField prop = obj.GetComponent<IPropertyField>();
			if (prop != null)
			{
				string value = prop.Value;
				if (!string.IsNullOrWhiteSpace(value))
					properties.Add(prop.PropertyName, value);
			}
		}
		if (properties.Count > 0)
		{
			List<IItem> items = ItemManager.GetItemsOfType(CurrentItem.TypeName);
			foreach (IItem item in items)
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
		RefreshBlocks();
		RefreshItems();
	}

	private void RefreshBlocks()
	{
		if (CurrentBlock != null)
		{
			//TODO make this generic
			Sprite icon = BlockManager.Instance.BlockFactories[BlockManager.BlockFactoryIndex((CurrentBlock as AbstractBlock).Palette)].IconForType(CurrentBlock.TypeName);
			BlockButton.image.sprite = icon;
			BlockButton.GetComponentInChildren<Text>().text = CurrentBlock.TypeName;
			NameField.text = CurrentBlock.Name;
			PosXField.text = CurrentBlock.Position.x.ToString();
			PosYField.text = CurrentBlock.Position.y.ToString();
			PosZField.text = CurrentBlock.Position.z.ToString();

			foreach (KeyValuePair<string, DefaultValue> property in CurrentBlock.DefaultProperties.Reverse())
			{
				Text label = Instantiate(ListLabelPrefab, PropertyList.transform) as Text;
				addedFields.Add(label.gameObject);
				label.text = property.Key;

				GameObject fieldPrefab = null;
				foreach (IPropertyField type in FieldTypes)
				{
					if (property.Value.TypeInfo == type.FieldType)
					{
						fieldPrefab = type.GameObject;
						break;
					}
				}
				if (fieldPrefab == null)
					fieldPrefab = ListStringFieldPrefab.gameObject;

				IPropertyField stringField = Instantiate(fieldPrefab, PropertyList.transform).GetComponent<IPropertyField>();
				addedFields.Add(stringField.GameObject);
				stringField.Placeholder = property.Value.Value;
				stringField.PropertyName = property.Key;
				stringField.PropertyDefault = property.Value.Value;

				foreach (KeyValuePair<string, string> setProperty in CurrentBlock.Properties)
				{
					if (setProperty.Key == property.Key)
						stringField.Value = setProperty.Value;
				}
			}
		}
	}
	
	private void RefreshItems()
	{
		if (CurrentItem != null)
		{
			Sprite icon = ItemManager.Instance.ItemFactories[ItemManager.ItemFactoryIndex((CurrentItem as AbstractItem).Palette)].IconForType(CurrentItem.TypeName);
			BlockButton.image.sprite = icon;
			BlockButton.GetComponentInChildren<Text>().text = CurrentItem.TypeName;
			NameField.text = CurrentItem.Name;
			PosXField.text = CurrentItem.Position.x.ToString();
			PosYField.text = CurrentItem.Position.y.ToString();
			PosZField.text = CurrentItem.Position.z.ToString();

			foreach (KeyValuePair<string, DefaultValue> property in CurrentItem.DefaultProperties.Reverse())
			{
				if (string.IsNullOrWhiteSpace(property.Key))
					continue;
				Text label = Instantiate(ListLabelPrefab, PropertyList.transform) as Text;
				addedFields.Add(label.gameObject);
				label.text = property.Key;


				GameObject fieldPrefab = null;
				foreach (IPropertyField type in FieldTypes)
				{
					if (property.Value.TypeInfo == type.FieldType)
					{
						fieldPrefab = type.GameObject;
						break;
					}
				}
				if (fieldPrefab == null)
					fieldPrefab = ListStringFieldPrefab.gameObject;

				IPropertyField stringField = Instantiate(fieldPrefab, PropertyList.transform).GetComponent<IPropertyField>();
				addedFields.Add(stringField.GameObject);
				stringField.Placeholder = property.Value.Value;
				stringField.PropertyName = property.Key;
				stringField.PropertyDefault = property.Value.Value;

				foreach (KeyValuePair<string, string> setProperty in CurrentItem.Properties)
				{
					if (setProperty.Key == property.Key)
						stringField.Value = setProperty.Value;
				}
			}

		}
	}

	void Update()
	{
		period += Time.deltaTime;
		bool next = (CnInputManager.GetAxis("joystick 1 X axis") > 0f && period > InputManager.JoystickThrottleRate);
		bool prev = (CnInputManager.GetAxis("joystick 1 X axis") < 0f && period > InputManager.JoystickThrottleRate);
		if (CurrentBlock != null)
		{
			if (next)
			{
				List<string> types = new List<string>(BlockManager.Instance.BlockFactories[0].BlockTypeNames);
				int index = types.IndexOf(CurrentBlock.TypeName);
				if (++index >= types.Count)
					index = 0;
				IBlock newBlock = BlockManager.CreateBlockAt(CurrentBlock.Position, "Cathy1 Internal", types[index]);
				Clear();
				CurrentBlock = newBlock;
				Refresh();
				period = 0f;
			}
			else if (prev)
			{
				List<string> types = new List<string>(BlockManager.Instance.BlockFactories[0].BlockTypeNames);
				int index = types.IndexOf(CurrentBlock.TypeName);
				if (--index <= 0)
					index = types.Count - 1;
				IBlock newBlock = BlockManager.CreateBlockAt(CurrentBlock.Position, "Cathy1 Internal", types[index]);
				Clear();
				CurrentBlock = newBlock;
				Refresh();
				period = 0f;
			}
		}
		else if(CurrentItem != null)
		{
			if (next)
			{
				List<string> types = new List<string>(ItemManager.Instance.ItemFactories[0].ItemTypeNames);
				int index = types.IndexOf(CurrentItem.TypeName);
				if (++index >= types.Count)
					index = 0;
				IItem newItem = ItemManager.CreateItemAt(CurrentItem.Position, "Cat5 Internal", types[index]);
				Clear();
				CurrentItem = newItem;
				Refresh();
				period = 0f;
			}
			else if (prev)
			{
				List<string> types = new List<string>(ItemManager.Instance.ItemFactories[0].ItemTypeNames);
				int index = types.IndexOf(CurrentItem.TypeName);
				if (--index <= 0)
					index = types.Count - 1;
				IItem newItem = ItemManager.CreateItemAt(CurrentItem.Position, "Cat5 Internal", types[index]);
				Clear();
				CurrentItem = newItem;
				Refresh();
				period = 0f;
			}
		}
	}
}

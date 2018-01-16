using CnControls;
using System.Collections;
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

    public Text ListLabelPrefab;
    public InputField ListStringFieldPrefab;

    private List<GameObject> addedFields = new List<GameObject>();

    void Start()
    {
        CancelButton.onClick.AddListener(Hide);
        ApplyButton.onClick.AddListener(Apply);
        OKButton.onClick.AddListener(Accept);
    }

    public void Show(IBlock block = null)
    {
        Clear();
        CurrentBlock = block;
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
        if (!string.IsNullOrEmpty(NameField.text) && !string.IsNullOrWhiteSpace(NameField.text))
            CurrentBlock.Name = NameField.text;
        Vector3 position = Vector3.zero;
        position.x = float.Parse(PosXField.text);
        position.y = float.Parse(PosYField.text);
        position.z = float.Parse(PosZField.text);
        CurrentBlock.Position = position;
        BlockManager.Cursor.transform.position = position;
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

            foreach(KeyValuePair<string,KeyValuePair<string,string>> property in CurrentBlock.DefaultProperties)
            {
                Text label = Instantiate(ListLabelPrefab, PropertyList.transform) as Text ;
                label.text = property.Key;
                addedFields.Add(label.gameObject);
                InputField stringField = Instantiate(ListStringFieldPrefab, PropertyList.transform) as InputField;
                stringField.placeholder.GetComponent<Text>().text = property.Value.Value;
                addedFields.Add(stringField.gameObject);
            }

        }
    }

    // Update is called once per frame
    void Update () {
	}
}

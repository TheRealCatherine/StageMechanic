/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockTypeScrollBoxPopulator : MonoBehaviour {

	public Button ButtonPrefab;
	public Text FactoryLabelPrefab;

	private List<Button> _buttonsCache = new List<Button>();
	private List<Text> _labelsCache = new List<Text>();

	void Start () {
		Populate();
	}

	void Populate()
	{
		foreach (AbstractBlockFactory factory in BlockManager.Instance.BlockFactories)
		{
			Text label = Instantiate(FactoryLabelPrefab, transform) as Text;
			label.text = factory.DisplayName;
			_labelsCache.Add(label);

			string[] types = factory.BlockTypeNames;
			int count = 0;
			foreach (string name in types)
			{
				Sprite icon = factory.IconForType(name);
				if (icon != null)
				{
					Button newButton = Instantiate(ButtonPrefab, transform) as Button;
					newButton.GetComponentInChildren<Text>().text = name;
					newButton.image.sprite = icon;
					newButton.onClick.AddListener(OnBlockClicked);
					_buttonsCache.Add(newButton);
					++count;
				}
			}
		}
	}

	void OnBlockClicked()
	{
		Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
		foreach(AbstractBlockFactory factory in BlockManager.Instance.BlockFactories)
		{
			if (Array.IndexOf(factory.BlockTypeNames, clickedButton.GetComponentInChildren<Text>().text) > -1)
			{
				BlockManager.CreateBlockAtCursor(factory.Name, clickedButton.GetComponentInChildren<Text>().text);
				return;
			}
		}
	}

	public void Repopulate()
	{
		foreach(Button button in _buttonsCache)
		{
			Destroy(button.gameObject);
		}
		foreach(Text label in _labelsCache)
		{
			Destroy(label.gameObject);
		}
		_buttonsCache.Clear();
		_labelsCache.Clear();
		Populate();
	}
}

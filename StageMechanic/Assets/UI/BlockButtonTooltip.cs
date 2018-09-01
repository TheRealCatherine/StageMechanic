using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	bool _mouseOver = false;

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log(true);
		_mouseOver = true;
		string text = GetComponent<SinglePropertyWithDefault>().PropertyName;
		if (!string.IsNullOrWhiteSpace(text))
		{
			UIManager.Instance.TooltipDisplay.TextElement.text = text;
			Vector3 position = eventData.position;
			position.x -= ((UIManager.Instance.TooltipDisplay.GetComponent<RectTransform>().rect.width / 2f) - 20);
			UIManager.Instance.TooltipDisplay.gameObject.transform.position = position;
			UIManager.Instance.TooltipDisplay.gameObject.SetActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_mouseOver = false;
		UIManager.Instance.TooltipDisplay.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if(_mouseOver)
			UIManager.Instance.TooltipDisplay.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		if (_mouseOver)
			UIManager.Instance.TooltipDisplay.gameObject.SetActive(false);
	}
}

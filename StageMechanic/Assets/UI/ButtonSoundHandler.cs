using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundHandler : MonoBehaviour
{

	public AudioClip ClickSound;
	public AudioClip RolloverSound;

	private Button _button { get { return GetComponent<Button>(); } }
	
	void Start()
	{
		_button.onClick.AddListener(PlayClickSound);
	}

	private void OnMouseEnter()
	{
		PlayRolloverSound();
	}

	public void PlayClickSound()
	{
		AudioEffectsManager.PlaySound(ClickSound);
	}

	public void PlayRolloverSound()
	{
		AudioEffectsManager.PlaySound(RolloverSound);
	}
}
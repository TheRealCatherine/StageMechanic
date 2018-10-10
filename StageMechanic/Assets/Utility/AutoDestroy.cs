using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	public float Seconds = 3;

    void Start()
    {
		DoAutoDestroy(Seconds);
    }


	public void DoAutoDestroy(float seconds)
	{
		StartCoroutine(AutoDestoryHelper(seconds));
	}

	private IEnumerator AutoDestoryHelper(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}
}

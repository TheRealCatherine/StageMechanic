using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInLightVersion : MonoBehaviour
{
    void Start()
    {
		if (GameManager.IsLiteBuild)
			gameObject.SetActive(false);
    }
}

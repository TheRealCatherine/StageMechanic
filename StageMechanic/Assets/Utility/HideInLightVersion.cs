using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInLightVersion : MonoBehaviour
{
    void Update()
    {
		if (GameManager.IsLiteBuild)
			gameObject.SetActive(false);
    }
}

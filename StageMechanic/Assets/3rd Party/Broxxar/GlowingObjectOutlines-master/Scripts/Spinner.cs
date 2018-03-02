using UnityEngine;

public class Spinner : MonoBehaviour
{
	public Vector3 EulersPerSecond;

	void Update()
	{
		transform.Rotate(EulersPerSecond * Time.deltaTime);
    }
}

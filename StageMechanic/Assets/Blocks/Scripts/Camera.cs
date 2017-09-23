using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public GameObject Cursor;

    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - Cursor.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Cursor.transform.position + offset;
    }
}

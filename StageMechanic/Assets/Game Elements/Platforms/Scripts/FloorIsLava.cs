using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIsLava : Platform
{

    public GameObject Stage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Assert(Stage != null);
        BlockManager bm = Stage.GetComponent<BlockManager>();
        Debug.Assert(bm != null);
        if (bm.PlayMode)
            Destroy(collision.gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Assert(Stage != null);
        BlockManager bm = Stage.GetComponent<BlockManager>();
        Debug.Assert(bm != null);
        if (bm.PlayMode)
            Destroy(collision.gameObject);

    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Assert(Stage != null);
        BlockManager bm = Stage.GetComponent<BlockManager>();
        Debug.Assert(bm != null);
        if (bm.PlayMode)
            Destroy(collision.gameObject);
    }

    void OnTriggerStay(Collider collision)
    {
        Debug.Assert(Stage != null);
        BlockManager bm = Stage.GetComponent<BlockManager>();
        Debug.Assert(bm != null);
        if (bm.PlayMode)
            Destroy(collision.gameObject);
    }
}

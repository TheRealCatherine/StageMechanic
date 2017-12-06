using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClimbBehavior : MonoBehaviour {

    /// <summary>
    /// How many Unity distance units the object should move per frame.
    /// </summary>
    public float ClimbSpeed { get; set; } = 0.05f;

    public float Granularity { get; set; } = 1.0f;

    public GameObject Character { get; set; }

    public enum State
    {
        None = 0,
        Aproach,
        Climb,
        Center
    }

    public State CurrentMoveState { get; set; } = State.None;

    /// <summary>
    /// Synonym for transform.localposition
    /// </summary>
    public Vector3 CurrentLocation
    {
        get
        {
            return transform.localPosition;
        }
        set
        {
            transform.localPosition = value;
        }
    }

    /// <summary>
    /// Location the object should climb to. If this is
    /// the same as CurrentLocation then no climbing will
    /// happen this frame.
    /// </summary>
    public Vector3 DesiredLocation { get; set; }

    /// <summary>
    /// Immediately apply the relative movmement offset
    /// </summary>
    /// <param name="offset"></param>
    public void ForceMove(Vector3 offset)
    {
        if (offset != Vector3.zero)
            CurrentLocation += offset;
    }

    /// <summary>
    /// Apply the movement offset taking into account ClimbSpeed
    /// </summary>
    /// <param name="offset"></param>
    public void Climb(Vector3 offset)
    {
        if (CurrentMoveState == State.None)
        {
            DesiredLocation = CurrentLocation + offset;
            CurrentMoveState = State.Climb;
        }
    }

    public void ClimbUp()
    {
        Climb(new Vector3(0f, Granularity, 0f));
    }

    public void ClimbDown()
    {
        Climb(new Vector3(0f, -Granularity, 0f));
    }

    public void ClimbLeft()
    {
        Climb(new Vector3(-Granularity, 0f, 0f));
    }

    public void ClimbRight()
    {
        Climb(new Vector3(Granularity, 0f, 0f));
    }

    public void ClimbForward()
    {
        Climb(new Vector3(0f, 0f, -Granularity));
    }

    public void ClimbBack()
    {
        Climb(new Vector3(0f, 0f, Granularity));
    }

    private void Start()
    {
        DesiredLocation = CurrentLocation;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentMoveState != State.None && DesiredLocation != CurrentLocation)
        {
            if(CurrentMoveState == State.Aproach)
            {
                Character.GetComponent<Animator>().SetBool("walking", true);
                Character.GetComponent<Animator>().SetBool("climbing", false);
                Vector3 offset = DesiredLocation - CurrentLocation;
                offset.Normalize();
                ForceMove(new Vector3(offset.x * ClimbSpeed, offset.y * ClimbSpeed, offset.z * ClimbSpeed));
                if (CurrentLocation == DesiredLocation)
                    CurrentMoveState = State.None;
            }
            else if(CurrentMoveState == State.Climb)
            {
                Character.GetComponent<Animator>().SetBool("walking", false);
                Character.GetComponent<Animator>().SetBool("climbing", true);
                ForceMove(Vector3.up);
                DesiredLocation += Vector3.up;
                CurrentMoveState = State.Aproach;
            }
        }
        else
        {
            CurrentMoveState = State.None;
            Character.GetComponent<Animator>().SetBool("climbing", false);
            Character.GetComponent<Animator>().SetBool("walking", false);
        }
    }
}

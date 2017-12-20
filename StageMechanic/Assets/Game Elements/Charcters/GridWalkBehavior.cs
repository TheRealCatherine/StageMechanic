using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWalkBehavior : MonoBehaviour
{
    /// <summary>
    /// How many Unity distance units the object should move per frame.
    /// </summary>
    public float WalkSpeed { get; set; } = 0.05f;

    public float Granularity { get; set; } = 1.0f;

    public float ClimbSpeed { get; set; } = 0.05f;

    public enum State
    {
        None = 0,
        Aproach,
        Climb,
        Center
    }

    public State CurrentMoveState { get; set; } = State.None;

    public GameObject Character { get; set; }

    public bool IsWalking { get; set; } = false;

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
    /// Location the object should walk to. If this is
    /// the same as CurrentLocation then no walking will
    /// happen this frame.
    /// </summary>
    public Vector3 DesiredLocation { get; set; }

    /// <summary>
    /// Immediately apply the relative movmement offset
    /// </summary>
    /// <param name="offset"></param>
    public void ForceMove(Vector3 offset)
    {
        if(offset != Vector3.zero)
            CurrentLocation += offset;
    }

    public void Teleport(Vector3 location)
    {
        CurrentLocation = location;
    }

    /// <summary>
    /// Apply the movement offset taking into account WalkSpeed
    /// </summary>
    /// <param name="offset"></param>
    public void Move(Vector3 offset)
    {
        DesiredLocation = CurrentLocation + offset;
        IsWalking = true;
    }

    public void MoveUp()
    {
        Move(new Vector3(0f,Granularity,0f));
    }

    public void MoveDown()
    {
        Move(new Vector3(0f, -Granularity, 0f));
    }

    public void MoveLeft()
    {
        Move(new Vector3(-Granularity, 0f, 0f));
    }

    public void MoveRight()
    {
        Move(new Vector3(Granularity, 0f, 0f));
    }

    public void MoveForward()
    {
        Move(new Vector3(0f, 0f, -Granularity));
    }

    public void MoveBack()
    {
        Move(new Vector3(0f, 0f, Granularity));
    }

    /// <summary>
    /// Apply the movement offset taking into account ClimbSpeed
    /// </summary>
    /// <param name="offset"></param>
    public void Climb(Vector3 offset)
    {
        //if (CurrentMoveState == State.None)
        //{
            DesiredLocation = CurrentLocation + offset;
            //CurrentMoveState = State.Climb;
       // }
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

    internal IEnumerator MoveToLocation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            Teleport(DesiredLocation);
        }
    }

    private void Start()
    {
        DesiredLocation = CurrentLocation;
        StartCoroutine(MoveToLocation());
    }

    // Update is called once per frame
    void Update()
    {
      /*  if(IsWalking && DesiredLocation != CurrentLocation)
        {
            Character.GetComponent<Animator>().SetBool("walking", true);
            Vector3 offset = DesiredLocation - CurrentLocation;
            offset.Normalize();
            ForceMove(new Vector3(offset.x*WalkSpeed, offset.y*WalkSpeed,offset.z*WalkSpeed));
        }
        else
        {
            IsWalking = false;
            Character.GetComponent<Animator>().SetBool("walking", false);
        }*/
    }
}

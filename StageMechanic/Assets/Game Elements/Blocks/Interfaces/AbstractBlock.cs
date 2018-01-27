/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public abstract class AbstractBlock : MonoBehaviour, IBlock
{


    /// <summary>
    /// Synonym/passthrough for GameObject.name
    /// See <see cref="IBlock.Name"/>
    /// See also <seealso cref="UnityEngine.GameObject"/>
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    /// <summary>
    /// See <see cref="IBlock.TypeName"/>
    /// </summary>
    public abstract string TypeName
    {
        get;
        set;
    }

    /// <summary>
    /// Synonym/passthrough for GameObject.transform.position
    /// See <see cref="IBlock.Position"/>
    /// See also <seealso cref="UnityEngine.Transform.position"/>
    /// See also <seealso cref="Vector3"/>
    /// </summary>
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    /// <summary>
    /// Synonym/passthrough for GameObject.transform.rotation
    /// See <see cref="IBlock.Rotation"/>
    /// See also <seealso cref="UnityEngine.Transform.rotation"/>
    /// See also <seealso cref="Quaternion"/>
    /// </summary>
    public Quaternion Rotation
    {
        get
        {
            return transform.rotation;
        }
        set
        {
            transform.rotation = value;
        }
    }

    /// <summary>
    /// Synonym/passthrough for GameObject.gameObject
    /// See <see cref="IBlock.GameObject"/>
    /// See also <seealso cref="UnityEngine.GameObject.gameObject"/>
    /// </summary>
    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }
    }

    /// <summary>
    /// List of items associated with this block. These will moved to be child elements of the block.
    /// This property may be null, so check this when calling.
    /// </summary>
    public virtual List<GameObject> Items { get; set; }

    public virtual List<IEvent> Events { get; set; }

    /// <summary>
    /// Uses GameObject.transform.parent internally. This method is
    /// virtual because some blocks may want to report no parent
    /// if their actual parent is a certain type, or some other
    /// condition in which there actually is a parent.
    /// See <see cref="IBlock.Parent"/>
    /// See also <seealso cref="UnityEngine.GameObject"/>
    /// See also <seealso cref="Transform.parent"/>
    /// </summary>
    public virtual GameObject Parent
    {
        get
        {
            Debug.Assert(transform != null);
            if (transform.parent == null)
                return null;
            return transform.parent.gameObject;
        }

        set
        {
            if (value == null)
                transform.SetParent(null);
            else
                transform.SetParent(value.transform, true);
        }
    }

    /// <summary>
    /// This implementation returns all AbstractBlock-derived
    /// children of an instance. It is marked virtual because it
    /// is expected that other implementations may include other
    /// types of children. Note that Items, while technically children
    /// of blocks in most implementations, have their own properties.
    /// </summary>
    public virtual List<GameObject> Children
    {
        get
        {
            List<GameObject> chillins = new List<GameObject>();
            foreach (GameObject kiddo in transform)
            {
                AbstractBlock jennyFromTheBlock = kiddo.GetComponent<AbstractBlock>();
                if (jennyFromTheBlock != null)
                    chillins.Add(kiddo);
            }
            return chillins;
        }
        //TODO remove items not in list or clear list first
        set
        {
            foreach (GameObject rugrat in value)
            {
                //Cathy1 blocks can currently only have Cathy1 blocks as children
                //Items are also children technically, but should be accessd via Item or Items
                AbstractBlock blockPartyLikeIts1999 = rugrat.GetComponent<AbstractBlock>();
                if (blockPartyLikeIts1999 != null)
                    blockPartyLikeIts1999.Parent = GameObject;
            }
        }
    }

    public virtual Dictionary<string, KeyValuePair<Type, string>> DefaultProperties
    {
        get
        {
            Dictionary<string, KeyValuePair<Type, string>> ret = new Dictionary<string, KeyValuePair<Type, string>>();
            ret.Add("Rotation", new KeyValuePair<Type, string>(typeof(Quaternion), Quaternion.identity.ToString()));
            ret.Add("Fixed Rotation", new KeyValuePair<Type, string>(typeof(bool), "false"));
            ret.Add("Weight Factor", new KeyValuePair<Type, string>(typeof(float), "1.0"));
            ret.Add("Gravity Factor", new KeyValuePair<Type, string>(typeof(float), "1.0"));
            ret.Add("Block Group", new KeyValuePair<Type, string>(typeof(int), "-1"));
            return ret;
        }
    }

    public virtual Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (Rotation != Quaternion.identity)
                ret.Add("Rotation", Rotation.ToString());
            if (IsFixedRotation)
                ret.Add("Fixed Rotation", IsFixedRotation.ToString());
            if (WeightFactor != 1.0f)
                ret.Add("Weight Factor", WeightFactor.ToString());
            if (GravityFactor != 1.0f)
                ret.Add("Gravity Factor", GravityFactor.ToString());
            if (BlockManager.BlockGroupNumber(this) != -1)
                ret.Add("Block Group", BlockManager.BlockGroupNumber(this).ToString());
            return ret;
        }
        set
        {
            //    Rotation = Utility.StringToQuaternion(value["Rotation"]);*/
            if (value.ContainsKey("Fixed Rotation"))
                IsFixedRotation = Convert.ToBoolean(value["Fixed Rotation"]);
            if (value.ContainsKey("Weight Factor"))
                WeightFactor = (float)Convert.ToDouble(value["Weight Factor"]);
            if (value.ContainsKey("Gravity Factor"))
                GravityFactor = (float)Convert.ToDouble(value["Gravity Factor"]);
            if (value.ContainsKey("Block Group"))
                BlockManager.AddBlockToGroup(this, Convert.ToInt32(value["Block Group"]));
        }
    }

    /// <summary>
    /// See <see cref="IBlock.IsFixedRotation"/>
    /// </summary>
    public bool IsFixedRotation { get; set; } = false;

    /// <summary>
    /// See <see cref="IBlock.WeightFactor"/>
    /// </summary>
    public float WeightFactor { get; set; } = 1.0f;

    /// <summary>
    /// See <see cref="IBlock.GravityFactor"/>
    /// </summary>
    public float GravityFactor { get; set; } = 1.0f;

    [SerializeField]
    private BlockMotionState _motionState = BlockMotionState.Unknown;
    public BlockMotionState MotionState
    {
        get
        {
            return _motionState;
        }
        set
        {
            _motionState = value;
        }
    }

    private string _motionStateName;
    public string MotionStateName
    {
        get
        {
            if (MotionState != BlockMotionState.Extended)
                return MotionState.ToString();
            else
                return _motionStateName;
        }
        set
        {
            BlockMotionState newState = BlockMotionState.Unknown;
            if (Enum.TryParse<BlockMotionState>(value, out newState))
            {
                MotionState = newState;
            }
            else
            {
                MotionState = BlockMotionState.Extended;
                _motionStateName = value;
            }
        }
    }

    /// <summary>
    /// This will be set to true only if the state is Falling in this implementation.
    /// </summary>
    public virtual bool IsGrounded
    {
        get
        {
            return MotionState != BlockMotionState.Falling;
        }
    }

    virtual public BlockJsonDelegate GetJsonDelegate()
    {
        return new BlockJsonDelegate(this);
    }

    public virtual bool CanBeMoved(Vector3 direction, int distance = 1)
    {
        if (WeightFactor == 0)
            return false;

        IBlock neighbor = BlockManager.GetBlockAt(Position + direction);
        if (neighbor != null)
            return neighbor.CanBeMoved(direction, distance);
        return true;
    }

    public virtual float MoveWeight(Vector3 direction, int distance = 1)
    {
        if (!CanBeMoved(direction, distance))
            return 0;
        IBlock neighbor = BlockManager.GetBlockAt(Position + direction);
        float weight = WeightFactor;
        if (neighbor != null)
        {
            if (!neighbor.CanBeMoved(direction, distance))
                return 0;
            weight = Math.Max(weight, neighbor.WeightFactor);
        }
        return weight;
    }

    public virtual bool Move(Vector3 direction, int distance = 1)
    {
        if (!BlockManager.CanBeMoved(this, direction, distance))
            return false;
        MotionState = BlockMotionState.Moving;
        IBlock neighbor = BlockManager.GetBlockAt(Position + direction);
        if (neighbor != null)
            BlockManager.Move(neighbor, direction, distance);
        StartCoroutine(AnimateMove(Position, Position + direction, 0.2f * MoveWeight(direction, distance)));
        return true;
    }

    internal IEnumerator AnimateMove(Vector3 origin, Vector3 target, float duration)
    {
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            transform.position = Vector3.Lerp(origin, target, percent);

            yield return null;
        }
        MotionState = BlockMotionState.Unknown;
        UpdateNeighborsCache();
        SetStateBySupport();
    }

    /// <summary>
    /// Sets the name to a random GUID
    /// Called when this object is created in the scene. If overriding
    /// you may wish to call this base class in order to have the name
    /// set to a random GUID.
    /// </summary>
    public virtual void Awake()
    {
        name = System.Guid.NewGuid().ToString();
        PhysicsEnabled = false;
        while (blocksAbove.Count < 5)
            blocksAbove.Add(null);
        while (blocksBelow.Count < 5)
            blocksBelow.Add(null);
    }

    internal virtual void Start()
    {
        /*SphereCollider topSphere = gameObject.AddComponent<SphereCollider>();
        topSphere.isTrigger = true;
        topSphere.transform.position = new Vector3(0, 0.8f, 0f);
        topSphere.radius = 0.55f;

        SphereCollider bottomSphere = gameObject.AddComponent<SphereCollider>();
        bottomSphere.isTrigger = true;
        bottomSphere.transform.position = new Vector3(0, 0.8f, 0f);
        bottomSphere.radius = 0.55f;*/

        //TODO: don't make assumptions about the floor
        if (Position.y == 1f)
        {
            MotionState = BlockMotionState.Grounded;
            PhysicsEnabled = false;
        }
        UpdateNeighborsCache();
    }


    private void OnDisable()
    {
        if (BlockManager.Instance.State == BlockManager.BlockManagerState.PlayMode)
            UpdateAllNeighborsCaches();
    }

    private void OnDestroy()
    {
    }

    internal List<IBlock> blocksBelow = new List<IBlock>(5);
    internal List<IBlock> blocksAbove = new List<IBlock>(5);
    internal const int DOWN = 0;
    internal const int UP = DOWN;
    internal const int FORWARD = 1;
    internal const int BACK = 2;
    internal const int LEFT = 3;
    internal const int RIGHT = 4;
#if UNITY_EDITOR
    public int BelowCount;
    public int AboveCount;
    public string BlockDown;
    public string BlockDownLeft;
    public string BlockDownRight;
    public string BlockDownForward;
    public string BlockDownBack;
#endif

    public void UpdateNeighborsCache()
    {
        Profiler.BeginSample("Updating block neighbor cache");
        HardUpdateBlocksAbove();
        HardUpdateBlocksBelow();
        UpdateNeighborCounts();
        Profiler.EndSample();
    }

    public void UpdateAllNeighborsCaches()
    {
        Profiler.BeginSample("Yelling at neighbors");
        foreach (IBlock block in blocksBelow)
        {
            (block as AbstractBlock)?.UpdateNeighborsCache();
        }
        foreach (IBlock block in blocksAbove)
        {
            (block as AbstractBlock)?.UpdateNeighborsCache();
        }
        Profiler.EndSample();
    }

    protected void HardUpdateBlocksAbove()
    {
        blocksAbove[UP] = BlockManager.GetBlockNear(Position + Vector3.up);
        blocksAbove[FORWARD] = BlockManager.GetBlockNear(Position + Vector3.up + new Vector3(0f,0f,0.7f));
        blocksAbove[BACK] = BlockManager.GetBlockNear(Position + Vector3.up + new Vector3(0f, 0f, -0.7f));
        blocksAbove[LEFT] = BlockManager.GetBlockNear(Position + Vector3.up + new Vector3(-0.7f, 0f, 0f));
        blocksAbove[RIGHT] = BlockManager.GetBlockNear(Position + Vector3.up + new Vector3(0.7f, 0f, 0f));
    }

    protected void HardUpdateBlocksBelow()
    {
        blocksBelow[DOWN] = BlockManager.GetBlockNear(Position + Vector3.down);
        blocksBelow[FORWARD] = BlockManager.GetBlockNear(Position + Vector3.down + new Vector3(0f, 0f, 0.7f));
        blocksBelow[BACK] = BlockManager.GetBlockNear(Position + Vector3.down + new Vector3(0f, 0f, -0.7f));
        blocksBelow[LEFT] = BlockManager.GetBlockNear(Position + Vector3.down + new Vector3(-0.7f, 0f, 0f));
        blocksBelow[RIGHT] = BlockManager.GetBlockNear(Position + Vector3.down + new Vector3(0.7f, 0f, 0f));
#if UNITY_EDITOR
        BlockDown = blocksBelow[DOWN]?.Name;
        BlockDownBack = blocksBelow[BACK]?.Name;
        BlockDownForward = blocksBelow[FORWARD]?.Name;
        BlockDownLeft = blocksBelow[LEFT]?.Name;
        BlockDownRight = blocksBelow[RIGHT]?.Name;
#endif
    }

    public void UpdateNeighborCounts()
    {
        BelowCount = 0;
        AboveCount = 0;
        foreach (IBlock support in blocksBelow)
        {
            if (support != null)
                ++BelowCount;
        }
        foreach (IBlock support in blocksAbove)
        {
            if (support != null)
                ++AboveCount;
        }
        if(Selection.activeGameObject == this.gameObject)
            Debug.Log("Updated counts: v:" + BelowCount+" ^:" + AboveCount);
    }

    public int NumberOfActualSupporters()
    {
        int ret = 0;
        foreach (IBlock support in blocksBelow)
        {
            AbstractBlock block = support as AbstractBlock;
            if (support != null && (support.MotionState == BlockMotionState.Edged || support.MotionState == BlockMotionState.Grounded))
                ++ret;
        }
        return ret;
    }

    [SerializeField]
    bool _physicsEnabled = false;
    public virtual bool PhysicsEnabled
    {
        get
        {
            return _physicsEnabled;
        }
        set
        {
            if (value == _physicsEnabled)
                return;
            Profiler.BeginSample("Changing physics state");
            if (value)
            {
                GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
                GetComponent<Rigidbody>().useGravity = true;
                _physicsEnabled = true;
            }
            else
            {
                transform.position = Utility.Round(Position, 0);
                transform.rotation = Quaternion.identity;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().useGravity = false;
                _physicsEnabled = false;

                //Probably not needed
               /*UpdateAllNeighborsCaches();
                foreach (IBlock edge in blocksAbove)
                {
                    (edge as AbstractBlock)?.SetStateBySupport();
                }
                UpdateNeighborsCache();*/
            }
            Profiler.EndSample();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        IBlock otherBlock = collision.gameObject.GetComponent<IBlock>();
        if (otherBlock == null)
        {
            Profiler.BeginSample("Non-block collision");
            if (collision.gameObject == BlockManager.ActiveFloor && IsGrounded)
            {
                Profiler.BeginSample("Floor collision");
                PhysicsEnabled = false;
                MotionState = BlockMotionState.Grounded;
                Profiler.EndSample(); //Floor collision
            }
            else
            {
                IPlayerCharacter player = collision.gameObject.GetComponent<IPlayerCharacter>();
                if(player != null)
                {
                    Profiler.BeginSample("Player collision");
                    OnPlayerMovement(player, PlayerMovementEvent.EventType.Enter);
                    Profiler.EndSample(); //Player collision
                }
            }

            Profiler.EndSample(); //Non-block collision
            return;
        }
        else
        {
            Profiler.BeginSample("Block collision");
            if ((otherBlock.Position.y > Position.y + 0.01) || (otherBlock.Position.y < Position.y - 0.01)
                && ((otherBlock.Position.x == Position.x && otherBlock.Position.z == Position.z)
                || (otherBlock.Position.x != Position.x ^ otherBlock.Position.z != Position.z)))
            {
                if (MotionState != BlockMotionState.Falling)
                {
                    
                    if (otherBlock.Position.y > Position.y)
                    {
                        (otherBlock as AbstractBlock).UpdateNeighborsCache();
                        (otherBlock as AbstractBlock).SetStateBySupport();
                        if ((otherBlock.Position.x == Position.x && otherBlock.Position.z == Position.z)
                            || (otherBlock.Position.x != Position.x ^ otherBlock.Position.z != Position.z))
                            otherBlock.PhysicsEnabled = false;
                    }
                    else
                        PhysicsEnabled = false;
                }
            }
            UpdateNeighborsCache();
            Profiler.EndSample();
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        Profiler.BeginSample("Collision exit");
        UpdateNeighborsCache();
        IPlayerCharacter player = collision.gameObject.GetComponent<IPlayerCharacter>();
        if (player != null)
        {
            Profiler.BeginSample("Player collision");
            OnPlayerMovement(player, PlayerMovementEvent.EventType.Leave);
            Profiler.EndSample(); //Player collision
        }
        Profiler.EndSample(); //Collision exit
    }


    internal IEnumerator EstablishCurrentState()
    {
        if (MotionState == BlockMotionState.Moving || MotionState == BlockMotionState.Sliding)
            yield break;
        BlockMotionState oldState = MotionState;
        UpdateNeighborsCache();
        MotionState = BlockMotionState.Unknown;

        //If there is nothing below us we can dip out quick
        if (BelowCount==0 || NumberOfActualSupporters()==0)
        {
            if (oldState == BlockMotionState.Hovering)
                MotionState = BlockMotionState.Falling;
            else if (oldState != BlockMotionState.Falling)
                MotionState = BlockMotionState.Hovering;
            yield break;
        }

        //If we are resting on the floor
        if(Position.y == 1 && oldState != BlockMotionState.Falling)
        {
            MotionState = BlockMotionState.Grounded;
            yield break;
        }

        if (blocksBelow[DOWN] != null && (blocksBelow[DOWN].MotionState == BlockMotionState.Edged || blocksBelow[DOWN].MotionState == BlockMotionState.Grounded))
        {
            MotionState = BlockMotionState.Grounded;
            yield break;
        }
        else
        {
            MotionState = BlockMotionState.Edged;
            yield break;
        }
    }

    bool _startHoverOnPlay = false;
    internal void SetStateBySupport()
    {
        if (MotionState == BlockMotionState.Moving || MotionState == BlockMotionState.Sliding)
            return;

        Profiler.BeginSample("SetStateBySupport");

        BlockMotionState oldState = MotionState;

        if (MotionState != BlockMotionState.Falling)
        {
            Profiler.BeginSample("Not Falling");
            if (Position.y == 1)
            {
                PhysicsEnabled = false;
                MotionState = BlockMotionState.Grounded;
                return;
            }
            else
            {
                MotionState = BlockMotionState.Hovering;
            }
            Profiler.EndSample();
        }
        else
            PhysicsEnabled = true;

        Profiler.BeginSample("testing support");
        if (blocksBelow[DOWN] != null && (blocksBelow[DOWN].MotionState == BlockMotionState.Grounded || blocksBelow[DOWN].MotionState == BlockMotionState.Edged))
        {
            MotionState = BlockMotionState.Grounded;
        }
        else
        {
            Profiler.BeginSample("edging");
            foreach (IBlock edge in blocksBelow)
            {
                if (edge != null && (edge.MotionState == BlockMotionState.Grounded || edge.MotionState == BlockMotionState.Edged))
                {
                    if (edge.Position.y > Position.y - 1.1f)
                    {
                        MotionState = BlockMotionState.Edged;
                        break;
                    }
                }
            }
            Profiler.EndSample();
        }
        Profiler.EndSample();

        if (oldState != MotionState)
        {
            Profiler.BeginSample("changed state");
            PhysicsEnabled = !IsGrounded;
            if (MotionState == BlockMotionState.Hovering)
            {
                if (BlockManager.Instance.State == BlockManager.BlockManagerState.PlayMode)
                    StartCoroutine(DoHoverAnimation());
                else
                    _startHoverOnPlay = true;
            }
            else
                _startHoverOnPlay = false;
            Profiler.EndSample();
        }
        else if (_startHoverOnPlay && MotionState == BlockMotionState.Hovering && BlockManager.Instance.State == BlockManager.BlockManagerState.PlayMode)
        {
            Profiler.BeginSample("start the hover");
            StartCoroutine(DoHoverAnimation());
            _startHoverOnPlay = false;
            Profiler.EndSample();
        }
        Profiler.EndSample();
    }

    bool _secondLoop = false;
    public IEnumerator DoHoverAnimation()
    {
        yield return new WaitForEndOfFrame();
        if (!_secondLoop)
        {
            UpdateNeighborsCache();
            SetStateBySupport();
            _secondLoop = true;
        }
        else
            _secondLoop = false;
        yield return new WaitForEndOfFrame();
        GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePosition);
        transform.Rotate(0f, 0f, .5f);
        if (MotionState == BlockMotionState.Grounded || MotionState == BlockMotionState.Edged)
            PhysicsEnabled = false;
        yield return new WaitForSeconds(0.1f);
        if (MotionState == BlockMotionState.Grounded || MotionState == BlockMotionState.Edged)
            PhysicsEnabled = false;
        transform.Rotate(0f, 0f, -.10f);
        yield return new WaitForSeconds(0.1f);
        if (MotionState == BlockMotionState.Grounded || MotionState == BlockMotionState.Edged)
            PhysicsEnabled = false;
        transform.Rotate(0f, 0f, .10f);
        yield return new WaitForSeconds(0.1f);
        if (MotionState == BlockMotionState.Grounded || MotionState == BlockMotionState.Edged)
            PhysicsEnabled = false;
        transform.Rotate(0f, 0f, -.5f);
        yield return new WaitForSeconds(0.1f);
        if (MotionState == BlockMotionState.Grounded || MotionState == BlockMotionState.Edged)
            PhysicsEnabled = false;
        transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(0.6f);
        if (MotionState == BlockMotionState.Grounded || MotionState == BlockMotionState.Edged)
            PhysicsEnabled = false;
        MotionState = BlockMotionState.Falling;
    }

    internal virtual void Update()
    {
        //I think we can get away with doing this in update which seems to help performance but
        //it needs to be tested still, maybe move to FixedUpdate
        SetStateBySupport();
    }

    internal virtual void FixedUpdate()
    {
        //SetStateBySupport();
    }


    public virtual void OnPlayerMovement(IPlayerCharacter player, PlayerMovementEvent.EventType type)
    {
        Debug.Assert(player != null);
        PlayerMovementEvent ev = new PlayerMovementEvent();
        ev.Type = type;
        ev.Player = player;
        if (player.Position.y > Position.y && player.Position.x == Position.x && player.Position.z == Position.z)
            ev.Location = PlayerMovementEvent.EventLocation.Top;
        else if(player.Position.y < Position.y && player.Position.x == Position.x && player.Position.z == Position.z)
            ev.Location = PlayerMovementEvent.EventLocation.Bottom;
        else if(player.Position.x == Position.x ^ player.Position.z == Position.z && player.Position.y == Position.y)
            ev.Location = PlayerMovementEvent.EventLocation.Side;
        else
            ev.Location = PlayerMovementEvent.EventLocation.None;
        OnPlayerMovement(ev);
    }

    protected virtual void OnPlayerMovement(PlayerMovementEvent ev)
    {
        Debug.Assert(ev != null);
        switch(ev.Type)
        {
            case PlayerMovementEvent.EventType.Stay:
                OnPlayerStay(ev);
                break;
            case PlayerMovementEvent.EventType.Enter:
                OnPlayerEnter(ev);
                break;
            case PlayerMovementEvent.EventType.Leave:
                OnPlayerLeave(ev);
                break;
            default:
            case PlayerMovementEvent.EventType.None:
                OnPlayerUnknownMotion(ev);
                break;
        }
    }

    protected virtual void OnPlayerEnter(PlayerMovementEvent ev) {}
    protected virtual void OnPlayerLeave(PlayerMovementEvent ev) {}
    protected virtual void OnPlayerStay(PlayerMovementEvent ev) {}
    protected virtual void OnPlayerUnknownMotion(PlayerMovementEvent ev) {}

    float period = 0.0f;
    const float throttle = 0.1f;
    internal virtual void OnMouseOver()
    {
        period += Time.deltaTime;
        if (period < throttle)
            return;
        if (Input.GetMouseButton(0))
        {
            period = 0;
            if (!BlockManager.PlayMode && !UIManager.IsAnyInputDialogOpen)
            {
                if (BlockManager.Instance.ActiveBlock == this)
                    UIManager.ShowBlockEditDialog(this);
                BlockManager.Cursor.transform.position = Position;
            }
#if UNITY_EDITOR
            Selection.activeGameObject = gameObject;
#endif
        }
    }
}

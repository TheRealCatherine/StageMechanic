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

    const float PUSH_PULL_MOVE_TIME_BASE = 0.25f;

    public ParticleSystem EdgeEffect;
    public float EdgeEffectScale = 1f;
    public float EdgeEffectDuration = 0.1f;

    #region Interface property implementations
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


    public virtual IHierarchical Parent
    {
        get
        {
            return null;
        }

        set
        {
            throw new HierarchyException("Blocks do not yet support Hierarchies");
        }
    }


    public virtual IHierarchical[] Children
    {
        get
        {
            return null;
        }
        set
        {
            throw new HierarchyException("Blocks do not yet support Hierarchies");
        }
    }

    public virtual Dictionary<string, DefaultValue> DefaultProperties
    {
        get
        {
            Dictionary<string, DefaultValue> ret = new Dictionary<string, DefaultValue>();
            ret.Add("Motion State",     new DefaultValue { TypeInfo = typeof(string),       Value = "Unknown" });
            ret.Add("Rotation",         new DefaultValue { TypeInfo = typeof(Quaternion),   Value = Quaternion.identity.ToString() });
            ret.Add("Fixed Rotation",   new DefaultValue { TypeInfo = typeof(bool),         Value = "False" });
            ret.Add("Weight Factor",    new DefaultValue { TypeInfo = typeof(float),        Value = "1.0" });
            ret.Add("Gravity Factor",   new DefaultValue { TypeInfo = typeof(float),        Value = "1.0" });
            ret.Add("Block Group",      new DefaultValue { TypeInfo = typeof(int),          Value = "-1" });
            return ret;
        }
    }

    public virtual Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (MotionState != BlockMotionState.Unknown)
                ret.Add("Motion State", MotionStateName);
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
            if (value.ContainsKey("Motion State"))
                MotionStateName = value["Motion State"];
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

    public BlockJsonDelegate GetJsonDelegate()
    {
        return new BlockJsonDelegate(this);
    }

    public BlockBinaryDelegate GetBinaryDelegate()
    {
        return new BlockBinaryDelegate(this);
    }
    #endregion

    #region constructors/destructors
    /// <summary>
    /// Sets the name to a random GUID
    /// Called when this object is created in the scene. If overriding
    /// you may wish to call this base class in order to have the name
    /// set to a random GUID.
    /// </summary>
    public virtual void Awake()
    {
        name = System.Guid.NewGuid().ToString();
        GravityEnabled = false;
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
            GravityEnabled = false;
        }
        UpdateNeighborsCache();
    }


    private void OnDisable()
    {
        if (BlockManager.PlayMode)
            UpdateAllNeighborsCaches();
    }

    private void OnDestroy()
    {
    }
    #endregion

    #region update functions
    internal virtual void Update()
    {
        //I think we can get away with doing this in update which seems to help performance but
        //it needs to be tested still, maybe move to FixedUpdate
        SetGravityEnabledByMotionState();
    }
    #endregion

    #region push/pull movement
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

    public virtual bool Move(Vector3 direction, int distance = 1, bool push = true)
    {
        if (!BlockManager.CanBeMoved(this, direction, distance))
            return false;
        MotionState = BlockMotionState.Moving;
        IBlock neighbor = BlockManager.GetBlockAt(Position + direction);
        if (neighbor != null)
            BlockManager.Move(neighbor, direction, distance);
        StartCoroutine(AnimateMove(Position, Position + direction, PUSH_PULL_MOVE_TIME_BASE * MoveWeight(direction, distance), push));
        return true;
    }

    internal IEnumerator AnimateMove(Vector3 origin, Vector3 target, float duration, bool push)
    {
        float journey = 0f;
        GravityEnabled = false;
        GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY);

        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(origin, target, percent));

            yield return null;
        }
        _gravityDirty = true;
        MotionState = BlockMotionState.Grounded;
        UpdateNeighborsCache();
        SetGravityEnabledByMotionState();
        //TODO NotLikeThiiiiiiisssssss do it in the ice block class handling blocks on top
        if (push && blocksBelow[DOWN] != null && (blocksBelow[DOWN] as Cathy1Block).Type == Cathy1Block.BlockType.Ice)
        {
            if (BlockManager.GetBlockNear(Position + (target - origin)) == null)
                Move(target - origin);
        }
    }
    #endregion

    #region neighbor caching
    internal List<IBlock> blocksBelow = new List<IBlock>(5);
    internal List<IBlock> blocksAbove = new List<IBlock>(5);
    internal const int DOWN = 0;
    internal const int UP = DOWN;
    internal const int FORWARD = 1;
    internal const int BACK = 2;
    internal const int LEFT = 3;
    internal const int RIGHT = 4;
    public int BelowCount;
    public int AboveCount;
#if UNITY_EDITOR
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
        blocksAbove[UP] = BlockManager.GetBlockAt(Position + Vector3.up);
        blocksAbove[FORWARD] = BlockManager.GetBlockNear(Position + Vector3.up + new Vector3(0f, 0f, 1f));
        blocksAbove[BACK] = BlockManager.GetBlockNear(Position + Vector3.up + new Vector3(0f, 0f, -1f));
        blocksAbove[LEFT] = BlockManager.GetBlockNear(Position + Vector3.up + new Vector3(-1f, 0f, 0f));
        blocksAbove[RIGHT] = BlockManager.GetBlockNear(Position + Vector3.up + new Vector3(1f, 0f, 0f));
    }

    protected void HardUpdateBlocksBelow()
    {
        blocksBelow[DOWN] = BlockManager.GetBlockNear(Position + Vector3.down);
        blocksBelow[FORWARD] = BlockManager.GetBlockNear(Position + Vector3.down + new Vector3(0f, 0f, 1f));
        blocksBelow[BACK] = BlockManager.GetBlockNear(Position + Vector3.down + new Vector3(0f, 0f, -1f));
        blocksBelow[LEFT] = BlockManager.GetBlockNear(Position + Vector3.down + new Vector3(-1f, 0f, 0f));
        blocksBelow[RIGHT] = BlockManager.GetBlockNear(Position + Vector3.down + new Vector3(1f, 0f, 0f));
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
    }

    public int NumberOfActualSupporters()
    {
        int ret = 0;
        foreach (IBlock support in blocksBelow)
        {
            if (support != null && (support.MotionState == BlockMotionState.Edged || support.MotionState == BlockMotionState.Grounded))
                ++ret;
        }
        return ret;
    }
    #endregion

    #region colliders and triggers
    public void OnCollisionEnter(Collision collision)
    {
        IBlock otherBlock = collision.gameObject.GetComponent<IBlock>();
        if (otherBlock == null)
        {
            IPlayerCharacter player = collision.gameObject.GetComponent<IPlayerCharacter>();
            if (player != null)
            {
                Profiler.BeginSample("Player collision");
                OnPlayerMovement(player, PlayerMovementEvent.EventType.Enter);
                Profiler.EndSample(); //Player collision
            }
        }
        else
            UpdateNeighborsCache();
    }

    public void OnCollisionExit(Collision collision)
    {
        Profiler.BeginSample("Collision exit");
        IPlayerCharacter player = collision.gameObject.GetComponent<IPlayerCharacter>();
        if (player != null)
        {
            Profiler.BeginSample("Player collision");
            OnPlayerMovement(player, PlayerMovementEvent.EventType.Leave);
            Profiler.EndSample(); //Player collision
        }
        else
            UpdateNeighborsCache();

        Profiler.EndSample(); //Collision exit
    }
    #endregion

    #region gravity movement
    /// <summary>
    /// Sets this blocks current IBlock.MotionState based on the precense or absence of
    /// supporting blocks. Note that this method _only_ sets the IBlock.MotionState
    /// property and does not actual activate gravity or other such actions.
    /// </summary>
    internal void SetMotionStateBySupport()
    {
        //If we are currently in motion then we can wait - the move() methods will call this
        //again once they are finished.
        if (MotionState == BlockMotionState.Moving || MotionState == BlockMotionState.Sliding)
            return;

        BlockMotionState oldState = MotionState;

        //We might be able to move this down within the method - at least for now this is
        //very uncommon
        if (GravityFactor == 0f)
        {
            MotionState = BlockMotionState.Grounded;
            if (MotionState != oldState)
                OnMotionStateChanged(MotionState, oldState);
            return;
        }

        MotionState = BlockMotionState.Unknown;

        Profiler.BeginSample("On the floor");
        //If we are resting on the floor
        if (Position.y == 1)
        {
            if (oldState != BlockMotionState.Falling)
            {
                MotionState = BlockMotionState.Grounded;
                if (MotionState != oldState)
                    OnMotionStateChanged(MotionState, oldState);
                return;
            }
            else
            {
                BlockManager.DestroyBlock(this);
            }
        }
        Profiler.EndSample(); //On the floor

        Profiler.BeginSample("No supporters");
        //If there is nothing below us we can dip out quick
        if (BelowCount == 0 || NumberOfActualSupporters() == 0)
        {
            if (oldState != BlockMotionState.Falling)
                MotionState = BlockMotionState.Hovering;
            else
                MotionState = BlockMotionState.Falling;
            if (MotionState != oldState)
                OnMotionStateChanged(MotionState, oldState);
            return;
        }
        Profiler.EndSample(); //No supporters

        Profiler.BeginSample("Supported");
        if (blocksBelow[DOWN] != null && (blocksBelow[DOWN].MotionState == BlockMotionState.Edged || blocksBelow[DOWN].MotionState == BlockMotionState.Grounded))
        {
            MotionState = BlockMotionState.Grounded;
            if (MotionState != oldState)
                OnMotionStateChanged(MotionState, oldState);
            return;
        }
        else
        {
            Profiler.BeginSample("Edged");
            MotionState = BlockMotionState.Edged;
            if (oldState != BlockMotionState.Unknown && oldState != BlockMotionState.Edged && EdgeEffect != null)
            {
                if (blocksBelow[LEFT] != null)
                    VisualEffectsManager.PlayEffect(this, EdgeEffect, EdgeEffectScale, -1, new Vector3(-0.5f, -0.5f, -0.5f));
                if (blocksBelow[RIGHT] != null)
                    VisualEffectsManager.PlayEffect(this, EdgeEffect, EdgeEffectScale, -1, new Vector3(0.5f, -0.5f, -0.5f));
                //TODO Figur out correct Vector3 and Quaterion values for issue #32
                //if (blocksBelow[FORWARD] != null)
                //    BlockManager.PlayEffect(this, EdgeEffect, EdgeEffectScale, -1, new Vector3(-0.5f, -0.5f, -0.5f),Quaternion.Euler(0,90,0));
                //if (blocksBelow[BACK] != null)
                //    BlockManager.PlayEffect(this, EdgeEffect, EdgeEffectScale, -1, new Vector3(0.5f, -0.5f, -0.5f),Quaternion.Euler(0,90,0));

            }
            if (MotionState != oldState)
                OnMotionStateChanged(MotionState, oldState);
            Profiler.EndSample(); //Edged
        }
        Profiler.EndSample(); //Supported
    }

    [SerializeField]
    bool _gravityEnabled = false;
    bool _gravityDirty = false;
    /// <summary>
    /// When true the block's RigidBody component will be set to allow this block to be affected by gravity otherwise gravity is
    /// off and the RigidBody's constraints are set. When we eventually change to physics-based block pushing this will need
    /// to be refactored. Note that unless _gravityDirty has been set to true setting this property multiple times to the same
    /// value will not produce any calls to the underlying RigidBody.
    /// </summary>
    public virtual bool GravityEnabled
    {
        get
        {
            return _gravityEnabled;
        }
        set
        {
            if (!BlockManager.PlayMode)
                value = false;

            if (value == _gravityEnabled && !_gravityDirty)
                return;
            Profiler.BeginSample("Changing physics state");
            UpdateNeighborsCache();
            if (value)
            {
                GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
                GetComponent<Rigidbody>().useGravity = true;
                _gravityEnabled = true;
                _gravityDirty = false;
            }
            else
            {
                transform.position = Utility.Round(Position, 0);
                transform.rotation = Quaternion.identity;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().useGravity = false;
                _gravityEnabled = false;
                _gravityDirty = false;
            }
            Profiler.EndSample();
        }
    }


    bool _startedHover = false;
    /// <summary>
    /// This method first calls AbstractBlock.SetMotionStateBySupport() to update this block's
    /// motion state, then enables or disables gravity using AbstractBlock.GravityEnabled based
    /// on the new motion state.
    /// </summary>
    internal void SetGravityEnabledByMotionState()
    {
        SetMotionStateBySupport();
        if (MotionState == BlockMotionState.Edged || MotionState == BlockMotionState.Grounded)
        {
            GravityEnabled = false;
        }
        else if (MotionState == BlockMotionState.Falling)
        {
            GravityEnabled = true;
        }
        else if (MotionState == BlockMotionState.Hovering)
        {
            if (!_startedHover)
            {
                _startedHover = true;
                StartCoroutine(DoHoverAnimation());
                foreach (IBlock neighbor in blocksAbove)
                {
                    if (neighbor != null)
                    {
                        (neighbor as AbstractBlock).UpdateNeighborsCache();
                        (neighbor as AbstractBlock).SetGravityEnabledByMotionState();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Coroutine for wobbling the block prior to falling. If the block's MotionState changes
    /// during this animation the wobbling will stop and _gravityDirty will be set to true
    /// so that GravityEnabled will ensure proper settings the next time it is set.
    /// </summary>
    /// <returns></returns>
    /// TODO: Don't hardcode the length of hover
    private IEnumerator DoHoverAnimation()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        UpdateNeighborsCache();
        SetGravityEnabledByMotionState();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (MotionState != BlockMotionState.Hovering)
        {
            _startedHover = false;
            _gravityDirty = true;
            yield break;
        }
        if (BlockManager.PlayMode)
        {
            rb.constraints = (RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePosition);
            transform.Rotate(0f, 0f, 5f);
            yield return new WaitForSeconds(0.1f);
            if (MotionState != BlockMotionState.Hovering)
            {
                _startedHover = false;
                _gravityDirty = true;
                yield break;
            }
            transform.Rotate(0f, 0f, -10f);
            yield return new WaitForSeconds(0.1f);
            if (MotionState != BlockMotionState.Hovering)
            {
                _startedHover = false;
                _gravityDirty = true;
                yield break;
            }
            transform.Rotate(0f, 0f, 10f);
            yield return new WaitForSeconds(0.1f);
            if (MotionState != BlockMotionState.Hovering)
            {
                _startedHover = false;
                _gravityDirty = true;
                yield break;
            }
            transform.Rotate(0f, 0f, -5f);
            yield return new WaitForSeconds(0.1f);
            if (MotionState != BlockMotionState.Hovering)
            {
                _startedHover = false;
                _gravityDirty = true;
                yield break;
            }
            transform.rotation = Quaternion.identity;
            if (MotionState != BlockMotionState.Hovering)
            {
                _startedHover = false;
                _gravityDirty = true;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
            if (MotionState != BlockMotionState.Hovering)
            {
                _startedHover = false;
                _gravityDirty = true;
                yield break;
            }
        }
        _startedHover = false;
        _gravityDirty = true;
        MotionState = BlockMotionState.Falling;
    }
    #endregion

    protected virtual void OnMotionStateChanged(BlockMotionState newState, BlockMotionState oldState) { }

    #region Player movement event handling
    public virtual void OnPlayerMovement(IPlayerCharacter player, PlayerMovementEvent.EventType type)
    {
        Debug.Assert(player != null);
        PlayerMovementEvent ev = new PlayerMovementEvent();
        ev.Type = type;
        ev.Player = player;
        if (player.Position.y > Position.y && player.Position.x == Position.x && player.Position.z == Position.z)
            ev.Location = PlayerMovementEvent.EventLocation.Top;
        else if (player.Position.y < Position.y && player.Position.x == Position.x && player.Position.z == Position.z)
            ev.Location = PlayerMovementEvent.EventLocation.Bottom;
        else if (player.Position.x == Position.x ^ player.Position.z == Position.z && player.Position.y == Position.y)
            ev.Location = PlayerMovementEvent.EventLocation.Side;
        else
            ev.Location = PlayerMovementEvent.EventLocation.None;
        OnPlayerMovement(ev);
    }

    protected virtual void OnPlayerMovement(PlayerMovementEvent ev)
    {
        Debug.Assert(ev != null);
        switch (ev.Type)
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

    protected virtual void OnPlayerEnter(PlayerMovementEvent ev) { }
    protected virtual void OnPlayerLeave(PlayerMovementEvent ev) { }
    protected virtual void OnPlayerStay(PlayerMovementEvent ev) { }
    protected virtual void OnPlayerUnknownMotion(PlayerMovementEvent ev) { }
    #endregion

    #region input handling
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
                if (BlockManager.ActiveBlock == this)
                    UIManager.ShowBlockEditDialog(this);
                BlockManager.Cursor.transform.position = Position;
            }
#if UNITY_EDITOR
            Selection.activeGameObject = gameObject;
#endif
        }
    }

    public void AddChild(IHierarchical child)
    {
        throw new HierarchyException("Blocks do not yet support Hierarchies");
    }

    public void RemoveChild(IHierarchical child)
    {
        throw new HierarchyException("Blocks do not yet support Hierarchies");
    }
    #endregion
}

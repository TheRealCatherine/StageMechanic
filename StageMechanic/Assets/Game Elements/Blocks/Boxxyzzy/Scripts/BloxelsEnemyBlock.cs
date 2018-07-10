using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloxelsEnemyBlock : AbstractBloxelsBlock
{
	private bool _pushing = false;
	private bool _moveLeft = false;

	private const string	PROPNAME_MOVELEFT = "Start facing left";
	private const string	PROPNAME_STEPDELAY = "Step delay (in seconds)";

	private const float	DEFAULT_STEPDELAY = 1.0f;
	private const int	DEFAULT_MOVELEFT = 1;

	private float		_stepDelayProperty = DEFAULT_STEPDELAY;
	private int		_moveLeftProperty  = DEFAULT_MOVELEFT;

	public override string TypeName
	{
		get
		{
			return "Enemy";
		}

		set
		{
			throw new System.NotImplementedException();
		}
	}

	public void SetInitDirection()
	{
		_moveLeft = ((_moveLeftProperty == 0)?false:true);
		Debug.Log("SetInitDirection Called moveLeft " + _moveLeft);
		StaticMeshInstance.gameObject.transform.RotateAround(transform.position, transform.up, _moveLeft?90f:-90f);
	}

	public IEnumerator MoveEnemy(Vector3 direction) 
	{
		if (!_pushing) {
			_pushing = true;
			Push(direction, 1);
			yield return new WaitForEndOfFrame();
			yield return new WaitForSeconds(_stepDelayProperty);
			_pushing = false;
		}
	}

	internal override void Update()
	{
		base.Update();
		if (BlockManager.PlayMode) {
			if (MotionState == BlockMotionState.Grounded) {
				Vector3 direction = _moveLeft?Vector3.left:Vector3.right;

				// Check if there is something in the way
				IBlock neighbor = BlockManager.GetBlockNear(Position + direction);
				if (neighbor != null) {
					ChangeDirection();
					return;
				}
				// Check if we have support
				neighbor = BlockManager.GetBlockNear(Position + direction + new Vector3(0,-1,0));
				if (neighbor == null) {
					ChangeDirection();
					return;
				}
				StartCoroutine(MoveEnemy(direction));	
			}
		}
	}

	public void ChangeDirection() {
		_moveLeft = !_moveLeft;
		StaticMeshInstance.gameObject.transform.RotateAround(transform.position, transform.up, 180f);
		
	}
	
	public override void ApplyTheme(AbstractBlockTheme theme)
	{
		throw new System.NotImplementedException();
	}

	protected override void OnPlayerEnter(PlayerMovementEvent ev)
	{
		base.OnPlayerEnter(ev);
		ev.Player.TakeDamage(10f, "Enemy Contact");
	}

	protected override void OnPlayerStay(PlayerMovementEvent ev)
	{
		base.OnPlayerStay(ev);
		ev.Player.TakeDamage(10f, "Enemy Contact");
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add(PROPNAME_MOVELEFT,   new DefaultValue { TypeInfo = typeof(int), Value = DEFAULT_MOVELEFT.ToString() } );
			ret.Add(PROPNAME_STEPDELAY,  new DefaultValue { TypeInfo = typeof(float), Value = DEFAULT_STEPDELAY.ToString() } );
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			ret.Add(PROPNAME_MOVELEFT, _moveLeftProperty.ToString());
			ret.Add(PROPNAME_STEPDELAY, _stepDelayProperty.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey(PROPNAME_MOVELEFT)) {
				_moveLeftProperty = int.Parse(value[PROPNAME_MOVELEFT]);
				Debug.Log("Move Left has been set to " + _moveLeftProperty);
				SetInitDirection();
			}
			if (value.ContainsKey(PROPNAME_STEPDELAY)) {
				_stepDelayProperty = float.Parse(value[PROPNAME_STEPDELAY]);
			}
		}
	}

}

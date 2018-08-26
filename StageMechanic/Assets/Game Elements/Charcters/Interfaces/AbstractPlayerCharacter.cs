/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayerCharacter : MonoBehaviour, IPlayerCharacter
{
    public virtual string Name
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

    public virtual Vector3 Position
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

    protected Vector3 _facingDirection = Vector3.back;
    public virtual Vector3 FacingDirection { get { return _facingDirection; } set { _facingDirection = value; } }

    public abstract List<string> StateNames { get; }

    public abstract int CurrentStateIndex { get; set; }

    public virtual Dictionary<string, string[]> SuggestedInputs
    {
        get
        {
            Dictionary<string, string[]> ret = new Dictionary<string, string[]>();
            ret.Add("Up", new string[] { "up", "joystick 1 7th axis +", "joystick 1 Y axis +" });
            ret.Add("Down", new string[] { "down", "joystick 1 7th axis -", "joystick 1 Y axis -" });
            ret.Add("Left", new string[] { "left", "joystick 1 6th axis -", "joystick 1 X axis -" });
            ret.Add("Right", new string[] { "right", "joystick 1 6th axis +", "joystick 1 X axis +" });
            ret.Add("Grab", new string[] { "left shift", "right shift", "joystick 1 button 0", "Grab" });
            ret.Add("Item", new string[] { "space", "joystick 1 button 1" });
            return ret;
        }
    }

    public GameObject GameObject
    {
        get
        {
            Debug.Assert(gameObject != null);
            return gameObject;
        }
    }

	/// <summary>
	/// Lets go ahead and assume all AbstractPlayerCharacters can have an int-based score.
	/// When possible, this should be accessed as a Property to ensure extensibility
	/// though initial implementations can use this to speed up production.
	/// </summary>
	public int Score
	{
		get; set;
	}

	public IItem Item
	{
		get; set;
	}

    public virtual Dictionary<string, string> Properties
    {
        get
        {
			Dictionary<string, string> ret = new Dictionary<string, string>();
			ret.Add("Score", Score.ToString());
			return ret;
        }
        set {
			if(value.ContainsKey("Score"))
				Score = int.Parse("Score");
		}
    }

	/// <summary>
	/// Convenience method for setting a single property
	/// </summary>
	/// <param name="name"></param>
	/// <param name="value"></param>
	public void SetProperty(string name, string value)
	{
		Dictionary<string, string> dict = new Dictionary<string, string>(1);
		dict.Add(name, value);
		Properties = dict;
	}

    public virtual bool ApplyGravity(float factor = 1f, float acceleration = 0f)
    {
        Position -= new Vector3(0f, factor, 0f);
        return true;
    }

    public abstract float ApplyInput(List<string> inputNames, Dictionary<string, string> parameters = null);

    public virtual bool Face(Vector3 direction)
    {
        _facingDirection = direction;
        return true;
    }

    public virtual Dictionary<string, string> InputParameters(string inputName)
    {
        return new Dictionary<string, string>();
    }

    public virtual bool TurnAround()
    {
        Face(-FacingDirection);
        return true;
    }

    public virtual bool Turn(Vector3 direction)
    {
        if (direction == Vector3.right)
        {
            TurnRight();
            return true;
        }
        else if (direction == Vector3.left)
        {
            TurnLeft();
            return true;
        }
        else if (direction == Vector3.zero)
        {
            LogController.Log("Daddy! Quit skipping my turn!");
            return false;
        }
        else
            return false;
    }

    public virtual bool TurnRight()
    {
        if (FacingDirection == Vector3.forward)
            return Face(Vector3.right);
        else if (FacingDirection == Vector3.right)
            return Face(Vector3.back);
        else if (FacingDirection == Vector3.left)
            return Face(Vector3.forward);
        else
            return Face(Vector3.left);
    }

    public virtual bool TurnLeft()
    {
        if (FacingDirection == Vector3.forward)
            return Face(Vector3.left);
        else if (FacingDirection == Vector3.right)
            return Face(Vector3.forward);
        else if (FacingDirection == Vector3.back)
            return Face(Vector3.right);
        else
            return Face(Vector3.back);
    }

    public virtual bool TakeDamage(float amount = float.PositiveInfinity, string type = null)
    {
        if (!UIManager.IsSinglePlayerDeathDialogOpen)
        {
            UIManager.ShowSinglePlayerDeathDialog();
        }
        return true;
    }

	public virtual bool UseItem()
	{
		if (Item == null)
			return false;
		Item.OnPlayerActivate(this);
		return true;
	}
}

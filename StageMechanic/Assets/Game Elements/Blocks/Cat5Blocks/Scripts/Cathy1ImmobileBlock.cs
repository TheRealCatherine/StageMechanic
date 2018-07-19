/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using UnityEngine;

public class Cathy1ImmobileBlock : Cathy1Block {

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		Debug.Assert(theme.Immobile != null);
		Model1 = theme.Immobile;
	}

	public override void Awake () {
        base.Awake();
		WeightFactor = 0f;
	}
}

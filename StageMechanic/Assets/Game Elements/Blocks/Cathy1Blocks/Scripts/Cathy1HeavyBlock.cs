/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
 
 public class Cathy1HeavyBlock : Cathy1Block {

	internal override void Start () {
        base.Start();
        WeightFactor = 2.5f;
	}
}

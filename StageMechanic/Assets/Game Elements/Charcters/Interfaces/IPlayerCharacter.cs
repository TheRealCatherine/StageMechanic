/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCharacter {

    GameObject GameObject { get; }

    string Name { get; set; }

    Vector3 Position { get; set; }

    Vector3 FacingDirection { get; set; }

    List<string> StateNames { get; }

    int CurrentStateIndex { get; set; }

    Dictionary<string, string> Properties { get; set; }

    Dictionary<string, string[]> SuggestedInputs { get; }

    Dictionary<string, string> InputParameters(string inputName);

    float ApplyInput(List<string> inputNames, Dictionary<string, string> parameters = null);

    bool ApplyGravity(float factor=1f, float acceleration=0f);

    bool Face(Vector3 direction);

    bool Turn(Vector3 direction);

    bool TurnAround();

    bool TurnLeft();

    bool TurnRight();

    bool TakeDamage(float amount = float.PositiveInfinity, string type = null);
}

/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;

public class Cathy1PlayerStartLocation : Cathy1AbstractEvent, IPlayerEventExtension {
    public int PlayerNumber;

    int IPlayerEventExtension.PlayerNumber
    {
        get
        {
            return PlayerNumber;
        }

        set
        {
            PlayerNumber = value;
        }
    }

    public override void Awake()
    {
        base.Awake();
        Type = EventType.PlayerStart;
    }

    public override Dictionary<string, string> Properties
    {
        get
        {
            //Dictionary<string, string> ret = base.Properties;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PlayerNumber", PlayerNumber.ToString());
            return ret;
        }
        set
        {
            if (value.ContainsKey("PlayerNumber"))
                PlayerNumber = Int32.Parse(value["PlayerNumber"]);
            //base.Properties = value;
            //TODO
        }
    }
}

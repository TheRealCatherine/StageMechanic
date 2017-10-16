using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerEventExtension {

    /// <summary>
    /// The index number of the associated player (ie Player 1, Player 2, etc.)
    /// </summary>
    int PlayerNumber { get; set; }
}

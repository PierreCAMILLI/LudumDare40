using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : SingletonBehaviour<Controls> {

    [SerializeField]
    private PlayerControls[] _players;

}

[System.Serializable]
public class PlayerControls
{

}

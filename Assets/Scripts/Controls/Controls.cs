using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : SingletonBehaviour<Controls> {

    [SerializeField]
    private PlayerControls[] _players;

    public PlayerControls Player(byte playerIndex = 0)
    {
        return _players[playerIndex];
    }

}

[System.Serializable]
public class PlayerControls
{
    [SerializeField]
    string _horizontalAxis = "Horizontal";
    [SerializeField]
    string _verticalAxis = "Vertical";
    [SerializeField]
    KeyCode[] _throw;
    [SerializeField]
    KeyCode _melee;

    #region Getter
    public float Horizontal
    {
        get { return Input.GetAxis(_horizontalAxis); }
    }

    public float Vertical
    {
        get { return Input.GetAxis(_verticalAxis); }
    }

    public Vector2 Movement
    {
        get { return new Vector2(Horizontal, Vertical).normalized; }
    }

    public bool Throw(byte index)
    {
        return Input.GetKeyDown(_throw[index]);
    }

    public int ThrowCount
    {
        get { return _throw.Length; }
    }
    
    public bool Melee
    {
        get { return Input.GetKeyDown(_melee); }
    }
    #endregion
}

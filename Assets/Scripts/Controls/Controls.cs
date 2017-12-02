using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : SingletonBehaviour<Controls> {

    [SerializeField]
    private PlayerControls[] _players;

    public PlayerControls Player(byte playerNumber = 0)
    {
        return _players[playerNumber];
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
    KeyCode _throw;
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
    
    public bool Throw
    {
        get { return Input.GetKeyDown(_throw); }
    }
    
    public bool Melee
    {
        get { return Input.GetKeyDown(_melee); }
    }
    #endregion
}

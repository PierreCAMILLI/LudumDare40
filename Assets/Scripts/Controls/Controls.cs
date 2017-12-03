using System;
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

    private void OnValidate()
    {
        foreach (PlayerControls player in _players)
            player.OnValidate();
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
    KeyCode[] _throwAlternative;
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
        return Input.GetKeyDown(_throw[index]) || Input.GetKeyDown(_throwAlternative[index]);
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

    public void OnValidate()
    {
        if(_throwAlternative.Length != _throw.Length)
        {
            Array.Resize(ref _throwAlternative, _throw.Length);
        }
    }
}

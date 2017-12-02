using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    Player _player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_player == null)
            _player = GetComponent<Player>();

        PlayerControls controls = Controls.Instance.Player();

        _player.Velocity = controls.Movement;
        for(byte i = 0; i < controls.ThrowCount; ++i)
        {
            if (controls.Throw(i))
                _player.Throw(i);
        }
	}
}

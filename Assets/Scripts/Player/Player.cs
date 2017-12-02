using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBehaviour<Player> {

    [SerializeField]
    private float _speed = 1f;

    private Vector2 _velocity;
    public Vector2 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMovements();
	}

    void UpdateMovements()
    {
        transform.Translate(_velocity * _speed * Time.deltaTime);
        _velocity = Vector2.zero;
    }
}

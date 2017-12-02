using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBehaviour<Player> {

    [SerializeField]
    private float _speed = 1f;

    [SerializeField]
    private float _size = 1f;
    public float Size
    {
        get { return _size; }
    }

    [SerializeField]
    private float _throwForce = 3f;

    private Vector2 _velocity;
    public Vector2 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    private Vector2 _forward;
    public Vector2 Forward
    {
        get { return _forward; }
        set { _forward = value; }
    }

	// Use this for initialization
	void Start () {
        _forward = Vector2.down;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMovements();
        transform.localScale = Vector3.one * _size;
	}

    void UpdateMovements()
    {
        transform.Translate(_velocity * _speed * Time.deltaTime);
        _velocity = Vector2.zero;
    }

    /// <summary>
    /// Moves the player to a position in the world
    /// </summary>
    /// <param name="position"></param>
    /// <returns>Indicates whether or not the player reached the position</returns>
    public bool MoveTo(Vector3 position)
    {
        Vector3 dir = (position - transform.position);
        if(Vector2.SqrMagnitude(dir) > _speed * _speed)
        {
            _velocity += (Vector2) dir.normalized;
            return false;
        }
        else
        {
            transform.position = position;
            return true;
        }

    }

    public bool Throw(byte objectIndex = 0)
    {
        GameObject go = Inventory.Instance.instanciateItem(Inventory.Instance.popItem(objectIndex));
        if(go != null)
        {
            go.transform.position = transform.position + (Vector3)Forward;

            Item item = go.GetComponent<Item>();
            if (item != null)
            {
                item.thrown = true;
                item.cooldownSensitive = true;
            }

            Rigidbody2D rigidbody = go.GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                rigidbody.AddForce((Forward) * _throwForce + (Velocity * _speed), ForceMode2D.Impulse);
            }
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ennemy enemy = collision.gameObject.GetComponent<Ennemy>();
        if(enemy != null)
        {
            enemy.Hurt();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null)
        {
            if (!item.thrown)
            {
                Inventory.Instance.PushFront(item);
                Destroy(item.gameObject);
            }
        }
    }
}

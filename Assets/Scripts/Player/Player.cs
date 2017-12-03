using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBehaviour<Player> {

    [SerializeField]
    private float _speed = 1f;

    [Header("Resize")]
    [SerializeField]
    private float _sizeTarget = 1f;
    [SerializeField]
    private float _resizeSmoothTime = 1f;
    [SerializeField]
    private float _resizeVelocity;
    private float _size = 1f;
    public float Size
    {
        get { return _size; }
    }
    public float SizeTarget;
    public float SizeMin = 1.0F;
    public float SizeMax = 4.0F;
    public float grownFactor;

    [Header("Throw")]
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

#region Force
    private Vector2 _force;
    public Vector2 Force
    {
        get { return _force; }
        set { _force = value; }
    }

    private Vector2 _forceVelocity;
#endregion

    // Use this for initialization
    void Start () {
        _forward = Vector2.down;
        _force = Vector2.zero;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        UpdateMovements();
        transform.localScale = Vector3.one * _size;
        _size = Mathf.SmoothDamp(_size, _sizeTarget, ref _resizeVelocity, _resizeSmoothTime, Mathf.Infinity, Time.deltaTime);
	}

    void UpdateMovements()
    {
        transform.Translate(((_velocity * _speed) + _force) * Time.deltaTime);
        _force = Vector2.SmoothDamp(_force, Vector2.zero, ref _forceVelocity, 1f, Mathf.Infinity, Time.deltaTime);
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
            go.transform.position = transform.position + SizeTarget * (Vector3)Forward;

            Item item = go.GetComponent<Item>();
            if (item != null)
            {
                item.thrown = true;
                if(item.type == Item.Type.FOOD)
                    item.cooldownSensitive = true;

                SizeTarget -= grownFactor;
                _sizeTarget = SizeTarget;
                SizeTarget = Mathf.Clamp(SizeTarget, SizeMin, SizeMax);
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

        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null)
        {
            Rigidbody2D rigidbody = item.GetComponent<Rigidbody2D>();
            float controlSpeed = Controls.Instance.Player().Movement.sqrMagnitude;
            if (rigidbody != null && rigidbody.velocity.sqrMagnitude <=  _speed * _speed * controlSpeed)
            {
                Inventory.Instance.PushFront(item);
                Destroy(item.gameObject);

                SizeTarget += grownFactor;
                _sizeTarget = SizeTarget;
                SizeTarget = Mathf.Clamp(SizeTarget, SizeMin, SizeMax);
            }
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
		Ennemy ennemy = collision.gameObject.GetComponent<Ennemy> ();
        if (item != null)
        {
            Rigidbody2D rigidbody = item.GetComponent<Rigidbody2D>();
            float controlSpeed = Controls.Instance.Player().Movement.sqrMagnitude;
            if (rigidbody != null && rigidbody.velocity.sqrMagnitude <= _speed * _speed * controlSpeed)
            {
                Inventory.Instance.PushFront(item);
                Destroy(item.gameObject);

                SizeTarget += grownFactor;
                _sizeTarget = SizeTarget;
                SizeTarget = Mathf.Clamp(SizeTarget, SizeMin, SizeMax);
            }
        }

		if (ennemy != null) {

		}
    }
}

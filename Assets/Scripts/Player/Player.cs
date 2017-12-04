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
    public float SizeTarget
    {
        get { return _sizeTarget; }
        set { _sizeTarget = value; }
    }
    [SerializeField]
    public Bounds1D _sizeBounds;
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

	[Range(0,5)]
	public int invulnerabilityTime = 3;
	public bool _invulnerable = false;

#region Force
    private Vector2 _force;
    public Vector2 Force
    {
        get { return _force; }
        set { _force = value; }
    }

    private Vector2 _forceVelocity;
#endregion

	public int _circleSizePopObject = 5;

    // Use this for initialization
    void Start () {
        _forward = Vector2.down;
        _force = Vector2.zero;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Animator animationPlayer = GetComponent<Animator>();
		if (Velocity != Vector2.zero) {
			if (Velocity.x >= 0)
				GetComponent<SpriteRenderer> ().flipX = true;
			else 
				GetComponent<SpriteRenderer> ().flipX = false;
			animationPlayer.SetBool ("walking", true);
		} else
			animationPlayer.SetBool ("walking", false);
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
        RaycastHit2D oclusion = Physics2D.CircleCast(transform.position + SizeTarget * (Vector3)Forward, 0.5F, Forward, 0, 1 << LayerMask.NameToLayer("Map"));
        if(!oclusion)
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
                
                    SizeTarget = Mathf.Clamp(SizeTarget - grownFactor, _sizeBounds.Min, _sizeBounds.Max);
                }

                Rigidbody2D rigidbody = go.GetComponent<Rigidbody2D>();
                if (rigidbody != null)
                {
                    rigidbody.AddForce((Forward) * _throwForce + (Velocity * _speed), ForceMode2D.Impulse);
                }
                return true;
            }
        }
        return false;
    }

	IEnumerator InvulnerableRoutine()
	{
		yield return new WaitForSeconds(invulnerabilityTime);
		_invulnerable = false;
		StopCoroutine ("BlikingRoutin");
		SpriteRenderer spritePlayer = GetComponent<SpriteRenderer> ();
		spritePlayer.color = new Color(1f,1f,1f,1f) ;
	}

	IEnumerator BlikingRoutin()
	{
		while (_invulnerable) {
			SpriteRenderer spritePlayer = GetComponent<SpriteRenderer> ();
			if (spritePlayer.color.a == 1)
				spritePlayer.color = new Color (1f, 1f, 1f, 0f);
			else
				spritePlayer.color = new Color (1f, 1f, 1f, 1f);
			yield return new WaitForSeconds (0.2f);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
		Item item = collision.gameObject.GetComponent<Item>();
		if (item != null)
		{
			Rigidbody2D rigidbody = item.GetComponent<Rigidbody2D>();
			float controlSpeed = Controls.Instance.Player().Movement.sqrMagnitude;
			if (rigidbody != null && rigidbody.velocity.sqrMagnitude <=  _speed * _speed * controlSpeed)
			{
				Animator animationPlayer = GetComponent<Animator>();
				animationPlayer.SetTrigger("melee");
				Inventory.Instance.PushFront(item);
				Destroy(item.gameObject);

                SizeTarget = Mathf.Clamp(_sizeTarget + grownFactor, _sizeBounds.Min, _sizeBounds.Max);
            }
		}

        Ennemy ennemy = collision.gameObject.GetComponent<Ennemy>();
        if (ennemy != null) {
			if (ennemy.Stunned) {
				ennemy.died ();
				switch (ennemy.Ennemytype) {
				case Ennemy.Ennemies.animal:
					Inventory.Instance.PushFrontElement (Item.Element.MEAT);
					Inventory.Instance.PushFrontElement (Item.Element.BONE);
					break;
				case Ennemy.Ennemies.peacefulAnimal:
					Inventory.Instance.PushFrontElement (Item.Element.MEAT);
					break;
				case Ennemy.Ennemies.goblin:
					Inventory.Instance.PushFrontElement (Item.Element.MEAT);
					Inventory.Instance.PushFrontElement (Item.Element.DIAMOND);
					Inventory.Instance.PushFrontElement (Item.Element.COIN);
					break;
				case Ennemy.Ennemies.hero:
					Inventory.Instance.PushFrontElement (Item.Element.MEAT);
					Inventory.Instance.PushFrontElement (Item.Element.APPLE);
					Inventory.Instance.PushFrontElement (Item.Element.SWORD);
					Inventory.Instance.PushFrontElement (Item.Element.DIAMOND);
					break;
				default:
					break;
				}
			}
			else {
				if (!_invulnerable) {
					if (Inventory.Instance.getInventorySize() == 0) {
						gameOver ();
					} else {
						ennemy.gameObject.GetComponent<Animator> ().SetTrigger ("attack");
						Animator animationPlayer = GetComponent<Animator> ();
						animationPlayer.SetTrigger ("Hurt");
						int magnitude = 6;
						Vector3 force = transform.position - collision.transform.position;
						force.Normalize ();
						GetComponent<Rigidbody2D> ().AddForce (force * magnitude, ForceMode2D.Impulse);
						//TODO Frame d'invulnérabilité et perte de 6 objets.

						createObjectAroundPlayer (Inventory.Instance.popItem (0));
						createObjectAroundPlayer (Inventory.Instance.popItem (1));
						createObjectAroundPlayer (Inventory.Instance.popItem (2));
						createObjectAroundPlayer (Inventory.Instance.popItem (0));
						createObjectAroundPlayer (Inventory.Instance.popItem (1));
						createObjectAroundPlayer (Inventory.Instance.popItem (2));
						for (int i = 0; i < 6; i++) {
							SizeTarget = Mathf.Clamp (SizeTarget - grownFactor, _sizeBounds.Min, _sizeBounds.Max);
						}
						//change le premier paramètre par la position ou l'on veut poser l'objet
						//

						_invulnerable = true;
						StartCoroutine (InvulnerableRoutine ());
						StartCoroutine ("BlikingRoutin", 0.1f);
					}
				}
			}
		}
    }

	private bool createObjectAroundPlayer(Item.Element item)
	{
		
		if (item != Item.Element.APPLE && item != Item.Element.FISH && item != Item.Element.MEAT && item != Item.Element.NONE) {
			for (int i = 0; i < 100; i++) {
				Vector2 itemPositionTmp;
				itemPositionTmp = (Vector2)transform.position + Random.insideUnitCircle * _circleSizePopObject * SizeTarget;
				RaycastHit2D oclusion = Physics2D.CircleCast(itemPositionTmp, 0.5F, Forward, 0, 1 << LayerMask.NameToLayer("Map"));
				float distancePlayerObject = Vector2.Distance((Vector2)transform.position, itemPositionTmp);
				Debug.Log (oclusion);
				Debug.Log(distancePlayerObject);
				if (!oclusion && distancePlayerObject > SizeTarget * 2) {
					GameObject newItem = Inventory.Instance.instanciateItem(item);
					newItem.transform.position = itemPositionTmp;
					newItem.GetComponent<Item> ().cooldownSensitive = true;
					return true;
				}
			}
		}
		return false;
	}

	private void gameOver()
	{
		this.gameObject.SetActive(false);

        GameOverUI.Instance.ToogleGameOver(true);
	}
}

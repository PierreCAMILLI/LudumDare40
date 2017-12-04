using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour {

	public enum Ennemies {peacefulAnimal,animal,hero,goblin};
	public Ennemies Ennemytype;


	List<Item.Element> _items = new List<Item.Element>();
	[Range(0,10)]
	public int InventorySize = 0;
	public bool flee = false;
    public bool HasItems
    {
        get { return _items.Count > 0; }
    }

    [SerializeField]
    private float _dropRadius = 1f;

    private bool _stunned = false;
    public bool Stunned
    {
        get { return _stunned; }
        set { _stunned = value; }
    }

    [SerializeField]
    private float _stunTime;

	[Range(0,10)]
	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();
	public LayerMask obstacleMask;

	[SerializeField]
	LayerMask targetMask;

	bool seePlayer = false;
	bool seeItemWanted = false;

	public int MoveSpeed = 4;
	public int RotationSpeed = 2;
	private Transform targetMovement;

    // idle mode relative
    Vector3 idleTarget;
    float idleAngle;
    float idleCooldown = 0.0F;


    // Use this for initialization
    void Start () 
	{
		StartCoroutine ("FindTargetsWithDelay", .2f);
        idleTarget = transform.position;
        idleAngle = 0.0F;
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (_stunned) {
			transform.position.Set(3*Mathf.Sin (Time.time * 1.0f),transform.position.y,transform.position.z);
		}
		else
		{
		Behavior();
		Move ();
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
			if (flee && !sprite.isVisible && Ennemytype == Ennemies.peacefulAnimal)
                died();
		}
	}

	public void Move()
	{
		Animator ennemyAnimation = GetComponent<Animator> ();
		if (flee && Ennemytype == Ennemy.Ennemies.peacefulAnimal) {
			transform.right = transform.position + Player.Instance.transform.position;
			transform.position += transform.right * Time.deltaTime * MoveSpeed*20;
			ennemyAnimation.SetBool ("walking", true);
		}
		else if (targetMovement != null) {
			ennemyAnimation.SetBool ("walking", true);
			moveToTarget (targetMovement);
			Vector3 direction = targetMovement.transform.position- transform.position;
			if (direction.normalized.x >= 0)
				GetComponent<SpriteRenderer> ().flipX = false;
			else
				GetComponent<SpriteRenderer> ().flipX = true;
		}
        else // idle state (wondering randomely)
        {
            float deltaAngle = 40.0F * MoveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, idleTarget) <= 0.01F)
            {
				ennemyAnimation.SetBool ("walking", false);
                if (idleCooldown <= 0.0F)
                {
                    idleCooldown = Random.Range(1.0F, 2.0F);

                    Vector3 delta = new Vector3(Random.Range(-1.0F, 1.0F), Random.Range(-1.0F, 1.0F), 0) * 0.5F;
                    delta.Normalize();
                    idleTarget = transform.position + delta;

                }
                else idleCooldown -= Time.deltaTime;
            }
            else
            {
				ennemyAnimation.SetBool ("walking", true);
                Vector3 direction = idleTarget - transform.position;
                transform.position += direction.normalized * Time.deltaTime * MoveSpeed/2;
				if (direction.normalized.x >= 0)
					GetComponent<SpriteRenderer> ().flipX = false;
				else
					GetComponent<SpriteRenderer> ().flipX = true;

            }
        }
	}

	void moveToTarget (Transform targetPosition)
	{
		Vector3 direction = targetPosition.position - transform.position;
		transform.position += direction.normalized * Time.deltaTime * MoveSpeed;
	
	}

	void Behavior()
	{
		List<Transform> wantedItemInVision = new List<Transform> ();
		switch (this.Ennemytype) {
		case Ennemies.peacefulAnimal:
			if (visibleTargets.Count == 0)
				targetMovement = null;
			foreach (var target in visibleTargets) {
				Item item = null;
				if (target != null) {
					item = target.GetComponent<Item> ();
					if (item != null && _items.Count != InventorySize) {
						if (item.type == Item.Type.FOOD) {
							wantedItemInVision.Add (target);
						}
					}
					if (wantedItemInVision.Count != 0) {
						targetMovement = wantedItemInVision [0];
					}
				}
			}
			break;
		case Ennemies.animal:
			
			if (visibleTargets.Count == 0)
				targetMovement = null;
			
			foreach (var target in visibleTargets) {
				Item item = null;
				Player player = null;
				if (target != null) {
					item = target.GetComponent<Item> ();
					player = target.GetComponent<Player> ();
				}
				
				if (item != null && _items.Count != InventorySize) {
					if (item.type == Item.Type.FOOD) {
						wantedItemInVision.Add (target);
					}
				} else if (player != null) {
					targetMovement = target;
				}
			}
			if (wantedItemInVision.Count != 0) {
				targetMovement = wantedItemInVision [0];
			}
			break;
		case Ennemies.goblin:
			break;
		case Ennemies.hero:
			
			Item itemSee = null;
			Player playerSee = null;

			if (visibleTargets.Count == 0) {
				
				targetMovement = null;
			}
			
			foreach (var target in visibleTargets) {

				if (target != null) {
					itemSee = target.GetComponent<Item> ();
					playerSee = target.GetComponent<Player> ();
				}

				if (itemSee != null && _items.Count != InventorySize) {
					if (itemSee.type == Item.Type.WEAPON) {
						wantedItemInVision.Add (target);
					}
				} else if (playerSee != null) {
					targetMovement = target;
				}
				if (wantedItemInVision.Count != 0 && playerSee == null) {
					targetMovement = wantedItemInVision [0];
				}
			}
			break;
		default:
			break;
		}
	}

	private void OnDrawGizmos()
	{
		float anglePersonnage = transform.eulerAngles.z;
		float anglePos = ((anglePersonnage + viewAngle/2) * Mathf.Deg2Rad);
		float angleNeg = ((anglePersonnage - viewAngle/2) * Mathf.Deg2Rad) ;
		Vector2 pointView  = new Vector2(viewRadius * Mathf.Cos(((anglePos))),viewRadius * Mathf.Sin(((anglePos))));
		Vector2 pointView2 = new Vector2(viewRadius * Mathf.Cos(((angleNeg))),viewRadius * Mathf.Sin(((angleNeg))));

		Gizmos.DrawWireSphere (transform.position,viewRadius);


	}

	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets() {
		visibleTargets.Clear ();
		Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll (transform.position, viewRadius,targetMask);
		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector2 dirToTarget = (target.position - transform.position).normalized;
			float dstToTarget = Vector2.Distance (transform.position, target.position);
			if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
				visibleTargets.Add (target);
			}
		}
	}

    public void Hurt()
    {
        if (!HasItems)
        {
			Debug.Log ("Je suis stun");
            _stunned = true;
			Animator animation = GetComponent<Animator> ();
			animation.SetBool ("stunned", true);
            StartCoroutine(StunRoutine());
        }
    }

	public void touchObject(Item item)
	{
		switch (this.Ennemytype) {
		case Ennemies.animal:
			if (item.type == Item.Type.FOOD && _items.Count != InventorySize) {
				targetMovement = null;
				Destroy (item.gameObject);
				_items.Insert (0,item.element);
				if (_items.Count == InventorySize) {
					flee = true;
				}
			} else if (item.type == Item.Type.WEAPON || item.type == Item.Type.GOLD) {
				if (_items.Count != 0) {
					GameObject itemToPop = Inventory.Instance.instanciateItem (_items [0]);
					_items.RemoveAt (0);
					itemToPop.transform.position = transform.position - transform.right * _dropRadius;
					} else if (_items.Count == 0) {
						Hurt ();
					}
				}
			break;
		case Ennemies.peacefulAnimal:
				if (item.type == Item.Type.FOOD && _items.Count != InventorySize) {
					targetMovement = null;
					Destroy(item.gameObject);
					_items.Insert(0,item.element);
					if(_items.Count == InventorySize){
						flee = true;
					}
			}  else if (item.type == Item.Type.WEAPON || item.type == Item.Type.GOLD || item.type == Item.Type.CRAP) {
					if (_items.Count != 0) {
						GameObject itemToPop = Inventory.Instance.instanciateItem (_items [0]);
						_items.RemoveAt (0);
						itemToPop.transform.position = transform.position - transform.right * _dropRadius;
					} else if (_items.Count == 0) {
						Hurt ();
					}
				}

			break;
		case Ennemies.hero:
			if (item.type == Item.Type.WEAPON && _items.Count != InventorySize) {
				targetMovement = null;
				Destroy(item.gameObject);
				_items.Insert(0,item.element);
				if(_items.Count == InventorySize){
					flee = true;
				}
			}  else if (item.type == Item.Type.CRAP) {
				Animator ennemyAnimation = GetComponent<Animator> ();

				if (_items.Count != 0) {
					ennemyAnimation.SetTrigger ("damaged");
					GameObject itemToPop = Inventory.Instance.instanciateItem (_items [0]);
					_items.RemoveAt (0);
					itemToPop.transform.position = transform.position - transform.right * _dropRadius;
				} else if (_items.Count == 0) {
					Hurt ();
				}
			}

			break;
		default:
			break;
		}
	}

    IEnumerator StunRoutine()
    {
        yield return new WaitForSeconds(_stunTime);
        _stunned = false;
		Animator animation = GetComponent<Animator> ();
		animation.SetBool ("stunned", false);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Item item = collision.gameObject.GetComponent<Item>();
		if (item != null) {
			touchObject (item);
		}
	}

	public void died()
	{
        --(GameManager.Instance.EnemiesCount);
		Destroy (this.gameObject);
	}
}

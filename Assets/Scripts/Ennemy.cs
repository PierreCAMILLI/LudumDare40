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
	// Use this for initialization
	void Start () 
	{
		StartCoroutine ("FindTargetsWithDelay", .2f);

	}
	
	// Update is called once per frame
	void Update () 
	{
		Behavior();
		Move ();
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		if (flee && !sprite.isVisible)
			Destroy (this.gameObject);

	}

	public void Move()
	{
		if (flee) {
			transform.right = transform.position + Player.Instance.transform.position;
			transform.position += transform.right * Time.deltaTime * MoveSpeed*20;
		}
		if (targetMovement != null && !flee)
			moveToTarget (targetMovement);
	}

	void moveToTarget (Transform targetPosition)
	{
		
		transform.right = targetPosition.position - transform.position;
		transform.position += transform.right * Time.deltaTime * MoveSpeed;
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

		Gizmos.DrawLine (transform.position, transform.position + (Vector3)pointView);
		Gizmos.DrawLine (transform.position, transform.position + (Vector3)pointView2);
		Gizmos.DrawLine (transform.position + (Vector3)pointView, transform.position + (Vector3)pointView2);

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
			if (Vector2.Angle (transform.right, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector2.Distance (transform.position, target.position);
				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
					visibleTargets.Add (target);
				}
			}
		}
			
	}

    public void Hurt()
    {
        if (!HasItems)
        {
            _stunned = true;
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
					itemToPop.transform.position = -transform.right;
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
						itemToPop.transform.position = -transform.right;
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
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Item item = collision.gameObject.GetComponent<Item>();
		if (item != null) {
			touchObject (item);
		}
	}

}

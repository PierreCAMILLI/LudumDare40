﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour {

	public enum Ennemies {peacefulAnimal,animal,hero,goblin};
	public Ennemies Ennemytype;
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

	}

	public void Move()
	{
		if (targetMovement != null)
			moveToTarget (targetMovement);
	}

	void moveToTarget (Transform targetPosition)
	{
		
		transform.right = targetPosition.position - transform.position;
		transform.position += transform.right * Time.deltaTime * MoveSpeed;
	}

	void Behavior()
	{
		
		switch (this.Ennemytype) {
		case Ennemies.animal:
			List<GameObject> itmeInVision = new List<GameObject> ();
			if (visibleTargets.Count == 0)
				targetMovement = null;
			
			foreach (var target in visibleTargets) {
				Item item = target.GetComponent<Item> ();
				if (item != null) {
					
				}
				else if (target.GetComponent<Player> () != null) {
					targetMovement = target;
				}
				else
				{
					targetMovement = null;
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
		Debug.Log (anglePersonnage);
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

		if (visibleTargets.Count == 0)
			Debug.Log ("I see nobody");
		else
			Debug.Log ("I see " + visibleTargets.Count + " things");
			
	}


}

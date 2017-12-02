using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour {

    [SerializeField]
    Transform _followingObject;

    [SerializeField]
    Bounds1D _zoneWidth, _zoneHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateGetFollowing();

        Vector3 viewportPoint = Camera.current.WorldToViewportPoint(_followingObject.position);
	}

    void UpdateGetFollowing()
    {
        if (_followingObject == null && Player.Instance != null)
            _followingObject = Player.Instance.transform;
    }
}

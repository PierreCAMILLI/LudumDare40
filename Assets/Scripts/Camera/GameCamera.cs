using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour {

    [SerializeField]
    Transform _followingObject;

    [SerializeField]
    Vector2 _velocity;

    [SerializeField]
    float _smoothTime;

    [SerializeField]
    float _zoomCoefficient = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateFollowing();
        
        
	}

    void UpdateFollowing()
    {
        // Handle get object to follow
        if (_followingObject == null && Player.Instance != null)
            _followingObject = Player.Instance.transform;
        if (_followingObject == null)
            return;

        // Handle camera movement
        Vector3 newPosition = Vector2.SmoothDamp(transform.position, _followingObject.position, ref _velocity, _smoothTime, Mathf.Infinity, Time.deltaTime);
        newPosition.z = transform.position.z;
        transform.position = newPosition;

        // Handle zoom
        Player player = _followingObject.GetComponent<Player>();
        if(player != null)
        {
            Camera.main.orthographicSize = _zoomCoefficient * player.Size;
        }
    }
}

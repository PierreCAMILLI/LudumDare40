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
	void LateUpdate () {
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
        Vector2 projectPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f,0.5f));
        Vector3 diff = transform.position - (Vector3) projectPosition;
        Vector3 newPosition = Vector2.SmoothDamp(projectPosition, _followingObject.position, ref _velocity, _smoothTime, Mathf.Infinity, Time.deltaTime);
        transform.position = newPosition + diff;

        // Handle zoom
        Player player = _followingObject.GetComponent<Player>();
        if(player != null)
        {
            Camera.main.orthographicSize = _zoomCoefficient * player.SizeTarget;
        }

    }
}

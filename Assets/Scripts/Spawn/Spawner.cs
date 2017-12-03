using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private static HashSet<Spawner> _spawners;
    public IList<Spawner> Spawners {
        get { return _spawners.ToList(); }
    }

    private void Awake()
    {
        if (_spawners == null)
            _spawners = new HashSet<Spawner>();
        _spawners.Add(this);
    }

    private void OnEnable()
    {
        _spawners.Add(this);
    }

    private void OnDisable()
    {
        _spawners.Remove(this);
    }

    private void OnDestroy()
    {
        _spawners.Remove(this);
    }

    public GameObject Spawn(GameObject prefab)
    {
        return Instantiate(prefab, transform.position, Quaternion.identity, null);
    }

}

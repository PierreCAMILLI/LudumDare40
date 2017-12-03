using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private static HashSet<Spawner> _spawners;
    public static IList<Spawner> Spawners {
        get { return _spawners != null ? _spawners.ToArray() : null; }
    }

    private Ennemy _lastEnemySpawned = null;
    public Ennemy LastEnemySpawned
    {
        get { return _lastEnemySpawned; }
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
        GameObject go = Instantiate(prefab, transform.position, Quaternion.identity, null);
        _lastEnemySpawned = go.GetComponent<Ennemy>();
        return go;
    }

}

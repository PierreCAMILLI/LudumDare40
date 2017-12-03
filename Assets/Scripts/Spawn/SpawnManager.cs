using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : SingletonBehaviour<SpawnManager> {

    Queue<GameObject> _toSpawn;
    Queue<Spawner> _shuffledSpawners;

    [SerializeField]
    private float _timeNextSpawn = 5f;
    public float TimeNextSpawn
    {
        get { return _timeNextSpawn; }
        set { _timeNextSpawn = value; }
    }

    void Awake()
    {
        base.Awake();
        _toSpawn = new Queue<GameObject>();
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnRoutine());
	}

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (_toSpawn.Count <= 0)
            {
                yield return null;
                continue;
            }
            yield return new WaitForSeconds(_timeNextSpawn);
            if (_shuffledSpawners == null || _shuffledSpawners.Count <= 0)
                ShuffleSpawners();
            if(_shuffledSpawners != null && _toSpawn != null)
                _shuffledSpawners.Dequeue().Spawn(_toSpawn.Dequeue());
        }
    }

    /// <summary>
    /// Shuffle spawners and put em in queue
    /// </summary>
    void ShuffleSpawners()
    {
        if (Spawner.Spawners == null)
            return;
        System.Random rnd = new System.Random();
        Spawner[] spawners = Spawner.Spawners.OrderBy(c => rnd.Next()).ToArray();
        
        _shuffledSpawners = new Queue<Spawner>();
        foreach(Spawner spawner in spawners)
        {
            _shuffledSpawners.Enqueue(spawner);
        }
    }

    public void Add(EnemyWave wave)
    {
        Add(wave.Enemies.ToArray());
    }

    public void Add(GameObject[] prefabs)
    {
        foreach (GameObject prefab in prefabs)
            _toSpawn.Enqueue(prefab);
    }
}

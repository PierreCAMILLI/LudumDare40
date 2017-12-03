using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager> {

    private byte _level = 0;
    public byte Level
    {
        get { return _level; }
        set { _level = value; }
    }

    private int _enemiesCount = 0;
    public int EnemiesCount
    {
        get { return _enemiesCount; }
        set { _enemiesCount = value; }
    }

    void OnStartLevel(byte levelNumber)
    {
        _level = levelNumber;

        _enemiesCount = WaveManager.Instance.Waves[levelNumber].Enemies.Count;
        SpawnManager.Instance.Add(WaveManager.Instance.Waves[levelNumber]);
    }
	
	// Update is called once per frame
	void Update () {
		if(_enemiesCount <= 0)
        {
            OnStartLevel(_level++);
        }
	}
}

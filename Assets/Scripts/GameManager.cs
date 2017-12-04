using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager> {

    private int _level = -1;
    public int Level
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

    /// <summary>
    /// Indicates the amount of loop there have been for every waves
    /// </summary>
    public int WavesLoop
    {
        get { return Mathf.FloorToInt(_level / WaveManager.Instance.Waves.Count); }
    }

    void OnStartLevel(int levelNumber)
    {
        _level = levelNumber;

        int index = levelNumber % WaveManager.Instance.Waves.Count;
        _enemiesCount = (WavesLoop + 1) * WaveManager.Instance.Waves[index].Enemies.Count;

        Debug.Log("level : " + levelNumber);
        Debug.Log("wave count : " + WavesLoop);
        Debug.Log("ennemui count : " + _enemiesCount);

        for(int i = 0; i < WavesLoop + 1; i++)
            SpawnManager.Instance.Add(WaveManager.Instance.Waves[index]);
    }
	
	// Update is called once per frame
	void Update () {
		if(_enemiesCount <= 0)
        {
            OnStartLevel(++_level);
        }
	}
}

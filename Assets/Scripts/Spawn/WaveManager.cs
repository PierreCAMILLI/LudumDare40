using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : SingletonBehaviour<WaveManager> {

    [SerializeField]
    private List<EnemyWave> _waves;
    public IList<EnemyWave> Waves
    {
        get { return _waves; }
    }

}

[System.Serializable]
public class EnemyWave
{
    [SerializeField]
    private List<GameObject> _enemies;
    public IList<GameObject> Enemies
    {
        get { return _enemies; }
    }
}

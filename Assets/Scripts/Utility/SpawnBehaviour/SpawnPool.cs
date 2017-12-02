using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pool for spawnable objects
/// </summary>
/// <typeparam name="T">Type of the spawnable object derived from SpawnBehaviour</typeparam>
public class SpawnPool<T> where T : SpawnBehaviour<T>
{

    #region Private Variables
    // Prefab of the spawned object
    T _prefab;

    // Queue of deactivated spawning objects
    Queue<T> _pool = new Queue<T>();
    // List of popped objects
    // Remark: It might be better to use a LinkedList if the removed objects aren't removed in order
    List<T> _spawned = new List<T>();
    #endregion

    #region Public Variables
    public bool destroyOnUnspawn = false;
    #endregion

    #region Getters
    /// <summary>
    /// Spawned objects
    /// </summary>
    public IList<T> Spawned { get { return _spawned; } }

    /// <summary>
    /// Number of objects in pool
    /// </summary>
    public int PoolCount { get { return _pool.Count; } }
    #endregion

    public SpawnPool(T prefab)
    {
        _prefab = prefab;
    }

    #region Instantiate Functions
    T _Instantiate(Transform parent = null)
    {
        T instance;
        // Retrieve new instance of popping object
        if (_pool.Count <= 0)
            instance = GameObject.Instantiate(_prefab);
        else
            instance = _pool.Dequeue();

        // Add the instance of the object to the list of popped object
        _spawned.Add(instance);
        instance.SetPool(this);

        // Set the object active
        instance.gameObject.SetActive(true);

        return instance;
    }

    public T Instantiate(Transform parent = null, bool instantiateInWorldSpace = false)
    {
        T instance = _Instantiate();

        // Set parent of the object
        Transform instTransform = instance.transform;
        instTransform.parent = parent;

        // Set transform coordinates of the object
        Transform prefTransform = _prefab.transform;
        if (instantiateInWorldSpace)
        {
            instTransform.position = prefTransform.position;
            instTransform.rotation = prefTransform.rotation;
        }
        else
        {
            instTransform.position = prefTransform.localPosition;
            instTransform.rotation = prefTransform.localRotation;
        }
        instTransform.localScale = prefTransform.localScale;

        // Call the on spawn function
        instance.OnSpawn();

        return instance;
    }

    public T Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        T instance = _Instantiate();

        // Set parent of the object
        Transform instTransform = instance.transform;
        instTransform.parent = parent;

        // Set transform coordinates of the object
        instTransform.position = position;
        instTransform.rotation = rotation;

        // Call the on spawn function
        instance.OnSpawn();

        return instance;
    }
    #endregion

    #region Unspawn
    // Handle unspawn of object
    internal void Unspawn(SpawnBehaviour<T> instance)
    {
        // Remove object from spawned objects list
        T _instance = (T)instance;
        if (destroyOnUnspawn)
            GameObject.Destroy(_instance.gameObject);
        else if (_spawned.Remove(_instance))
        {
            // Put object in pool and activate it
            _pool.Enqueue(_instance);
            _instance.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Unspawn every spawned objects
    /// </summary>
    public void UnspawnAllObjects()
    {
        for (int i = _spawned.Count - 1; i >= 0; --i)
        {
            _pool.Enqueue(_spawned[i]);
            _spawned.RemoveAt(i);
        }
    }
    #endregion

    #region Destroy
    /// <summary>
    /// Destroy every gameobject in pool
    /// </summary>
    public void DestroyPool()
    {
        while (_pool.Count > 0)
            GameObject.Destroy(_pool.Dequeue());
    }

    /// <summary>
    /// Destroy every spawned gameobject
    /// </summary>
    public void DestroySpawned()
    {
        for (int i = _spawned.Count - 1; i >= 0; --i)
        {
            GameObject.Destroy(_spawned[i]);
            _spawned.RemoveAt(i);
        }
    }
    #endregion

    public void ResizePool(int size)
    {
        while (_pool.Count > 0 && _pool.Count > size)
            GameObject.Destroy(_pool.Dequeue());
    }
}

/// <summary>
/// Base class from spawnable MonoBehaviour
/// </summary>
/// <typeparam name="T">Type of the spawnable MonoBehaviour</typeparam>
public class SpawnBehaviour<T> : MonoBehaviour where T : SpawnBehaviour<T>
{
    SpawnPool<T> _pool;
    /// <summary>
    /// Pool associated to this spawnable object
    /// </summary>
    public SpawnPool<T> Pool { get { return _pool; } }

    /// <summary>
    /// Function called when the object is spawning in the scene
    /// </summary>
    public virtual void OnSpawn() { }

    /// <summary>
    /// Unspawn the object
    /// </summary>
    public void Unspawn()
    {
        _pool.Unspawn(this);
    }

    internal void SetPool(SpawnPool<T> pool)
    {
        _pool = pool;
    }
}

public class SpawnPool
{

    #region Private Variables
    // Prefab of the spawned object
    SpawnBehaviour _prefab;

    // Queue of deactivated spawning objects
    Queue<SpawnBehaviour> _pool = new Queue<SpawnBehaviour>();
    // List of popped objects
    // Remark: It might be better to use a LinkedList if the removed objects aren't removed in order
    List<SpawnBehaviour> _spawned = new List<SpawnBehaviour>();
    #endregion

    #region Public Variables
    public bool destroyOnUnspawn = false;
    #endregion

    #region Getters
    /// <summary>
    /// Spawned objects
    /// </summary>
    public IList<SpawnBehaviour> Spawned { get { return _spawned; } }

    /// <summary>
    /// Number of objects in pool
    /// </summary>
    public int PoolCount { get { return _pool.Count; } }
    #endregion

    public SpawnPool(SpawnBehaviour prefab)
    {
        _prefab = prefab;
    }

    #region Instantiate Functions
    T _Instantiate<T>(Transform parent = null) where T : SpawnBehaviour
    {
        SpawnBehaviour instance;
        // Retrieve new instance of popping object
        if (_pool.Count <= 0)
            instance = GameObject.Instantiate(_prefab);
        else
            instance = _pool.Dequeue();

        // Add the instance of the object to the list of popped object
        _spawned.Add(instance);
        instance.SetPool(this);

        // Set the object active
        instance.gameObject.SetActive(true);

        return (T) instance;
    }

    public T Instantiate<T>(Transform parent = null, bool instantiateInWorldSpace = false) where T : SpawnBehaviour
    {
        T instance = _Instantiate<T>();

        // Set parent of the object
        Transform instTransform = instance.transform;
        instTransform.parent = parent;

        // Set transform coordinates of the object
        Transform prefTransform = _prefab.transform;
        if (instantiateInWorldSpace)
        {
            instTransform.position = prefTransform.position;
            instTransform.rotation = prefTransform.rotation;
        }
        else
        {
            instTransform.position = prefTransform.localPosition;
            instTransform.rotation = prefTransform.localRotation;
        }
        instTransform.localScale = prefTransform.localScale;

        // Call the on spawn function
        instance.OnSpawn();

        return instance;
    }

    public T Instantiate<T>(Vector3 position, Quaternion rotation, Transform parent = null) where T : SpawnBehaviour
    {
        T instance = _Instantiate<T>();

        // Set parent of the object
        Transform instTransform = instance.transform;
        instTransform.parent = parent;

        // Set transform coordinates of the object
        instTransform.position = position;
        instTransform.rotation = rotation;

        // Call the on spawn function
        instance.OnSpawn();

        return instance;
    }
    #endregion

    #region Unspawn
    // Handle unspawn of object
    internal void Unspawn(SpawnBehaviour instance)
    {
        // Remove object from spawned objects list
        if (destroyOnUnspawn)
            GameObject.Destroy(instance.gameObject);
        else if (_spawned.Remove(instance))
        {
            // Put object in pool and activate it
            _pool.Enqueue(instance);
            instance.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Unspawn every spawned objects
    /// </summary>
    public void UnspawnAllObjects()
    {
        for (int i = _spawned.Count - 1; i >= 0; --i)
        {
            _pool.Enqueue(_spawned[i]);
            _spawned.RemoveAt(i);
        }
    }
    #endregion

    #region Destroy
    /// <summary>
    /// Destroy every gameobject in pool
    /// </summary>
    public void DestroyPool()
    {
        while (_pool.Count > 0)
            GameObject.Destroy(_pool.Dequeue());
    }

    /// <summary>
    /// Destroy every spawned gameobject
    /// </summary>
    public void DestroySpawned()
    {
        for (int i = _spawned.Count - 1; i >= 0; --i)
        {
            GameObject.Destroy(_spawned[i]);
            _spawned.RemoveAt(i);
        }
    }
    #endregion

    public void ResizePool(int size)
    {
        while (_pool.Count > 0 && _pool.Count > size)
            GameObject.Destroy(_pool.Dequeue());
    }
}

/// <summary>
/// Base class from spawnable MonoBehaviour
/// </summary>
/// <typeparam name="T">Type of the spawnable MonoBehaviour</typeparam>
public class SpawnBehaviour : MonoBehaviour
{
    SpawnPool _pool;
    /// <summary>
    /// Pool associated to this spawnable object
    /// </summary>
    public SpawnPool Pool { get { return _pool; } }

    /// <summary>
    /// Function called when the object is spawning in the scene
    /// </summary>
    public virtual void OnSpawn() { }

    /// <summary>
    /// Unspawn the object
    /// </summary>
    public void Unspawn()
    {
        _pool.Unspawn(this);
    }

    internal void SetPool(SpawnPool pool)
    {
        _pool = pool;
    }
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 재사용 가능한 오브젝트를 관리하는 범용 오브젝트 풀러입니다.
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    /// <summary>
    /// 인스펙터에서 설정할 개별 오브젝트 풀 정보입니다.
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    // 싱글톤 인스턴스
    public static ObjectPooler Instance { get; private set; }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 정의된 각 풀에 대해 오브젝트를 미리 생성하여 큐에 저장합니다.
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// 풀에서 오브젝트를 가져와 지정된 위치와 회전값으로 활성화합니다.
    /// </summary>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning($"Pool with tag '{tag}' is exhausted. Consider increasing the pool size.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    /// <summary>
    /// 사용이 끝난 오브젝트를 비활성화하고 다시 풀로 반환합니다.
    /// </summary>
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist. Destroying object instead.");
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }
}
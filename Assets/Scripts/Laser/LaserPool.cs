using UnityEngine;
using UnityEngine.Pool;

public class LaserPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxPoolSize = 20;
    [SerializeField] private bool collectionChecks = true;

    public IObjectPool<GameObject> laser_pool { get; private set; }

    private void Awake()
    {
        laser_pool = new ObjectPool<GameObject>(CreateItem, OnGet, OnRelease, OnDestroyItem, collectionChecks, 10, maxPoolSize);
    }

    private GameObject CreateItem()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        obj.GetComponent<Laser>().SetPool(this);
        return obj;
    }

    private void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyItem(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject obj = laser_pool.Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        laser_pool.Release(obj);
    }
}
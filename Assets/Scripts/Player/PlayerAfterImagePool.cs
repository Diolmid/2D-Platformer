using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField] private GameObject _afterImagePrefab;

    
    private Queue<GameObject> _availableObject = new Queue<GameObject>();

    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(_afterImagePrefab, transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        _availableObject.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if(_availableObject.Count == 0)
            GrowPool();

        var instance = _availableObject.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
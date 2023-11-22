using ADV.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ADVPoolManager
{
    /*public Dictionary<string, ADVPoolData> Pools = new();

    private GameObject PoolsHolder;

    public ADVPoolManager(Action<ADVBaseManager> onComplete) : base(onComplete)
    {
        PoolsHolder = new GameObject("PoolsHolder");
        UnityEngine.Object.DontDestroyOnLoad(PoolsHolder);

        OnInitComplete();
    }

    public async void InitPool<T>(string originalName, int amount) where T : Component
    {
        var generateObjects = await ADVGameManager.Instance.FactoryManager.GenerateObjects<T>(originalName, amount);

        if (generateObjects == null || !generateObjects.Any())
        {
            Manager.MonitorManager.ReportException("Failed to generate objects.");
            return;
        }

        var poolHolder = new GameObject($"Pool_{originalName}");
        poolHolder.transform.SetParent(PoolsHolder.transform);

        Pools[originalName] = new ADVPoolData(generateObjects.ToArray(), poolHolder);
    }

    public T GetFromPool<T>(string poolName) where T : Component
    {
        if (!Pools.ContainsKey(poolName))
        {
            Manager.MonitorManager.ReportException($"No pool initialized found with the name {poolName}");
            return null;
        }

        //Check if available, if not generate only 1, add it and later return it
        //if (!Pools[poolName].AvailableItems.Any())
        //{
        //    Manager.MonitorManager.ReportException($"No available items in the pool {poolName}");
        //    var generateObject = ADVManager.Instance.FactoryManager.GenerateObject<T>(poolName);
        //    Pools[poolName].AddNewToPool(generateObject);
        //}

        //Returning poolable
        var availableItem = Pools[poolName].AvailableItems[0];
        Pools[poolName].AvailableItems.Remove(availableItem);
        Pools[poolName].UnAvailableItems.Add(availableItem);

        availableItem.gameObject.SetActive(true);
        return (T)availableItem;
    }

    public void ReturnToPool<T>(string poolName, T returnedObject) where T : Component
    {
        Pools[poolName].AvailableItems.Add(returnedObject);
        Pools[poolName].UnAvailableItems.Remove(returnedObject);

        returnedObject.gameObject.SetActive(false);
    }
}

public class ADVPoolData
{
    public List<Component> TotalItems;
    public List<Component> AvailableItems;
    public List<Component> UnAvailableItems;

    public GameObject PoolHolder;

    public ADVPoolData(Component[] generateObjects, GameObject poolHolder)
    {
        TotalItems = generateObjects.ToList();
        AvailableItems = generateObjects.ToList();
        UnAvailableItems = new List<Component>();

        PoolHolder = poolHolder;

        foreach (var generateObject in generateObjects)
        {
            generateObject.transform.SetParent(PoolHolder.transform);
            generateObject.gameObject.SetActive(false);
        }

    }

    public void AddNewToPool<T>(T generateObject) where T : Component
    {
        TotalItems.Add(generateObject);
        AvailableItems.Add(generateObject);

        generateObject.transform.SetParent(PoolHolder.transform);
        generateObject.gameObject.SetActive(false);
    }
}*/
}

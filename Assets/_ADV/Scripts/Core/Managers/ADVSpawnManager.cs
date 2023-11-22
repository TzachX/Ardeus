using System;
using System.Collections.Generic;
using UnityEngine;

public class ADVFactoryManager : ADVBaseManager
{
    public ADVFactoryManager(Action<ADVBaseManager> onComplete) : base(onComplete)
    {
        OnInitComplete();
    }

    public T[] CreateObjects<T>(string originalName, int amount, Transform parent) where T : Component
    {
        var created = new List<T>();

        for (var i = 0; i < amount; i++)
        {
            var generated = CreateObject<T>(originalName, parent);
            created.Add(generated);
        }
        return created.ToArray();
    }

    public T CreateObject<T>(string originalName, Transform parent) where T : Component
    {
        var original = Resources.Load<T>(originalName);
        return UnityEngine.Object.Instantiate(original, parent);
    }
}

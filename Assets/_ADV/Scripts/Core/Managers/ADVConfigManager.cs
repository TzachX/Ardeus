using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine;

public class ADVConfigManager : ADVBaseManager
{
    private readonly Dictionary<string, ADVBaseConfig> cachedValues = new();

    public ADVConfigManager(Action<ADVBaseManager> onComplete) : base(onComplete)
    {
        SetDefault();
    }

    private void SetDefault()
    {
        Dictionary<string, object> defaults = new();

        defaults.Add("CardList", "");

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task =>
        {
            OnDefaultComplete();
        });
    }

    private void OnDefaultComplete()
    {
        FetchConfigs();
    }

    private void FetchConfigs()
    {
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(task => { SyncConfigs(); });
    }

    private void SyncConfigs()
    {
        FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task => OnSyncComplete());
    }

    private void OnSyncComplete()
    {
        OnInitComplete();
    }

    public T GetConfig<T>() where T : ADVBaseConfig
    {
        var configName = typeof(T).Name;

        if (!cachedValues.TryGetValue(configName, out var jsonResult))
        {
            var configResult = FirebaseRemoteConfig.DefaultInstance.GetValue(configName);
            jsonResult = cachedValues[configName] = JsonConvert.DeserializeObject<T>(configResult.StringValue);
            Debug.Log($"GetConfig of {configName} is {configResult.StringValue}");
        }

        return (T)jsonResult;
    }
}

[Serializable]
public class ADVBaseConfig
{
    public bool IsEnabled = true;
    public int ConfigVersion = 1;
}

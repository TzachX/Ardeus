using System;
using System.Collections.Generic;
using System.Linq;

public class ADVEventManager : ADVBaseManager
{
    public Dictionary<ADVEventType, ADVEventListenersData> ListenersData = new();

    public ADVEventManager(Action<ADVBaseManager> onComplete) : base(onComplete)
    {
        OnInitComplete();
    }

    public void AddListener(ADVEventType eventType, Action<object> additionalData)
    {
        if (ListenersData.TryGetValue(eventType, out var value))
        {
            value.ActionsOnInvoke.Add(additionalData);
        }
        else
        {
            ListenersData[eventType] = new ADVEventListenersData(additionalData);
        }
    }

    public void RemoveListener(ADVEventType eventType, Action<object> actionToRemove)
    {
        if (!ListenersData.TryGetValue(eventType, out var value))
        {
            return;
        }

        value.ActionsOnInvoke.Remove(actionToRemove);

        if (!value.ActionsOnInvoke.Any())
        {
            ListenersData.Remove(eventType);
        }
    }

    public void InvokeADVEvent(ADVEventType eventType, object dataToInvoke)
    {
        if (!ListenersData.TryGetValue(eventType, out var value))
        {
            return;
        }

        foreach (var method in value.ActionsOnInvoke)
        {
            try
            {
                method.Invoke(dataToInvoke);
            }
            catch (Exception e)
            {
                //Manager.MonitorManager.ReportException(e);
            }
        }
    }
}

public enum ADVEventType
{
    CardChoiceMade,
    RevealNewCard,
    ResourceOver,
    Showresponse,
    HideIndicator,
    HideText
}

public class ADVEventListenersData
{
    public List<Action<object>> ActionsOnInvoke;

    public ADVEventListenersData(Action<object> additionalData)
    {
        ActionsOnInvoke = new List<Action<object>>
            {
                additionalData
            };
    }
}

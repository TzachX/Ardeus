using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ADVResourceManager : ADVBaseManager
{
    private Dictionary<ResourceType, int> resourcesValues;

    public ADVResourceManager(Action<ADVBaseManager> onComplete) : base(onComplete)
    {
        Manager.EventManager.AddListener(ADVEventType.CardChoiceMade, UpdateResourcesValue);
        resourcesValues = new Dictionary<ResourceType, int>();

        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            resourcesValues.Add(resource, 50);
        }

        OnInitComplete();
    }

    private void UpdateResourcesValue(object responseObj)
    {
        CardResponse response = (CardResponse)responseObj;

        foreach (ResourceType resource in response.effects.Keys)
        {
            resourcesValues[resource] += response.effects[resource];

            if (resourcesValues[resource] <= 0)
            {
                Manager.CardManager.SetLossCard(resource);
                return;
            }
        }

        Manager.CardManager.SetNextCard();
    }

    public override void ResetStats()
    {
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            resourcesValues[resource] = 50;
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class CardResponse
{
    [JsonProperty("Content")]
    public string content;

    [JsonProperty("Effects")]
    public Dictionary<ResourceType, int> effects;
}

public enum ResourceType
{
    World,
    Nation,
    Army,
    Economy
}
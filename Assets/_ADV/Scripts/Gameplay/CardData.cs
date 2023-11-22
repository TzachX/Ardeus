using Newtonsoft.Json;
using System;

public class CardData
{
    [JsonProperty("Advice")]
    public string advice;

    [JsonProperty("Responses")]
    public Tuple<CardResponse, CardResponse> responses;

    [JsonProperty("CardViewID")]
    public CardViewID cardViewID;
}

public class CardView
{
    [JsonProperty("Name")]
    public string name;

    [JsonProperty("SpriteName")]
    public string spriteName;

    [JsonProperty("CardViewID")]
    public CardViewID cardViewID;
}

public enum CardViewID
{
    MinisterDefense,
    MinisterCommerce,
    MinisterInternal,
    MinisterForeign,
    EndWorld,
    EndNation,
    EndArmy,
    EndEconomy
}

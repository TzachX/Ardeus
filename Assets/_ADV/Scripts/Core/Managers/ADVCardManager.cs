using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ADVCardManager : ADVBaseManager
{
    public List<CardData> cards;
    public readonly Dictionary<ResourceType, CardData> lossCards;
    private readonly List<CardView> cardViews;
    public readonly Dictionary<CardViewID, CardView> IDToCardViews;
    public readonly Dictionary<CardViewID, Sprite> characterSprites;
    public bool wasEndCardDrawn;
    private CardData nextCard;
    private int index;

    public ADVCardManager(Action<ADVBaseManager> onComplete) : base(onComplete)
    {
        IDToCardViews = new Dictionary<CardViewID, CardView>();
        characterSprites = new Dictionary<CardViewID, Sprite>();
        lossCards = new Dictionary<ResourceType, CardData>();

        ADVCardConfig cardConfig = Manager.ConfigManager.GetConfig<ADVCardConfig>();
        cards = cardConfig.cards;

        foreach (var endCard in cardConfig.endCards)
        {
            switch (endCard.cardViewID)
            {
                case CardViewID.EndWorld:
                    lossCards[ResourceType.World] = endCard;
                    break;
                case CardViewID.EndNation:
                    lossCards[ResourceType.Nation] = endCard;
                    break;
                case CardViewID.EndArmy:
                    lossCards[ResourceType.Army] = endCard;
                    break;
                case CardViewID.EndEconomy:
                    lossCards[ResourceType.Economy] = endCard;
                    break;
            };
        }

        cardViews = Manager.ConfigManager.GetConfig<ADVCardViewConfig>().cardViews;

        foreach (CardView cardView in cardViews)
        {
            IDToCardViews[cardView.cardViewID] = cardView;
            characterSprites[cardView.cardViewID] = Resources.Load<Sprite>(cardView.spriteName);
        }

        ResetStats();
        OnInitComplete();
    }

    public CardData GetNextCard()
    {
        return nextCard;
    }

    public void SetNextCard()
    {
        index = (index + 1) % cards.Count;
        nextCard = cards[index];
    }

    public void SetLossCard(ResourceType emptyResource)
    {
        nextCard = lossCards[emptyResource];
        wasEndCardDrawn = true;
    }

    public override void ResetStats()
    {
        index = 0;
        wasEndCardDrawn = false;
        nextCard = cards[index];
        Manager.isGameOver = false;
    }
}

public class ADVCardConfig : ADVBaseConfig
{
    [JsonProperty("Cards")]
    public List<CardData> cards { get; set; }

    [JsonProperty("End Cards")]
    public List<CardData> endCards { get; set; }
}

public class ADVCardViewConfig : ADVBaseConfig
{
    [JsonProperty("CardViews")]
    public List<CardView> cardViews { get; set; }
}

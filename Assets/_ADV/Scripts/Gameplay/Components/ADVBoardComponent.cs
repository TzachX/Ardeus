using ADV.Core.Components;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ADVBoardComponent : ADVMonoBehaviour
{
    [SerializeField] private Canvas cardHolder;
    [SerializeField] private TextMeshProUGUI adviceText;
    [SerializeField] private TextMeshProUGUI charNameText;

    private ADVCardComponent downCard;
    private ADVCardComponent upCard;

    void Awake()
    {
        Manager.EventManager.AddListener(ADVEventType.RevealNewCard, RevealNextCard);
        Manager.EventManager.AddListener(ADVEventType.HideText, HideCardText);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCardsAnimation(10);
        //CreateFirstCards();
    }

    private void StartCardsAnimation(int cardAmount)
    {
        RectTransform canvasRect;
        ADVCardComponent currCard;
        RectTransform cardRect;

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < cardAmount; i++)
        {
            canvasRect = GetComponent<RectTransform>();
            currCard = Manager.FactoryManager.CreateObject<ADVCardComponent>("Card", cardHolder.transform);
            cardRect = currCard.GetComponent<RectTransform>();
            currCard.transform.localPosition = new Vector3(-canvasRect.rect.width / 2 - cardRect.rect.width / 2, canvasRect.rect.height / 2 + cardRect.rect.height / 2, 0);
            StartCoroutine(DestroyAfterSec(currCard.gameObject, cardAmount));
            seq.PrependInterval(0.15f);
            seq.Join(currCard.transform.DOLocalMove(currCard.InitialPosition, 0.4f));
        }

        seq.OnComplete(() => { CreateFirstCards(); });
    }

    private IEnumerator DestroyAfterSec(GameObject gameObject, int i)
    {
        yield return new WaitForSeconds(0.5f + (0.15f * i));
        Destroy(gameObject);
    }

    private void CreateFirstCards()
    {
        upCard = Manager.FactoryManager.CreateObject<ADVCardComponent>("Card", cardHolder.transform);
        downCard = Manager.FactoryManager.CreateObject<ADVCardComponent>("Card", cardHolder.transform);
        OrderCards();
        PresentCardData();
    }

    private void OrderCards()
    {
        upCard.Portrait.canvas.sortingOrder = 2;
        downCard.Portrait.canvas.sortingOrder = 1;
    }

    private void RevealNextCard(object cardObj)
    {
        upCard = downCard;
        downCard = Manager.FactoryManager.CreateObject<ADVCardComponent>("Card", cardHolder.transform);
        OrderCards();
        PresentCardData();
    }

    private void PresentCardData()
    {
        CardData currCard = Manager.CardManager.GetNextCard();
        
        adviceText.text = currCard.advice;
        charNameText.text = Manager.CardManager.IDToCardViews[currCard.cardViewID].name;
        upCard.transform.DORotate(new Vector3(0, -90, 0), 0.5f).OnComplete(() => 
        { 
            upCard.SetCardData(currCard);
            upCard.transform.DORotate(Vector3.zero, 0.5f);
            ToggleCardText(1f, 1f);
            upCard.SetInteractable(true);

            if (Manager.CardManager.wasEndCardDrawn) 
            {
                Manager.isGameOver = true;
            }
        });
        
    }

    private void HideCardText(object obj)
    {
        float duration = (float)obj;
        ToggleCardText(0, duration);
    }

    private void ToggleCardText(float alphaValue, float duration) 
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(charNameText.DOFade(alphaValue, duration));
        seq.Join(adviceText.DOFade(alphaValue, duration));
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
using ADV.Core.Components;
using DG.Tweening;
using ADV.Core.Loader;
using UnityEngine.SceneManagement;

public class ADVCardComponent : ADVMonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private string rightCardResponse;
    [SerializeField] private string leftCardResponse;
    [SerializeField] private TextMeshProUGUI cardText;
    [SerializeField] private Image cardTextMask;
    [SerializeField] private AnimationCurve returnEaseCurve;
    [SerializeField] private CanvasGroup canvasGroup;
    private float normalizedRotation;
    private float normalizedAlpha;
    private float distanceMoved;
    private float diagonal;
    private Vector2 throwCard;
    private Vector2 initialPosition;
    private Sequence endDragSequence;
    [SerializeField] private CardData cardData;
    [SerializeField] private Image portrait;
    private Color currColor;
    private bool isInteractable;

    public Image Portrait { get => portrait; set => portrait = value; }
    public Vector2 InitialPosition { get => initialPosition; set => initialPosition = value; }

    private void Awake()
    {
        isInteractable = false;
        InitialPosition = transform.localPosition;
        Vector2 size = rectTransform.sizeDelta;
        diagonal = Mathf.Sqrt(Mathf.Pow(size.x, 2) + Mathf.Pow(size.y, 2));
        currColor = Color.white;
    }

    public void SetInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
    }

    public void SetCardData(CardData cardData)
    {
        rightCardResponse = cardData.responses.Item1.content;
        leftCardResponse  = cardData.responses.Item2.content;
        portrait.sprite = Manager.CardManager.characterSprites[cardData.cardViewID];
        this.cardData = cardData;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (isInteractable)
        {
            rectTransform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
            normalizedRotation = Mathf.Clamp((rectTransform.localPosition.x - InitialPosition.x) / (Screen.width / 2f), -1f, 1f);
            normalizedAlpha = Mathf.Clamp((rectTransform.localPosition.x - InitialPosition.x) / (Screen.width / 4f), -0.75f, 0.75f);
            rectTransform.transform.rotation = Quaternion.Euler(0f, 0f, normalizedRotation * -10f);
            currColor.a = Mathf.Abs(normalizedAlpha);
            cardTextMask.color = currColor;
            cardText.color = currColor;

            // Right swipe
            if (transform.localPosition.x > 0)
            {
                cardText.alignment = TextAlignmentOptions.Left;
                cardText.text = rightCardResponse;
                Manager.EventManager.InvokeADVEvent(ADVEventType.Showresponse, new Tuple<Color, CardResponse>(currColor, cardData.responses.Item1));
            }

            // Left swipe
            else if (transform.localPosition.x < 0)
            {
                cardText.alignment = TextAlignmentOptions.Right;
                cardText.text = leftCardResponse;
                Manager.EventManager.InvokeADVEvent(ADVEventType.Showresponse, new Tuple<Color, CardResponse>(currColor, cardData.responses.Item2));
            }
            // Center
            else
            {
                
                Manager.EventManager.InvokeADVEvent(ADVEventType.HideIndicator, null);
            }
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        distanceMoved = transform.localPosition.x - InitialPosition.x;

        // Choice made
        if (Mathf.Abs(distanceMoved) > Screen.width / 4f)
        {
            endDragSequence = DOTween.Sequence();

            // Right
            if(distanceMoved > 0)
            {
                Manager.EventManager.InvokeADVEvent(ADVEventType.CardChoiceMade, cardData.responses.Item1);
                throwCard = new Vector2((Screen.width / 2f) + (diagonal / 2f), transform.localPosition.y);
            }
            // Left
            else
            {
                Manager.EventManager.InvokeADVEvent(ADVEventType.CardChoiceMade, cardData.responses.Item2);
                throwCard = new Vector2(-1 * ((Screen.width / 2f) + (diagonal / 2f)), transform.localPosition.y);
            }

            Manager.EventManager.InvokeADVEvent(ADVEventType.HideText, 0.3f);
            endDragSequence.Join(transform.DOLocalMove(throwCard, 0.3f).SetEase(returnEaseCurve));
            endDragSequence.Join(cardTextMask.DOFade(0, 0.1f));
            endDragSequence.Join(cardText.DOFade(0, 0.1f));
            endDragSequence.OnComplete(() => 
            {
                if (Manager.isGameOver) 
                {
                    Manager.CardManager.ResetStats();
                    Manager.ResourceManager.ResetStats();
                    Manager.isGameOver = false;
                    SceneManager.LoadScene("MainGameScene");
                }
                else
                {
                    Manager.EventManager.InvokeADVEvent(ADVEventType.RevealNewCard, null);
                }

                
                Destroy(gameObject);
            });
        }
        else
        {
            Manager.EventManager.InvokeADVEvent(ADVEventType.HideIndicator, null);
            endDragSequence = DOTween.Sequence();
            endDragSequence.Join(transform.DOLocalMove(InitialPosition, 0.3f).SetEase(returnEaseCurve));
            endDragSequence.Join(transform.DORotate(Vector2.zero, 0.3f).SetEase(returnEaseCurve));
            endDragSequence.Join(cardTextMask.DOFade(0f, 0.25f));
            endDragSequence.Join(cardText.DOFade(0f, 0.25f));
        }

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //onCardDrop.Invoke();
    }
}

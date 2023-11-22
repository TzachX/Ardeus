using ADV.Core.Components;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ADVResourceComponent : ADVMonoBehaviour
{
    [SerializeField] private Slider resourceSlider;
    [SerializeField] private ResourceType resource;
    [SerializeField] private Image indicator;
    [SerializeField] private Image fillImage;
    private Color initialColor;

    void Awake()
    {
        Manager.EventManager.AddListener(ADVEventType.CardChoiceMade, SetResourceValue);
        Manager.EventManager.AddListener(ADVEventType.Showresponse, SetIndicatorColor);
        Manager.EventManager.AddListener(ADVEventType.HideIndicator, HideIndicator);
        resourceSlider.value = 0;
        initialColor = fillImage.color;
    }

    void Start()
    {
        resourceSlider.DOValue(50, 4f);
    }

    private void SetResourceValue(object cardresponseObj)
    {
        indicator.DOFade(0, 0f);
        CardResponse cardresponse = cardresponseObj as CardResponse;
        
        if (cardresponse.effects.ContainsKey(resource)) 
        {
            float finalSliderValue = resourceSlider.value + cardresponse.effects[resource];

            if (finalSliderValue < resourceSlider.value)
            {
                DOVirtual.Color(Color.red, initialColor, 1f, (value) => { fillImage.color = value; });
            }
            else if (finalSliderValue > resourceSlider.value) 
            {
                DOVirtual.Color(Color.green, initialColor, 1f, (value) => { fillImage.color = value; });
            }
            
            resourceSlider.DOValue(finalSliderValue, 1f);
            
        }
    }

    private void SetIndicatorColor(object colorresponse)
    {
        Color color = ((Tuple<Color, CardResponse>)colorresponse).Item1;
        CardResponse response = ((Tuple<Color, CardResponse>)colorresponse).Item2;

        if (response.effects.ContainsKey(resource))
        {
            indicator.color = color;
        }
        else
        {
            indicator.DOFade(0, 0f);
        }
        
    }

    private void HideIndicator(object obj)
    {
        indicator.DOFade(0, 0.25f);
    }
}

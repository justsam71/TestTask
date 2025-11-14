using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GunHandler gunHandler;
    [SerializeField] private TMP_Text hoverText_bttn;
    [SerializeField] private TMP_Text hoverText_effect;

    private Color originalColor_bttn;
    private Color originalColor_effect;

    [SerializeField] private float pressedAlpha = 0.3f;   // альфа при нажатии
    [SerializeField] private float normalAlpha = 1f;  // альфа при отпускании


    private bool isHolding = false;

    private void Awake()
    {
        if (hoverText_bttn != null)
            originalColor_bttn = hoverText_bttn.color;

        if (hoverText_effect != null)
            originalColor_effect = hoverText_effect.color;
    }

    private void Update()
    {
        if (isHolding)
        {
            gunHandler.Shoot(); 
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        SetAlpha(pressedAlpha);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        SetAlpha(normalAlpha);
    }

    private void SetAlpha(float a)
    {
        if (hoverText_bttn != null)
        {
            var c = hoverText_bttn.color;
            c.a = a;
            hoverText_bttn.color = c;
        }

        if (hoverText_effect != null)
        {
            var c = hoverText_effect.color;
            c.a = a;
            hoverText_effect.color = c;
        }
    }


}

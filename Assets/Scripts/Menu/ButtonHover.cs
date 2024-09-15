using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    [Header("ObjectControl")]
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _textObj;
    [SerializeField] private GameObject _highlightSprite;

    private TextMeshProUGUI _text;
    private Image _highlightImg;

    private int _UILayer;

    [Header("Styling")]
    [SerializeField] private float _hoverOpacity = 0.6f;
    [SerializeField] private float _normalOpacity = 0.0f;


    private void Awake()
    {
        _UILayer = LayerMask.NameToLayer("UI");

        if (_highlightSprite != null) 
            _highlightImg = _highlightSprite.GetComponent<Image>();

        if (_textObj != null)
            _text = _textObj.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateHover(IsPointerOverUIElement());
    }

    private void UpdateHover(bool isHovering)
    {
        //if(isHovering)
        //{
        //    Debug.Log("Hovering over " + gameObject.name);
        //}

        // Change button style when hovering over it
        if (_text != null)
        {
            Color textColor = (isHovering) ? _button.colors.highlightedColor : _button.colors.normalColor;
            textColor.a = 1;
            _text.color = textColor;
        }

        if (_highlightImg != null)
        {
            Color color = _highlightImg.color.linear;
            color.a = (isHovering) ? _hoverOpacity : _normalOpacity;
            _highlightImg.color = color;
        }
        
    }


    // Found on https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/
    private bool IsPointerOverUIElement()
    {
        List<RaycastResult> eventSystemRaycastResults = GetEventSystemRaycastResults();

        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaycastResults[index];
            if (curRaysastResult.gameObject.layer == _UILayer && curRaysastResult.gameObject == this.gameObject)
                return true;
        }
        return false;
    }

    List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}

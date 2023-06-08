using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TabButton : Selectable, ISubmitHandler
{

    //Text color settings
    [SerializeField] private bool _useTextColor = false;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _textSwapColorSelected, _textSwapColorDeselected;

    //Graphic color settings
    [SerializeField] private bool _useColor = true;
    [SerializeField] private bool _onlyAlpha = false;
    [SerializeField] private Color _selectedColor, _deselectedColor;

    //Graphic sprite swap settings
    [SerializeField] private bool _useSpriteSwap = false;
    [SerializeField] private Sprite _buttonSelectedSprite;
    [SerializeField] private Sprite _buttonDeselectedSprite;

    /// <summary>
    /// Additional icon object
    /// </summary>
    [SerializeField] private Image _icon;

    /// <summary>
    /// Object to enable/disable 
    /// </summary>
    [SerializeField] private GameObject _objectToActivate;


    //TODO: implement sound
    /*        [SerializeField] private bool _useSound = true;

            [SerializeField]
            private FMODAudioEvent _hoverSoundEvent;

            [SerializeField]
            private FMODAudioEvent _clickSoundEvent;*/

    private TabGroup _tabGroup;


    /// <summary>
    /// Convenience function that converts the referenced Graphic to a Image, if possible.
    /// </summary>
    public Image Image
    {
        get => targetGraphic as Image;
        set => targetGraphic = value;
    }

    public TabGroup TabGroup
    {
        get
        {
            if (_tabGroup == null)
                _tabGroup = GetComponentInParent<TabGroup>();

            if (_tabGroup == null)
                throw new NullReferenceException("Can't find TabGroup in parents");
            return _tabGroup;
        }
    }

    public UnityEvent OnSelectedAction;
    public UnityEvent OnDeselectedAction;


    protected override void Awake()
    {
        base.Awake();

        if (Application.isPlaying)
        {
            SetButtonState(false);

            if (_objectToActivate != null)
            {
                _objectToActivate.SetActive(false);
                TabGroup.Subscribe(this, _objectToActivate);
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        //_hoverSoundEvent.Play();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        OnSubmit(null);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        TabGroup.OnTabSelected(this);
        /*            if (_useSound)
                        _clickSoundEvent.Play();*/
    }


    internal void SetButtonState(bool state)
    {
        if (state)
            OnSelectedAction?.Invoke();
        else
            OnDeselectedAction?.Invoke();

        if (_useColor)
            SetButtonColor(state);

        if (_useTextColor)
        {
            if (_text != null)
            {
                _text.color = state ? _textSwapColorSelected : _textSwapColorDeselected;
            }
        }

        if (state)
        {
            if (_useSpriteSwap)
            {
                Image.overrideSprite = _buttonSelectedSprite;
            }
        }
        else
        {
            if (_useSpriteSwap)
            {
                Image.overrideSprite = _buttonDeselectedSprite;
            }
        }
    }

    internal void SetButtonColor(bool state)
    {
        Color color = state ? _selectedColor : _deselectedColor;

        var buttonColor = targetGraphic.color;
        buttonColor.a = color.a;
        targetGraphic.color = _onlyAlpha ? buttonColor : color;
    }

    public void SetButtonImage(Sprite sprite)
    {
        if (_icon != null)
        {
            _icon.sprite = sprite;
        }
        else
        {
            Debug.LogError("Tried to set TabButton sprite while Icon object is null.");
        }
    }

}

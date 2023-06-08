using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopService;
using Random = UnityEngine.Random;

public class ItemSlotUiController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _itemName;

    [SerializeField]
    private RawImage _itemImage;

    [SerializeField]
    private Button _useButton;


    public void Init(NFTGameModel model, Action buttonAction)
    {
        _itemName.text = model.Name;
        _itemImage.gameObject.SetActive(model.Texture != null);
        _itemImage.texture = model.Texture;
        _useButton.onClick.AddListener(() => { buttonAction?.Invoke(); });
        _useButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        transform.DOKill(true);
        transform.DOBlendablePunchRotation(new Vector3(1, 0, 1) * 10, 0.3f).timeScale = 1;
    }


}

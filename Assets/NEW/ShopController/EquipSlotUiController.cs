using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopService;
using Random = UnityEngine.Random;

public class EquipSlotUiController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _itemType;

    [SerializeField]
    private TextMeshProUGUI _itemName;

    [SerializeField]
    private RawImage _itemImage;

    internal void SetNullInfo()
    {
        gameObject.SetActive(false);
    }

    internal void SetInfo(NFTGameModel model)
    {
        if (model == null)
        {
            SetNullInfo();
            return;
        }

        gameObject.SetActive(true);

        _itemImage.gameObject.SetActive(model.Texture != null);
        _itemImage.texture = model.Texture;
        _itemName.text = model.Name;
        _itemType.text = model switch
        {
            GunModel => "Gun",
            SkinModel => "Skin",
            _ => "MODEL ERROR"
        };

    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using static ShopService;
using Random = UnityEngine.Random;

public class ShopUiController : MonoBehaviour
{

    [SerializeField]
    private RectTransform _skinRoot;
    [SerializeField]
    private RectTransform _gunRoot;


    [SerializeField]
    private ItemSlotUiController _slotPrefab;

    [SerializeField]
    private EquipSlotUiController _equipSlotPrefab;

    [SerializeField]
    private TextMeshProUGUI _selectedItemHeader, _selectedItemDescription;

    [SerializeField]
    private RawImage _selectedItemRawImage;

    [SerializeField]
    private RectTransform _infoPanel;

    [SerializeField]
    private Button _infoEquipButton;

    [SerializeField]
    private RectTransform _equipSlotsRoot;





    private List<EquipSlotUiController> _slots = new List<EquipSlotUiController>();


    private List<ItemSlotUiController> _gunInstances = new List<ItemSlotUiController>();
    private List<ItemSlotUiController> _skinInstances = new List<ItemSlotUiController>();


    private ShopService _shopService;
    private PlayerConfigService _playerService;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(ShopService shopService, PlayerConfigService playerService, SignalBus signalBus)
    {
        _playerService = playerService;
        _shopService = shopService;
        _signalBus = signalBus;


        shopService.OnSkinsLoaded.Subscribe(UpdateSkins).AddTo(this);
        shopService.OnGunsLoaded.Subscribe(UpdateGuns).AddTo(this);

    }

    private void Awake()
    {
        Time.timeScale = 1;
        _infoPanel.localScale = new Vector3(1, 0, 1);

        for (int i = 0; i < 2; i++)
        {
            EquipSlotUiController newInstance = Instantiate(_equipSlotPrefab);
            newInstance.transform.SetParent(_equipSlotsRoot, false);
            _slots.Add(newInstance);
        }

        _slots[0].SetInfo(_playerService.GetSkin());
        _slots[1].SetInfo(_playerService.GetGun());

        UpdateSkins(_shopService.LoadedSkins);
        UpdateGuns(_shopService.LoadedGuns);
    }

    private void UpdateSkins(IReadOnlyList<SkinModel> skinModels)
    {
        foreach (var item in _skinInstances)
            Destroy(item.gameObject);
        _skinInstances.Clear();

        foreach (var model in skinModels)
        {
            ItemSlotUiController newInstance = Instantiate(_slotPrefab);

            newInstance.transform.SetParent(_skinRoot, false);
            newInstance.Init(model, () =>
            {
                OnItemClick(model);
            });
        }
    }

    private void UpdateGuns(IReadOnlyList<GunModel> gunModels)
    {
        foreach (var item in _gunInstances)
            Destroy(item.gameObject);
        _gunInstances.Clear();

        //create child controllers
        foreach (var model in gunModels)
        {
            ItemSlotUiController newInstance = Instantiate(_slotPrefab);

            newInstance.transform.SetParent(_gunRoot, false);
            newInstance.Init(model, () =>
            {
                OnItemClick(model);
            });

        }
    }


    private void OnItemClick(NFTGameModel model)
    {
        _selectedItemHeader.text = model.Name;
        _selectedItemDescription.text = model.GetGameDescription();

        _selectedItemRawImage.gameObject.SetActive(model.Texture != null);
        _selectedItemRawImage.texture = model.Texture;



        _infoPanel.DOKill();
        _infoPanel.DOScaleY(1, 0.3f).From(0).SetEase(Ease.InOutExpo).timeScale = 1;



        int _equipSlotIndex = model switch
        {
            GunModel => 1,
            SkinModel => 0,
            _ => throw new ArgumentException($"Invalid model: {model.GetType()}")
        };

        _infoEquipButton.onClick.RemoveAllListeners();
        _infoEquipButton.onClick.AddListener(() =>
        {
            EquipItem(model, _equipSlotIndex);
            _infoEquipButton.transform.DOBlendablePunchRotation(new Vector3(1, 0, 1) * 10, 0.75f).timeScale = 1;
            _slots[_equipSlotIndex].transform.DOBlendablePunchRotation(new Vector3(1, 0, 1) * 10, 0.75f).timeScale = 1;
        });
    }

    private void EquipItem(NFTGameModel model, int equipSlotIndex)
    {
        _slots[equipSlotIndex].SetInfo(model);

        //call services...
        _playerService.SetModel(model);

        _signalBus.Fire(new OnEqipmentChanged());
    }

}
